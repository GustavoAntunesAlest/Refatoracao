# 🚀 Deploy Rápido - LegacyProcs

**Tempo estimado:** 10 minutos  
**Requisitos:** Docker e Docker Compose instalados

---

## ⚡ Deploy em 3 Comandos

```bash
# 1. Clonar repositório
git clone https://github.com/alest-github/TesteTimeLegado.git
cd TesteTimeLegado
git checkout Gustavo-Antunes/Modernizacao

# 2. Configurar variáveis
cp .env.example .env
# Editar .env e alterar SA_PASSWORD

# 3. Subir aplicação
docker-compose up -d
```

**Pronto!** Acesse:
- Frontend: http://localhost
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

---

## 📋 Pré-requisitos

### Windows
```powershell
# Instalar Docker Desktop
winget install Docker.DockerDesktop

# Verificar instalação
docker --version
docker-compose --version
```

### Linux
```bash
# Instalar Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Instalar Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Verificar instalação
docker --version
docker-compose --version
```

---

## 🐳 Comandos Docker Úteis

### Iniciar Aplicação
```bash
docker-compose up -d
```

### Ver Logs
```bash
# Todos os serviços
docker-compose logs -f

# Apenas backend
docker-compose logs -f backend

# Apenas frontend
docker-compose logs -f frontend

# Apenas database
docker-compose logs -f database
```

### Parar Aplicação
```bash
docker-compose down
```

### Parar e Remover Volumes (⚠️ Apaga dados!)
```bash
docker-compose down -v
```

### Rebuild (após mudanças no código)
```bash
docker-compose up -d --build
```

### Ver Status
```bash
docker-compose ps
```

### Acessar Container
```bash
# Backend
docker exec -it legacyprocs-backend bash

# Frontend
docker exec -it legacyprocs-frontend sh

# Database
docker exec -it legacyprocs-database bash
```

---

## 🔧 Configuração

### Arquivo .env

```env
# Alterar senha do banco (OBRIGATÓRIO)
SA_PASSWORD=SuaSenhaForte@123

# Outras configurações (opcional)
ASPNETCORE_ENVIRONMENT=Production
LOG_LEVEL=Information
```

### Portas Utilizadas

| Serviço | Porta | URL |
|---------|-------|-----|
| Frontend | 80 | http://localhost |
| Backend | 5000 | http://localhost:5000 |
| Database | 1433 | localhost:1433 |
| Swagger | 5000 | http://localhost:5000/swagger |

---

## ✅ Verificação Pós-Deploy

### 1. Health Checks
```bash
# Backend
curl http://localhost:5000/health
# Resposta esperada: {"status":"healthy"}

# Frontend
curl http://localhost
# Resposta esperada: HTML da aplicação
```

### 2. Testar API
```bash
# Listar Ordens de Serviço
curl http://localhost:5000/api/ordemservico

# Ver documentação
# Acessar: http://localhost:5000/swagger
```

### 3. Testar Frontend
```
# Abrir navegador
http://localhost

# Verificar:
✅ Página carrega
✅ Pode criar Ordem de Serviço
✅ MatSnackBar aparece (não alert!)
✅ Lista atualiza sem reload
✅ Validação de CNPJ funciona
```

---

## 🗄️ Database

### Acessar SQL Server

```bash
# Via Docker
docker exec -it legacyprocs-database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SuaSenha"

# Via SQL Server Management Studio (SSMS)
Server: localhost,1433
Login: sa
Password: (sua senha do .env)
```

### Executar Scripts

```bash
# Copiar script para container
docker cp database/schema.sql legacyprocs-database:/tmp/

# Executar
docker exec -it legacyprocs-database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SuaSenha" -i /tmp/schema.sql
```

### Backup

```bash
# Criar backup
docker exec legacyprocs-database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SuaSenha" -Q "BACKUP DATABASE LegacyProcs TO DISK='/var/opt/mssql/backup/legacyprocs.bak'"

# Copiar backup para host
docker cp legacyprocs-database:/var/opt/mssql/backup/legacyprocs.bak ./backup/
```

---

## 🔒 Segurança

### ⚠️ IMPORTANTE

