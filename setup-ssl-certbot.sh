#!/bin/bash

# AciPlatform SSL Setup Script using Certbot (Docker)

DOMAIN_MAIN="nguyenbinh.info.vn"
DOMAIN_WWW="www.nguyenbinh.info.vn"
DOMAIN_API="sit.backend.nguyenbinh.info.vn"
EMAIL="admin@nguyenbinh.info.vn"

# 1. Start Nginx container to serve the .well-known challenge
docker-compose up -d nginx

# 2. Request certificates
docker run -it --rm --name certbot \
  -v "$(pwd)/data/certbot/conf:/etc/letsencrypt" \
  -v "$(pwd)/data/certbot/www:/var/www/certbot" \
  certbot/certbot certonly --webroot -w /var/www/certbot \
  --email $EMAIL --agree-tos --no-eff-email \
  -d $DOMAIN_MAIN -d $DOMAIN_WWW -d $DOMAIN_API

# 3. Reload Nginx
docker-compose exec nginx nginx -s reload
