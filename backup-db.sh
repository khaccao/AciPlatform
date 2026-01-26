#!/bin/bash

# Script backup database

set -euo pipefail

BACKUP_DIR="./backups"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="$BACKUP_DIR/AciPlatform_$TIMESTAMP.bak"

mkdir -p "$BACKUP_DIR"

echo "💾 Bắt đầu backup database..."
echo "Thư mục: $BACKUP_DIR"
echo "File: $BACKUP_FILE"

# Load env
set -a
source .env
set +a

# Dump database
docker exec aciplatform-db /opt/mssql-tools/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "$DB_SA_PASSWORD" \
    -Q "BACKUP DATABASE AciPlatform TO DISK = '/var/opt/mssql/backup/AciPlatform_$TIMESTAMP.bak'"

# Copy backup ra ngoài
docker cp "aciplatform-db:/var/opt/mssql/backup/AciPlatform_$TIMESTAMP.bak" "$BACKUP_FILE"

echo "✅ Backup hoàn tất!"
echo "📝 File: $BACKUP_FILE"

# Cleanup old backups (keep last 7 days)
echo "🧹 Dọn dẹp backups cũ..."
find "$BACKUP_DIR" -name "*.bak" -mtime +7 -delete

echo "✨ Xong!"
