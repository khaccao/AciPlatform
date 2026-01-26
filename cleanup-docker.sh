#!/bin/bash

# Script dọn dẹp Docker images và containers

set -euo pipefail

echo "🧹 Bắt đầu dọn dẹp..."

# Remove unused containers
echo "🗑️  Xóa unused containers..."
docker container prune -f

# Remove unused images
echo "🖼️  Xóa unused images..."
docker image prune -f

# Remove unused volumes
echo "📦 Xóa unused volumes..."
docker volume prune -f

# Remove unused networks
echo "🌐 Xóa unused networks..."
docker network prune -f

echo "✅ Dọn dẹp hoàn tất!"

# Show disk usage
echo ""
echo "📊 Thông tin docker:"
docker system df
