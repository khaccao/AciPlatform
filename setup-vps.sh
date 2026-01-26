#!/bin/bash

# AciPlatform VPS Setup Script
# Chỉ setup files cấu hình, không clone repository

set -euo pipefail

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if running as root
if [ "$EUID" -eq 0 ]; then
   log_error "Do not run this script as root!"
   exit 1
fi

log_info "🚀 AciPlatform VPS Setup (Non-Git Version)"
log_info "=========================================="

# Check requirements
log_info "Checking requirements..."

if ! command -v docker &> /dev/null; then
    log_error "Docker is not installed"
    log_info "Install Docker: curl -fsSL https://get.docker.com -o get-docker.sh && sudo sh get-docker.sh"
    exit 1
fi

if ! command -v docker-compose &> /dev/null; then
    log_error "Docker Compose is not installed"
    log_info "Install Docker Compose: https://docs.docker.com/compose/install/"
    exit 1
fi

log_info "✅ Docker and Docker Compose are installed"

# Create directories
log_info "Creating directories..."
mkdir -p ssl logs logs/nginx backups

log_info "✅ Directories created"

# Create .env if not exists
if [ ! -f .env ]; then
    log_warn ".env file not found"
    
    # Try to create from template
    if [ -f .env.example ]; then
        log_info "Copying .env.example to .env..."
        cp .env.example .env
        log_warn "⚠️  Please edit .env with your configuration!"
        log_warn "Run: nano .env"
        exit 1
    else
        log_error ".env.example not found"
        log_info "Please copy .env.example file to VPS"
        exit 1
    fi
fi

log_info "✅ .env file found"

# Create docker-compose.yml if not exists
if [ ! -f docker-compose.yml ]; then
    log_error "docker-compose.yml not found"
    log_info "Please copy docker-compose.yml file to VPS"
    exit 1
fi

log_info "✅ docker-compose.yml found"

# Create nginx.conf if not exists
if [ ! -f nginx.conf ]; then
    log_error "nginx.conf not found"
    log_info "Please copy nginx.conf file to VPS"
    exit 1
fi

log_info "✅ nginx.conf found"

# Setup SSL certificates
if [ ! -f ssl/cert.pem ] || [ ! -f ssl/key.pem ]; then
    log_warn "SSL certificates not found"
    log_info "Creating self-signed certificates for testing..."
    
    openssl req -x509 -newkey rsa:4096 -keyout ssl/key.pem -out ssl/cert.pem \
        -days 365 -nodes -subj "/CN=sit-backend-nguyenbinh.info.vn" 2>/dev/null
    
    chmod 600 ssl/{cert.pem,key.pem}
    log_warn "⚠️  Self-signed certificates created"
    log_warn "For production, use Let's Encrypt: https://letsencrypt.org"
else
    log_info "✅ SSL certificates found"
fi

# Login to GitHub Container Registry if needed
log_info "Checking Docker registry access..."

# Load env vars
set -a
source .env
set +a

# Verify configuration
log_info "Verifying configuration..."

if [ -z "${DOCKER_REGISTRY:-}" ]; then
    log_error "DOCKER_REGISTRY not set in .env"
    exit 1
fi

if [ -z "${DOCKER_NAMESPACE:-}" ]; then
    log_error "DOCKER_NAMESPACE not set in .env"
    exit 1
fi

if [ -z "${DOCKER_IMAGE_NAME:-}" ]; then
    log_error "DOCKER_IMAGE_NAME not set in .env"
    exit 1
fi

log_info "✅ Configuration verified"

# Test pull (without actually pulling)
IMAGE="${DOCKER_REGISTRY}/${DOCKER_NAMESPACE}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_TAG:-latest}"
log_info "Image to pull: $IMAGE"

log_info ""
log_info "✅ Setup Complete!"
log_info ""
log_info "Next steps:"
log_info "1. Verify .env configuration: nano .env"
log_info "2. Pull image and start services: docker-compose pull && docker-compose up -d"
log_info "3. Check status: docker-compose ps"
log_info "4. View logs: docker-compose logs -f"
log_info ""
log_info "Quick commands:"
log_info "  docker-compose logs -f           # View logs"
log_info "  docker-compose ps                # Service status"
log_info "  docker-compose down              # Stop services"
log_info "  docker-compose up -d             # Start services"
log_info ""
