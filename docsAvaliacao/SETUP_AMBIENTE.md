# 🛠️ Setup do Ambiente - LegacyProcs

**Objetivo:** Configurar ambiente local para executar a aplicação legada  
**Tempo Estimado:** 1-2 horas  
**Dificuldade:** Média

---

## 📋 Pré-requisitos

### Software Obrigatório

| Software | Versão | Download | Observações |
|----------|--------|----------|-------------|
| **.NET SDK** | 8.0+ | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) | Para backend modernizado |
| **.NET Framework** | 4.8 | [Download](https://dotnet.microsoft.com/download/dotnet-framework/net48) | Para versão legada (opcional) |
| **Node.js** | 18+ LTS | [Download](https://nodejs.org/) | Para frontend Angular |
| **SQL Server Express** | 2019+ | [Download](https://www.microsoft.com/sql-server/sql-server-downloads) | Banco de dados |
| **Visual Studio 2022** | Community+ | [Download](https://visualstudio.microsoft.com/) | IDE backend (opcional) |
| **VS Code** | Latest | [Download](https://code.visualstudio.com/) | IDE frontend |
| **Git** | Latest | [Download](https://git-scm.com/) | Controle de versão |

### Software Opcional (Recomendado)

| Software | Uso | Download |
|----------|-----|----------|
| **SQL Server Management Studio (SSMS)** | Gerenciar banco | [Download](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) |
| **Postman** | Testar APIs | [Download](https://www.postman.com/downloads/) |
| **OWASP ZAP** | Análise de segurança | [Download](https://www.zaproxy.org/download/) |
| **Docker Desktop** | Containerização (Fase 5) | [Download](https://www.docker.com/products/docker-desktop) |

---

## 🗄️ Setup do Banco de Dados

### Passo 1: Instalar SQL Server Express

```powershell
# Baixar SQL Server Express 2019
# Instalar com configurações padrão
# Anotar o nome da instância (geralmente: localhost\SQLEXPRESS)
```

### Passo 2: Criar o Banco de Dados

**Opção A: Via SSMS (Recomendado)**

1. Abrir SQL Server Management Studio
2. Conectar em `localhost\SQLEXPRESS`
3. Abrir arquivo `database/create-database.sql`
4. Executar script (F5)

**Opção B: Via sqlcmd**

```powershell
# Navegar até a pasta database
cd database

# Executar script
sqlcmd -S localhost\SQLEXPRESS -i create-database.sql
```

### Passo 3: Verificar Criação

```sql
-- No SSMS, executar:
USE LegacyProcs;
GO

SELECT * FROM OrdemServico;
SELECT * FROM Tecnico;
SELECT * FROM Cliente;
```

**Resultado Esperado:** Tabelas criadas com dados de exemplo

---

## 🔧 Setup do Backend

### Passo 1: Clonar Repositório

```bash
git clone https://github.com/alest-github/TesteTimeLegado.git
cd TesteTimeLegado
```

### Passo 2: Criar Sua Branch

```bash
# Substitua "seu-nome" pelo seu nome (sem espaços)
git checkout -b seu-nome/modernizacao
```

### Passo 3: Configurar Connection String

**Arquivo:** `backend/LegacyProcs/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

**Ajustar se necessário:**
- Se sua instância tem nome diferente, altere `localhost\\SQLEXPRESS`
- Se usar autenticação SQL, use: `Server=localhost\\SQLEXPRESS;Database=LegacyProcs;User Id=sa;Password=SuaSenha;TrustServerCertificate=true;`

### Passo 4: Restaurar Pacotes

```bash
cd backend
dotnet restore
```

### Passo 5: Compilar

```bash
dotnet build LegacyProcs/LegacyProcs.csproj
```

**Resultado Esperado:**
```
Compilação com êxito.
    0 Aviso(s)
    0 Erro(s)
```

### Passo 6: Executar Backend

**Opção A: Via .NET CLI**

```bash
dotnet run --project LegacyProcs/LegacyProcs.csproj
```

**Opção B: Via Visual Studio**

1. Abrir `backend/LegacyProcs.sln`
2. Pressionar F5

**Opção C: Via PowerShell Script**

```powershell
cd backend
.\start-backend.ps1
```

### Passo 7: Verificar Backend

Abrir navegador em: `http://localhost:5000/swagger`

**Resultado Esperado:** Swagger UI com endpoints documentados

**Testar Endpoint:**
```bash
curl http://localhost:5000/api/ordemservico
```

---

## 🎨 Setup do Frontend

### Passo 1: Instalar Dependências

```bash
cd frontend
npm install
```

**Tempo Estimado:** 2-5 minutos

### Passo 2: Verificar Instalação

```bash
npm list @angular/core
```

**Resultado Esperado:** `@angular/core@18.2.14`

### Passo 3: Configurar API URL (Opcional)

**Arquivo:** `frontend/src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

### Passo 4: Executar Frontend

```bash
ng serve
```

**Ou via npm:**

```bash
npm start
```

**Resultado Esperado:**
```
✔ Browser application bundle generation complete.
✔ Compiled successfully.
** Angular Live Development Server is listening on localhost:4200 **
```

### Passo 5: Verificar Frontend

Abrir navegador em: `http://localhost:4200`

**Resultado Esperado:** Interface do LegacyProcs

---

## ✅ Testes Funcionais

### Teste 1: Listar Ordens de Serviço (US02)

1. Abrir `http://localhost:4200`
2. Verificar lista de OS exibida
3. Verificar colunas: ID, Título, Técnico, Status, Data

**Resultado Esperado:** Lista com 3-5 OS de exemplo

---

### Teste 2: Criar Ordem de Serviço (US01)

1. Clicar em "Nova Ordem de Serviço"
2. Preencher:
   - Título: "Teste de Criação"
   - Descrição: "Ordem de teste"
   - Técnico: "João Silva"
3. Clicar em "Salvar"

**Resultado Esperado:** 
- Mensagem "Ordem de serviço criada com sucesso!"
- Nova OS aparece na lista

---

### Teste 3: Alterar Status (US03)

1. Na lista, clicar no botão de status de uma OS
2. Selecionar "Em Andamento"
3. Confirmar

**Resultado Esperado:**
- Status atualizado
- Cor do badge alterada

---

### Teste 4: Excluir Ordem de Serviço (US04)

1. Na lista, clicar no botão "Excluir" de uma OS
2. Confirmar exclusão

**Resultado Esperado:**
- Mensagem "Ordem de serviço excluída com sucesso!"
- OS removida da lista

---

## 🐛 Troubleshooting

### Problema 1: Backend não inicia - Erro de conexão com banco

**Erro:**
```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error...
```

**Solução:**
1. Verificar se SQL Server está rodando:
   ```powershell
   Get-Service MSSQL*
   ```
2. Verificar connection string em `appsettings.json`
3. Testar conexão via SSMS

---

### Problema 2: Frontend não compila - Erro de dependências

**Erro:**
```
npm ERR! peer dep missing: @angular/core@^18.0.0
```

**Solução:**
```bash
# Limpar cache e reinstalar
rm -rf node_modules package-lock.json
npm install
```

---

### Problema 3: CORS Error no Frontend

**Erro no Console:**
```
Access to XMLHttpRequest at 'http://localhost:5000/api/ordemservico' 
from origin 'http://localhost:4200' has been blocked by CORS policy
```

**Solução:**
Verificar `Program.cs` no backend:
```csharp
app.UseCors("AllowFrontend");
```

---

### Problema 4: Porta 5000 ou 4200 já em uso

**Erro:**
```
EADDRINUSE: address already in use :::4200
```

**Solução:**

**Windows:**
```powershell
# Encontrar processo usando a porta
netstat -ano | findstr :4200

# Matar processo (substitua PID)
taskkill /PID <PID> /F
```

**Ou usar porta diferente:**
```bash
ng serve --port 4201
```

---

### Problema 5: SQL Server não instalado

**Erro:**
```
Could not find SQL Server instance
```

**Solução:**
1. Baixar SQL Server Express 2019
2. Instalar com configurações padrão
3. Anotar nome da instância
4. Atualizar connection string

---

## 📊 Verificação Final

Antes de iniciar a Fase 1 (Análise), confirme:

- [ ] Backend rodando em `http://localhost:5000`
- [ ] Swagger acessível em `http://localhost:5000/swagger`
- [ ] Frontend rodando em `http://localhost:4200`
- [ ] Banco de dados criado com dados de exemplo
- [ ] Consegue criar uma OS
- [ ] Consegue listar OS
- [ ] Consegue alterar status
- [ ] Consegue excluir OS
- [ ] Git configurado e branch criada

---

## 🔧 Comandos Úteis

### Backend

```bash
# Compilar
dotnet build

# Executar
dotnet run --project LegacyProcs/LegacyProcs.csproj

# Executar testes
dotnet test

# Verificar cobertura
dotnet test /p:CollectCoverage=true

# Limpar build
dotnet clean
```

### Frontend

```bash
# Instalar dependências
npm install

# Executar dev server
ng serve

# Compilar para produção
ng build --configuration production

# Executar testes
npm test

# Verificar cobertura
npm run test:coverage

# Lint
ng lint
```

### Banco de Dados

```sql
-- Ver todas as OS
SELECT * FROM OrdemServico ORDER BY DataCriacao DESC;

-- Ver técnicos ativos
SELECT * FROM Tecnico WHERE Status = 'Ativo';

-- Ver clientes
SELECT * FROM Cliente;

-- Limpar dados de teste
DELETE FROM OrdemServico WHERE Titulo LIKE 'Teste%';
```

---

## 📚 Próximos Passos

Após configurar o ambiente:

1. ✅ Leia o **PRD.md** (requisitos do produto)
2. ✅ Execute todos os testes funcionais (US01-US04)
3. ✅ Familiarize-se com o código legado
4. ✅ Inicie a **Fase 1: Análise e Inventário**
5. ✅ Leia **INSTRUCOES_ESTAGIARIOS.md** para próximos passos

---

## 🆘 Suporte

**Problemas não resolvidos?**

1. Consulte a documentação oficial:
   - [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
   - [Angular Docs](https://angular.dev)
   - [SQL Server Docs](https://docs.microsoft.com/sql/sql-server/)

2. Abra uma issue no repositório:
   - Descreva o problema
   - Inclua mensagens de erro
   - Informe seu ambiente (OS, versões)

3. Fale com o tech lead nas reuniões semanais

---

**Última atualização:** 2025-10-15  
**Versão:** 1.0  
**Autor:** Tech Lead Alest