```bash
# NÃO commitar:
❌ .env (com senhas reais)
❌ appsettings.Production.json (com connection strings)
❌ Backups de banco de dados

# Sempre usar:
✅ .env.example (sem senhas)
✅ Variáveis de ambiente
✅ Secrets management (produção)
```

### Alterar Senha do Banco

```bash
# 1. Parar containers
docker-compose down

# 2. Remover volume do banco (⚠️ apaga dados!)
docker volume rm testetimelegado_sqldata

# 3. Alterar SA_PASSWORD no .env

# 4. Subir novamente
docker-compose up -d
```

---

## 🐛 Troubleshooting

### Porta já em uso

```bash
# Erro: "port is already allocated"

# Solução 1: Parar processo na porta
# Windows
netstat -ano | findstr :80
taskkill /PID <PID> /F

# Linux
sudo lsof -i :80
sudo kill -9 <PID>

# Solução 2: Alterar porta no docker-compose.yml
ports:
  - "8080:80"  # Usar 8080 ao invés de 80
```

### Container não inicia

```bash
# Ver logs
docker-compose logs backend

# Problemas comuns:
1. Senha do banco inválida → Verificar .env
2. Porta em uso → Mudar porta
3. Falta de memória → Aumentar recursos do Docker
```

### Database não conecta

```bash
# Verificar se database está rodando
docker-compose ps database

# Ver logs do database
docker-compose logs database

# Testar conexão
docker exec legacyprocs-database /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SuaSenha" -Q "SELECT 1"
```

### Frontend não carrega

```bash
# Verificar se backend está respondendo
curl http://localhost:5000/health

# Ver logs do frontend
docker-compose logs frontend

# Rebuild do frontend
docker-compose up -d --build frontend
```

---

## 📊 Monitoramento

### Ver Recursos

```bash
# Uso de CPU/Memória
docker stats

# Apenas legacyprocs
docker stats legacyprocs-backend legacyprocs-frontend legacyprocs-database
```

### Logs em Tempo Real

```bash
# Todos os serviços
docker-compose logs -f --tail=100

# Filtrar por nível
docker-compose logs -f | grep ERROR
docker-compose logs -f | grep WARNING
```

---

## 🔄 Atualização

### Atualizar Código

```bash
# 1. Baixar última versão
git pull origin Gustavo-Antunes/Modernizacao

# 2. Rebuild containers
docker-compose up -d --build

# 3. Verificar
curl http://localhost:5000/health
```

### Rollback

```bash
# 1. Voltar para commit anterior
git checkout <commit-anterior>

# 2. Rebuild
docker-compose up -d --build
```

---

## 📈 Performance

### Otimizações

```yaml
# docker-compose.yml
services:
  backend:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 512M
        reservations:
          cpus: '1'
          memory: 256M
```

### Cache

```bash
# Limpar cache do Docker
docker system prune -a

# Limpar apenas images não usadas
docker image prune -a
```

---

## 🎯 Produção

### Checklist

- [ ] Alterar SA_PASSWORD no .env
- [ ] Configurar HTTPS/SSL
- [ ] Configurar domínio
- [ ] Configurar backup automático
- [ ] Configurar monitoramento
- [ ] Configurar logs centralizados
- [ ] Testar rollback
- [ ] Documentar procedimentos

### HTTPS

```yaml
# docker-compose.yml
services:
  frontend:
    ports:
      - "443:443"
    volumes:
      - ./ssl:/etc/nginx/ssl
```

```nginx
# nginx.conf
server {
    listen 443 ssl;
    ssl_certificate /etc/nginx/ssl/cert.pem;
    ssl_certificate_key /etc/nginx/ssl/key.pem;
    # ...
}
```

---

## 📞 Suporte

**Documentação Completa:** `docs/PLANO_DEPLOY.md`  
**Problemas:** Abrir issue no GitHub  
**Desenvolvedor:** Gustavo Antunes

---

## 🎉 Pronto!

Sua aplicação está rodando em:
- **Frontend:** http://localhost
- **Backend:** http://localhost:5000
- **Swagger:** http://localhost:5000/swagger

**Próximos passos:**
1. Testar todas as funcionalidades
2. Configurar backup
3. Configurar monitoramento
4. Deploy em produção

🚀 **BOA SORTE COM O DEPLOY!**
