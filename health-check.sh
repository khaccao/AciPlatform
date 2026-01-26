#!/bin/bash

# Script kiểm tra health check và hiển thị metrics

set -euo pipefail

echo "🏥 Kiểm tra Health Status"
echo "========================"

# Load env
set -a
source .env 2>/dev/null || true
set +a

API_URL="http://localhost:${API_PORT:-8080}"
DOMAIN="${DOMAIN_NAME:-sit-backend-nguyenbinh.info.vn}"

echo ""
echo "📊 Docker Containers Status:"
echo "----------------------------"
docker-compose ps

echo ""
echo "🔍 Health Check Endpoints:"
echo "---------------------------"

# API health
echo -n "API Health: "
if curl -sf "$API_URL/health" > /dev/null; then
    echo "✅ OK"
else
    echo "❌ FAILED"
fi

# API swagger
echo -n "API Swagger: "
if curl -sf "$API_URL/swagger" > /dev/null; then
    echo "✅ OK"
else
    echo "❌ FAILED"
fi

echo ""
echo "📈 Docker Metrics:"
echo "------------------"
docker stats --no-stream --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}"

echo ""
echo "📝 Latest Logs (last 20 lines):"
echo "-------------------------------"
docker-compose logs --tail=20

echo ""
echo "✅ Health check selesai"
