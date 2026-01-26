# 🚀 AciPlatform API - Hướng Dẫn Deployment Hoàn Chỉnh

**Domain:** `sit-backend-nguyenbinh.info.vn`  
**Database:** SQL Server 2022  
**Framework:** .NET 9.0  
**Deployment:** Docker + GitHub Actions (Staging Branch)

---

## 📋 Mục Lục

1. [Quick Start (5 phút)](#-quick-start)
2. [Yêu Cầu Hệ Thống](#-yêu-cầu)
3. [Local Development](#-local-development)
4. [Production Deployment](#-production-deployment)
5. [CI/CD Setup](#-cicd-setup)
6. [Monitoring & Maintenance](#-monitoring--maintenance)
7. [Troubleshooting](#-troubleshooting)

---

## ⚡ Quick Start

### Cách 1: Local Development (5 phút)

**Important:** Use **PowerShell** on Windows (not Git Bash)

```powershell
# Windows (PowerShell)
.\dev.ps1 setup
.\dev.ps1 start

# macOS/Linux
docker-compose -f docker-compose.dev.yml up -d
```

**API:** `http://localhost:5000`

### Cách 2: VPS Deployment (5 phút)

```bash
# Copy files to VPS
scp docker-compose.yml user@vps-ip:~/aciplatform-api/
scp .env user@vps-ip:~/aciplatform-api/
scp nginx.conf user@vps-ip:~/aciplatform-api/
scp setup-vps.sh user@vps-ip:~/aciplatform-api/

# SSH to VPS
ssh user@vps-ip
cd ~/aciplatform-api

# Setup & Run
chmod +x setup-vps.sh
./setup-vps.sh
docker-compose pull
docker-compose up -d
```

**API:** `https://sit-backend-nguyenbinh.info.vn`

---

## 📋 Yêu Cầu

### Máy Tính Local (Development)

- Docker & Docker Compose
- .NET 9.0 SDK
- Git
- VS Code (tuỳ chọn)

### Vietnix VPS (Production)

- VPS với 2GB+ RAM
- Docker đã cài đặt
- Docker Compose đã cài đặt
- SSH access
- Domain: `sit-backend-nguyenbinh.info.vn`

---

## 🖥️ Local Development

### Important: Use PowerShell on Windows

Git Bash does **NOT** support PowerShell scripts. You must use:
- **Windows:** PowerShell or PowerShell ISE (NOT Git Bash)
- **macOS/Linux:** bash or zsh

To open PowerShell in your project folder:
```powershell
# Right-click in Windows Explorer → Open PowerShell here
# Or from VS Code: Terminal → New Terminal → Select "PowerShell"
```

### 1. Chuẩn Bị

```bash
# Clone repository
git clone https://github.com/YOUR_USERNAME/AciPlatform.git
cd AciPlatform
```

### 2. Start Database

#### Windows (PowerShell):
```powershell
.\dev.ps1 setup
.\dev.ps1 start
```

**What this does:**
- ✅ Checks Docker/Docker Compose/SDK
- ✅ Starts SQL Server container
- ✅ Waits for database to be ready
- ✅ Runs EF migrations
- ✅ Starts .NET API with hot reload

#### macOS/Linux:
```bash
docker-compose -f docker-compose.dev.yml up -d
```

### 3. Database Connection

```
Server: localhost,1433
Database: AciPlatform
User: sa
Password: Dev@12345
```

### 4. Run Application (macOS/Linux only)

For Windows, `dev.ps1 start` handles this. For macOS/Linux:

```bash
cd AciPlatform.Api

# Apply migrations
dotnet ef database update

# Run with hot reload
dotnet watch run
```

**Result:** API available at `http://localhost:5000`

### 5. Test API

```bash
# Health check
curl http://localhost:5000/health

# Swagger docs
open http://localhost:5000/swagger
```

### Useful Commands

```powershell
# Windows (PowerShell)
.\dev.ps1 stop                        # Stop everything
.\dev.ps1 logs                        # View database logs
.\dev.ps1 clean                       # Remove containers/volumes
.\dev.ps1 help                        # Show all commands

# macOS/Linux
docker-compose -f docker-compose.dev.yml down      # Stop
docker-compose -f docker-compose.dev.yml logs -f   # Logs
docker-compose -f docker-compose.dev.yml down -v   # Clean
```

---

## 🚀 Production Deployment

### Architecture

```
Domain: sit-backend-nguyenbinh.info.vn (HTTPS)
    ↓
Nginx Reverse Proxy (Port 443)
    ↓
.NET 9.0 API Container (Port 8080)
    ↓
SQL Server 2022 Container (Port 1433)

All running on Vietnix VPS via Docker Compose
```

### Step-by-Step Setup

#### Bước 1: Chuẩn Bị VPS

**SSH vào VPS:**
```bash
ssh user@your-vps-ip
```

**Cài Docker (nếu chưa có):**
```bash
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Cấp quyền Docker cho user
sudo usermod -aG docker $USER
newgrp docker
```

**Cài Docker Compose:**
```bash
sudo curl -L "https://github.com/docker/compose/releases/download/v2.20.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
```

**Tạo thư mục project:**
```bash
mkdir -p ~/aciplatform-api
cd ~/aciplatform-api
```

#### Bước 2: Copy Files (Không Clone!)

**Từ máy local, copy files cấu hình:**

```bash
scp docker-compose.yml user@your-vps-ip:~/aciplatform-api/
scp .env user@your-vps-ip:~/aciplatform-api/
scp nginx.conf user@your-vps-ip:~/aciplatform-api/
scp setup-vps.sh user@your-vps-ip:~/aciplatform-api/
scp quick-deploy.sh user@your-vps-ip:~/aciplatform-api/
```

**Files cần copy:**
- `docker-compose.yml` - Docker Compose configuration
- `.env` - Environment variables (YOUR configured values)
- `nginx.conf` - Nginx reverse proxy config
- `setup-vps.sh` - Setup script
- `quick-deploy.sh` - Quick deploy script

#### Bước 3: Cấu Hình .env

**Edit .env trước khi copy:**

```bash
# Application
ASPNETCORE_ENVIRONMENT=Production

# Docker Image (từ GitHub Container Registry)
DOCKER_REGISTRY=ghcr.io
DOCKER_NAMESPACE=your-github-username
DOCKER_IMAGE_NAME=aciplatform-api
DOCKER_IMAGE_TAG=latest

# Port
API_PORT=8080

# Database
DB_PORT=1433
DB_CONNECTION_STRING=Server=14.225.212.145,1433;Database=AciPlatform;User Id=dev_user1;Password=Dev@123456;TrustServerCertificate=True;
DB_SA_PASSWORD=YourStrongPassword123!

# JWT
JWT_SECRET=SuperSecretKeyForAciPlatform_MustBeVeryLongString_12345!!!
JWT_ISSUER=AciPlatform
JWT_AUDIENCE=AciPlatformUsers
JWT_EXPIRE_HOURS=8

# Domain
DOMAIN_NAME=sit-backend-nguyenbinh.info.vn
CERT_EMAIL=admin@nguyenbinh.info.vn
```

**Important:** Change all passwords và secrets!

#### Bước 4: Cấu Hình DNS

**Tại Vietnix Control Panel hoặc DNS provider:**

```
Type: A
Name: sit-backend-nguyenbinh.info.vn
Value: YOUR_VPS_IP_ADDRESS
TTL: 3600
```

**Verify DNS:**
```bash
nslookup sit-backend-nguyenbinh.info.vn
```

#### Bước 5: Setup VPS

**Trên VPS:**
```bash
cd ~/aciplatform-api

# Setup
chmod +x setup-vps.sh
./setup-vps.sh

# Output:
# ✅ Docker and Docker Compose are installed
# ✅ Directories created
# ✅ .env file found
# ✅ docker-compose.yml found
# ✅ nginx.conf found
```

**Script này sẽ:**
- ✅ Kiểm tra Docker/Docker Compose
- ✅ Tạo directories (ssl, logs)
- ✅ Create self-signed SSL certificates
- ✅ Verify configuration

#### Bước 6: Start Services

```bash
cd ~/aciplatform-api

# Pull image từ GitHub Container Registry
docker-compose pull

# Start all services
docker-compose up -d
```

**Docker sẽ:**
1. Pull API image từ ghcr.io
2. Pull SQL Server image
3. Start Nginx container
4. Start API container
5. Start Database container

#### Bước 7: Verify Deployment

```bash
# Check services status
docker-compose ps

# Output should show:
# NAME                     STATUS
# aciplatform-api          Up (healthy)
# aciplatform-db           Up (healthy)
# aciplatform-nginx        Up

# View logs
docker-compose logs -f

# Health check
curl https://sit-backend-nguyenbinh.info.vn/health

# Should return HTTP 200
```

---

## 🔐 SSL/HTTPS Configuration

### Option 1: Self-Signed (Testing)

**VPS tự động tạo self-signed certs:**
```bash
# setup-vps.sh sẽ tạo nếu chưa có
# ssl/cert.pem
# ssl/key.pem
```

### Option 2: Let's Encrypt (Production - Recommended)

**Cài Certbot:**
```bash
sudo apt-get update
sudo apt-get install certbot python3-certbot-nginx
```

**Generate certificate:**
```bash
sudo certbot certonly --standalone \
  -d sit-backend-nguyenbinh.info.vn \
  --agree-tos \
  --email admin@nguyenbinh.info.vn
```

**Copy to project:**
```bash
cd ~/aciplatform-api
sudo cp /etc/letsencrypt/live/sit-backend-nguyenbinh.info.vn/fullchain.pem ssl/cert.pem
sudo cp /etc/letsencrypt/live/sit-backend-nguyenbinh.info.vn/privkey.pem ssl/key.pem
sudo chown $USER:$USER ssl/*
sudo chmod 600 ssl/*
```

**Restart Nginx:**
```bash
docker-compose restart nginx
```

**Auto-renew:**
```bash
# Certbot auto-renews daily
# Verify:
sudo certbot renew --dry-run
```

---

## 🔄 CI/CD Setup (GitHub Actions)

### Why Staging Branch?

- `main` branch: Production release (no auto-deploy)
- `develop`: Development (no auto-deploy)
- `staging` branch: Auto-deploy to VPS ✅

### Step 1: Setup GitHub Repository

```bash
git remote add origin https://github.com/YOUR_USERNAME/AciPlatform.git
git branch -M main
git push -u origin main
```

### Step 2: Create Staging Branch

```bash
git checkout -b staging
git push -u origin staging
```

### Step 3: Create SSH Key for VPS

**On your local machine:**
```bash
ssh-keygen -t rsa -b 4096 -f ~/.ssh/vps_key -N ""

# Add public key to VPS
ssh-copy-id -i ~/.ssh/vps_key.pub user@your-vps-ip
```

### Step 4: Add GitHub Secrets

**Go to:** Repository → Settings → Secrets and Variables → Actions

**Add these secrets:**

| Secret | Value |
|--------|-------|
| VPS_HOST | your-vps-ip-or-domain |
| VPS_USER | your-ssh-username |
| VPS_SSH_KEY | Content of `~/.ssh/vps_key` |

**Copy private key:**
```bash
cat ~/.ssh/vps_key  # Copy entire content
```

### Step 5: How CI/CD Works

**When you push to `staging` branch:**

```
1. GitHub Actions triggers automatically
   ↓
2. Checkout code
   ↓
3. Build & Test (.NET)
   ↓
4. Build Docker image
   ↓
5. Push to ghcr.io (GitHub Container Registry)
   ↓
6. SSH to VPS
   ↓
7. docker-compose pull (get new image)
   ↓
8. docker-compose up -d (restart services)
   ↓
9. Health check
   ↓
10. ✅ Deployed!
```

### Step 6: Workflow Example

**Development workflow:**

```bash
# 1. Create feature branch
git checkout -b feature/something develop

# 2. Make changes and test locally
# (use .\dev.ps1 for local testing)

# 3. Push to GitHub
git push origin feature/something

# 4. Create Pull Request to develop

# 5. After review, merge to staging
git checkout staging
git merge feature/something
git push origin staging

# 6. GitHub Actions automatically:
#    - Builds Docker image
#    - Pushes to ghcr.io
#    - Deploys to VPS
#    - Restarts services

# 7. Monitor deployment
# GitHub: https://github.com/YOUR_USERNAME/AciPlatform/actions
# VPS: docker-compose logs -f
```

### Workflow Files

The `.github/workflows/deploy.yml` is already configured:
- ✅ Triggers on `staging` branch push only
- ✅ Builds Docker image
- ✅ Pushes to ghcr.io
- ✅ Deploys to VPS (via SSH)
- ✅ Restarts services
- ✅ Checks health

---

## 📊 Monitoring & Maintenance

### Daily Operations on VPS

```bash
# Check service status
docker-compose ps

# View logs (last 50 lines)
docker-compose logs --tail=50

# View real-time logs
docker-compose logs -f

# Health check
curl https://sit-backend-nguyenbinh.info.vn/health
```

### Update to New Version

```bash
# When new image is pushed to ghcr.io
cd ~/aciplatform-api

# Option 1: Manual update
docker-compose pull
docker-compose restart

# Option 2: Use script
./quick-deploy.sh

# Verify
docker-compose ps
docker-compose logs --tail=20
```

### Database Backup

```bash
# Run backup script
./backup-db.sh

# Backup saved to: ./backups/AciPlatform_*.bak

# Check backups
ls -lh backups/
```

### Health Check

```bash
# Use health check script
./health-check.sh

# Or manual check
curl -I https://sit-backend-nguyenbinh.info.vn/health
curl https://sit-backend-nguyenbinh.info.vn/swagger
```

### View Service Logs

```bash
# API logs
docker-compose logs -f aciplatform-api

# Database logs
docker-compose logs -f db

# Nginx logs
docker-compose logs -f nginx

# All logs
docker-compose logs -f
```

### Docker Cleanup (Optional)

```bash
# Remove unused containers/images
docker system prune -a -f

# Or use script
./cleanup-docker.sh
```

### Scheduled Tasks (Cron)

**Setup daily backup (on VPS):**
```bash
crontab -e

# Add this line:
0 2 * * * cd ~/aciplatform-api && bash backup-db.sh
```

**Setup daily health check:**
```bash
# Add this line to crontab:
*/5 * * * * cd ~/aciplatform-api && bash health-check.sh >> logs/health.log 2>&1
```

---

## 🔐 Security Checklist

### Before Production

- [ ] Change all passwords:
  - [ ] `JWT_SECRET` - Unique & long (32+ chars)
  - [ ] `DB_SA_PASSWORD` - Strong password (uppercase, numbers, symbols)
  
- [ ] SSL/HTTPS:
  - [ ] Use Let's Encrypt (not self-signed)
  - [ ] Certificate valid and renewed automatically
  
- [ ] Firewall:
  - [ ] Port 22 (SSH) - Restricted to known IPs
  - [ ] Port 80 (HTTP) - Open
  - [ ] Port 443 (HTTPS) - Open
  - [ ] Port 1433 (Database) - Internal only

- [ ] Backup:
  - [ ] Tested backup & restore process
  - [ ] Automatic daily backups configured

- [ ] Monitoring:
  - [ ] Health checks running
  - [ ] Logs reviewed regularly

### Firewall Rules

```bash
# Allow SSH (restrict to your IP)
sudo ufw allow from YOUR_IP to any port 22

# Allow HTTP/HTTPS
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Deny database access from outside
# (Already configured - docker internal network)

# Enable firewall
sudo ufw enable
```

---

## 🐛 Troubleshooting

### API Not Responding

```bash
# 1. Check services
docker-compose ps

# 2. View logs
docker-compose logs aciplatform-api

# 3. Restart API
docker-compose restart aciplatform-api

# 4. Health check
curl http://localhost:8080/health
```

### Database Connection Error

```bash
# 1. Check database logs
docker-compose logs db

# 2. Verify connection string in .env
cat .env | grep DB_CONNECTION

# 3. Test connection
docker-compose exec db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa

# 4. Restart database
docker-compose restart db
```

### SSL Certificate Error

```bash
# 1. Check certificates exist
ls -la ssl/

# 2. View Nginx logs
docker-compose logs nginx

# 3. Regenerate self-signed cert
openssl req -x509 -newkey rsa:4096 -keyout ssl/key.pem -out ssl/cert.pem \
  -days 365 -nodes -subj "/CN=sit-backend-nguyenbinh.info.vn"

# 4. Restart Nginx
docker-compose restart nginx
```

### Port Already in Use

```bash
# Find process using port
lsof -i :8080

# Kill process (if needed)
kill -9 PID

# Or change port in .env
API_PORT=8081
docker-compose restart
```

### Image Pull Error (ghcr.io)

```bash
# Check authentication
docker login ghcr.io -u your-username -p your-token

# Try manual pull
docker pull ghcr.io/your-username/aciplatform-api:latest

# Update .env with correct namespace
cat .env | grep DOCKER_NAMESPACE
```

### Out of Disk Space

```bash
# Check disk usage
df -h

# Check Docker disk usage
docker system df

# Clean up
docker system prune -a -f
./cleanup-docker.sh
```

---

## 📁 Project Structure

```
AciPlatform/
├── AciPlatform.Api/              # Main API project
├── AciPlatform.Application/      # Application services
├── AciPlatform.Domain/           # Domain models
├── AciPlatform.Infrastructure/   # Data access layer
│
├── Dockerfile                    # Docker image definition
├── docker-compose.yml            # Production services
├── docker-compose.dev.yml        # Development database
├── nginx.conf                    # Nginx reverse proxy
│
├── .env.example                  # Environment template
├── .github/
│   └── workflows/
│       ├── deploy.yml            # GitHub Actions CI/CD
│       └── security.yml          # Security scanning
│
├── setup-vps.sh                  # VPS setup script
├── quick-deploy.sh               # Quick deploy script
├── backup-db.sh                  # Database backup
├── health-check.sh               # Health monitoring
├── cleanup-docker.sh             # Docker cleanup
├── dev.ps1                       # Windows development helper
│
└── README.md                     # This file
```

---

## ✅ Deployment Checklist

### Pre-Deployment

- [ ] All code committed to Git
- [ ] Tests passing locally
- [ ] No hardcoded secrets in code
- [ ] `.env.example` has safe defaults
- [ ] `.gitignore` includes `.env` and sensitive files

### VPS Setup

- [ ] Vietnix VPS ready (2GB+ RAM)
- [ ] Docker installed
- [ ] Docker Compose installed
- [ ] SSH access configured
- [ ] Directory created: `~/aciplatform-api`

### Configuration

- [ ] `.env` file copied to VPS
- [ ] All passwords changed to strong values
- [ ] JWT_SECRET set to unique value
- [ ] DOMAIN_NAME set correctly
- [ ] DOCKER_NAMESPACE set to GitHub username

### Domain & DNS

- [ ] Domain `sit-backend-nguyenbinh.info.vn` registered
- [ ] A record configured at DNS provider
- [ ] DNS propagated (5-10 minutes)
- [ ] `nslookup` returns correct IP

### SSL/HTTPS

- [ ] SSL certificates generated (Let's Encrypt or self-signed)
- [ ] Certificates placed in `ssl/` directory
- [ ] Certificate paths correct in `nginx.conf`

### Deployment

- [ ] Files copied to VPS
- [ ] `setup-vps.sh` executed successfully
- [ ] `docker-compose pull` completed
- [ ] `docker-compose up -d` started services
- [ ] `docker-compose ps` shows all services "Up"

### Verification

- [ ] Health check passes: `/health` returns 200
- [ ] Swagger docs accessible: `/swagger`
- [ ] HTTPS working (no certificate warnings)
- [ ] Database connected
- [ ] API responding to requests
- [ ] Logs show no errors

### CI/CD (Optional)

- [ ] GitHub repository created
- [ ] SSH key generated
- [ ] GitHub Secrets added (VPS_HOST, VPS_USER, VPS_SSH_KEY)
- [ ] Workflow tested with push to `staging`
- [ ] Auto-deployment working

---

## 🎯 Success Criteria

After deployment, verify:

✅ API accessible: `https://sit-backend-nguyenbinh.info.vn/`  
✅ Health check: `https://sit-backend-nguyenbinh.info.vn/health` (HTTP 200)  
✅ HTTPS valid: Green lock in browser  
✅ Swagger: `https://sit-backend-nguyenbinh.info.vn/swagger`  
✅ Database working: API returns data  
✅ Services running: `docker-compose ps` all "Up"  
✅ Logs clean: `docker-compose logs` no errors  
✅ Health checks passing: Every 30 seconds  

---

## 📞 Quick Reference

### Common Commands

```bash
# Service management
docker-compose ps                     # Status
docker-compose logs -f                # Real-time logs
docker-compose restart                # Restart
docker-compose up -d                  # Start
docker-compose down                   # Stop

# Update deployment
docker-compose pull                   # Get new image
docker-compose restart                # Restart services
./quick-deploy.sh                     # One-command deploy

# Maintenance
./backup-db.sh                        # Backup database
./health-check.sh                     # Health check
./cleanup-docker.sh                   # Cleanup Docker

# SSH to VPS
ssh user@your-vps-ip

# Deploy script
chmod +x *.sh
./setup-vps.sh                        # First time setup
./quick-deploy.sh                     # Regular deploy
```

### File Locations

```bash
# Configuration
~/.env                                # Environment variables
~/docker-compose.yml                  # Services definition
~/nginx.conf                          # Web server config

# Data
~/ssl/cert.pem                        # SSL certificate
~/ssl/key.pem                         # SSL private key
~/logs/                               # Application logs
~/backups/                            # Database backups
```

---

## 🎓 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [GitHub Actions](https://github.com/features/actions)
- [Nginx Documentation](https://nginx.org/en/docs/)
- [Let's Encrypt](https://letsencrypt.org/)
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)

---

## 📞 Support

**If you encounter issues:**

1. Check logs: `docker-compose logs -f`
2. Review [Troubleshooting](#-troubleshooting) section
3. Verify `.env` configuration
4. Check DNS: `nslookup sit-backend-nguyenbinh.info.vn`
5. Test health: `curl https://sit-backend-nguyenbinh.info.vn/health`

---

## 🚀 Summary

| Step | Time | Action |
|------|------|--------|
| 1 | 5 min | Copy files to VPS |
| 2 | 2 min | Setup VPS with `setup-vps.sh` |
| 3 | 2 min | Pull image & start: `docker-compose pull && docker-compose up -d` |
| 4 | 1 min | Verify: `docker-compose ps` |
| **Total** | **10 min** | **VPS Running!** ✅ |

---

**Status:** ✅ Production Ready

**Domain:** sit-backend-nguyenbinh.info.vn

**Framework:** .NET 9.0 + Docker + GitHub Actions

**Last Updated:** January 26, 2026

---

## 🎉 Let's Deploy!

Everything you need is ready. Follow the [Production Deployment](#-production-deployment) section step-by-step.

**Good luck! 🚀**

**Quan trọng:** Không có reference ngược lại!

## 🛠️ Công nghệ sử dụng

- **.NET 9**
- **Entity Framework Core 9.0.1** - ORM
- **SQL Server** - Database
- **ASP.NET Core** - Web API

## 📦 NuGet Packages

### AciPlatform.Infrastructure
- `Microsoft.EntityFrameworkCore` (9.0.1)
- `Microsoft.EntityFrameworkCore.SqlServer` (9.0.1)
- `Microsoft.EntityFrameworkCore.Tools` (9.0.1)

### AciPlatform.Api
- (Basic ASP.NET Core packages)

Created: 23/01/2026
.NET Version: 9.0
