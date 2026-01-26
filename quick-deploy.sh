#!/bin/bash

# Quick Deploy Script for VPS
# Just pull latest image and restart services

set -euo pipefail

echo "🚀 Quick Deploy"
echo "=============="

# Load env
set -a
source .env
set +a

echo "Pulling latest image..."
docker-compose pull

echo "Starting services..."
docker-compose up -d

echo "Waiting for services to be ready..."
sleep 5

echo ""
echo "Service status:"
docker-compose ps

echo ""
echo "Testing health check..."
if curl -sf http://localhost:8080/health > /dev/null; then
    echo "✅ Health check passed"
else
    echo "⚠️  Health check not ready yet, checking logs..."
    docker-compose logs --tail=20
fi

echo ""
echo "✅ Deploy complete!"
echo "API: https://sit-backend-nguyenbinh.info.vn"
