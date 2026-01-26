#!/bin/bash

# Script cấu hình SSL Let's Encrypt trên VPS

set -euo pipefail

DOMAIN="${1:-sit-backend-nguyenbinh.info.vn}"
EMAIL="${2:-admin@nguyenbinh.info.vn}"
PROJECT_DIR="${3:-.}"

echo "🔒 Cấu hình SSL/HTTPS với Let's Encrypt"
echo "Domain: $DOMAIN"
echo "Email: $EMAIL"

# Kiểm tra Certbot
if ! command -v certbot &> /dev/null; then
    echo "📦 Cài đặt Certbot..."
    sudo apt-get update
    sudo apt-get install -y certbot python3-certbot-nginx
fi

# Dừng Nginx tạm thời
echo "⏹️  Dừng Nginx..."
cd "$PROJECT_DIR"
docker-compose stop nginx || true

# Tạo certificate
echo "🔑 Tạo certificate từ Let's Encrypt..."
sudo certbot certonly \
    --standalone \
    -d "$DOMAIN" \
    --agree-tos \
    --email "$EMAIL" \
    --non-interactive

# Copy certificates
echo "📋 Copy certificates vào project..."
SSL_DIR="$PROJECT_DIR/ssl"
mkdir -p "$SSL_DIR"

sudo cp "/etc/letsencrypt/live/$DOMAIN/fullchain.pem" "$SSL_DIR/cert.pem"
sudo cp "/etc/letsencrypt/live/$DOMAIN/privkey.pem" "$SSL_DIR/key.pem"
sudo chown "$USER:$USER" "$SSL_DIR"/{cert.pem,key.pem}
sudo chmod 600 "$SSL_DIR"/{cert.pem,key.pem}

# Khởi động lại Nginx
echo "▶️  Khởi động Nginx..."
cd "$PROJECT_DIR"
docker-compose up -d nginx

echo "✅ SSL/HTTPS đã được cấu hình thành công!"
echo "🌐 API có thể truy cập tại: https://$DOMAIN"

# Auto-renew setup
echo "⚙️  Cấu hình auto-renew..."
RENEW_HOOK="cd $PROJECT_DIR && docker-compose restart nginx"
sudo bash -c "echo '0 3 * * * /usr/bin/certbot renew --quiet --post-hook \"$RENEW_HOOK\"' | crontab -"

echo "✨ Hoàn tất!"
