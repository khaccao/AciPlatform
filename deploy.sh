#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Set error handling
set -euo pipefail

# Configuration
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="${SCRIPT_DIR}"
DOCKER_REGISTRY="${DOCKER_REGISTRY:-ghcr.io}"
GITHUB_USERNAME="${GITHUB_USERNAME:-}"
IMAGE_NAME="${DOCKER_IMAGE_NAME:-aciplatform-api}"
IMAGE_TAG="${DOCKER_IMAGE_TAG:-latest}"

# Functions
log_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

check_requirements() {
    log_info "Checking requirements..."
    
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose is not installed"
        exit 1
    fi
    
    if ! command -v git &> /dev/null; then
        log_error "Git is not installed"
        exit 1
    fi
    
    log_info "All requirements met"
}

setup_environment() {
    log_info "Setting up environment..."
    
    if [ ! -f "${PROJECT_ROOT}/.env" ]; then
        log_warn ".env file not found. Copying from .env.example..."
        cp "${PROJECT_ROOT}/.env.example" "${PROJECT_ROOT}/.env"
        log_warn "Please edit .env file with your configuration and run deploy again"
        exit 1
    fi
    
    # Load environment variables
    set -a
    source "${PROJECT_ROOT}/.env"
    set +a
    
    log_info "Environment loaded successfully"
}

build_image() {
    log_info "Building Docker image..."
    
    cd "${PROJECT_ROOT}"
    
    docker build \
        -t "${IMAGE_NAME}:${IMAGE_TAG}" \
        -t "${IMAGE_NAME}:latest" \
        -f Dockerfile \
        .
    
    log_info "Docker image built successfully"
}

push_image() {
    log_info "Pushing image to registry..."
    
    if [ -z "${GITHUB_USERNAME}" ]; then
        log_warn "GITHUB_USERNAME not set. Skipping registry push"
        return 0
    fi
    
    REGISTRY_IMAGE="${DOCKER_REGISTRY}/${GITHUB_USERNAME}/${IMAGE_NAME}"
    
    docker tag "${IMAGE_NAME}:${IMAGE_TAG}" "${REGISTRY_IMAGE}:${IMAGE_TAG}"
    docker tag "${IMAGE_NAME}:latest" "${REGISTRY_IMAGE}:latest"
    
    # Login to registry (requires GITHUB_TOKEN env var)
    if [ -z "${GITHUB_TOKEN:-}" ]; then
        log_warn "GITHUB_TOKEN not set. Push operation requires manual login"
        return 0
    fi
    
    echo "${GITHUB_TOKEN}" | docker login "${DOCKER_REGISTRY}" -u "${GITHUB_USERNAME}" --password-stdin
    
    docker push "${REGISTRY_IMAGE}:${IMAGE_TAG}"
    docker push "${REGISTRY_IMAGE}:latest"
    
    log_info "Image pushed successfully"
}

start_services() {
    log_info "Starting services..."
    
    cd "${PROJECT_ROOT}"
    
    # Create necessary directories
    mkdir -p logs logs/nginx ssl
    
    # Start services
    docker-compose down || true
    docker-compose up -d
    
    sleep 10
    
    # Check health
    if docker-compose ps | grep -q "unhealthy"; then
        log_error "Some services are unhealthy"
        docker-compose logs
        exit 1
    fi
    
    log_info "Services started successfully"
}

verify_deployment() {
    log_info "Verifying deployment..."
    
    # Check if API is responding
    sleep 5
    
    if curl -sf http://localhost:${API_PORT:-8080}/health > /dev/null; then
        log_info "Health check passed"
    else
        log_warn "Health check failed, but continuing..."
    fi
    
    # Display service status
    log_info "Service status:"
    docker-compose ps
}

setup_ssl() {
    log_info "Setting up SSL certificates..."
    
    CERT_DIR="${PROJECT_ROOT}/ssl"
    mkdir -p "${CERT_DIR}"
    
    if [ ! -f "${CERT_DIR}/cert.pem" ] || [ ! -f "${CERT_DIR}/key.pem" ]; then
        log_warn "SSL certificates not found. Creating self-signed certificates for testing..."
        
        openssl req -x509 -newkey rsa:4096 -keyout "${CERT_DIR}/key.pem" -out "${CERT_DIR}/cert.pem" \
            -days 365 -nodes -subj "/CN=${DOMAIN_NAME}"
        
        log_info "Self-signed certificates created. Please replace with valid certificates for production"
    fi
    
    chmod 600 "${CERT_DIR}"/{cert.pem,key.pem}
}

main() {
    log_info "Starting AciPlatform API deployment..."
    log_info "Image: ${IMAGE_NAME}:${IMAGE_TAG}"
    
    check_requirements
    setup_environment
    setup_ssl
    build_image
    push_image
    start_services
    verify_deployment
    
    log_info "Deployment completed successfully!"
    log_info "API is available at: https://${DOMAIN_NAME}"
    log_info "View logs with: docker-compose logs -f"
}

main "$@"
