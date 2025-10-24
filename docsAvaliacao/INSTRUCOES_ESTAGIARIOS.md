---
title: "Instruções para Estagiários - Modernização LegacyProcs"
project: "LegacyProcs"
version: "1.0"
date: "2025-10-10"
---

# Instruções para Estagiários - Projeto de Modernização LegacyProcs

## 🎯 Objetivo

Modernizar uma aplicação legada com problemas técnicos **intencionais**, aplicando as melhores práticas da Alest.

**Nota mínima para aprovação:** 70%  
**Duração:** 10 semanas  
**Carga horária:** 160-200 horas

## ⚠️ VERSIONAMENTO OBRIGATÓRIO

**ATENÇÃO:** O uso de Git é **OBRIGATÓRIO** durante todo o projeto!

### Repositório Base
```
https://github.com/alest-github/TesteTimeLegado.git
```

### Fluxo de Trabalho (Obrigatório)

**1. Clone o repositório:**
```bash
git clone https://github.com/alest-github/TesteTimeLegado.git
cd TesteTimeLegado
```

**2. Crie sua branch de trabalho:**
```bash
# Nomenclatura: seu-nome/modernizacao
git checkout -b joao-silva/modernizacao
```

**3. Commits frequentes (diários recomendado):**
```bash
git add .
git commit -m "feat(backend): implementa camada Domain com entidade OrdemServico"
git push origin joao-silva/modernizacao
```

### Convenção de Commits (Conventional Commits)

Use o formato:
```
<tipo>(<escopo>): <descrição>

Exemplos:
feat(backend): adiciona Repository Pattern
fix(frontend): corrige SQL Injection no filtro
docs(adr): cria ADR-001 sobre Clean Architecture
test(backend): adiciona testes unitários para OrdemServicoService
refactor(frontend): extrai lógica HTTP para service
```

**Tipos permitidos:**
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `test`: Testes
- `refactor`: Refatoração
- `chore`: Tarefas gerais (configuração, etc.)

### Entregáveis no Git

**✅ DEVE commitar:**
- Código fonte (backend/frontend)
- Testes
- Documentação (ADRs, README)
- Configurações (Docker, CI/CD)
- Scripts de banco (migrations)

**❌ NÃO commitar:**
- `node_modules/`
- `bin/`, `obj/`
- Arquivos de banco (`.mdf`, `.ldf`)
- Senhas ou credenciais
- `.vs/`, `.idea/`

### Gates de Qualidade

Antes de cada reunião semanal, você DEVE ter:
- ✅ Commits da semana pushados
- ✅ Mensagens de commit descritivas
- ✅ Branch atualizada e funcional

**Falta de versionamento = penalização na nota!**

---

## 📋 Processo de Modernização

```
Fase 1 (Semanas 1-2): Análise e Inventário
   ├── Executar aplicação legada
   ├── Inventariar débitos técnicos (8 identificados = obrigatório)
   ├── Documentar vulnerabilidades (OWASP ZAP)
   ├── Criar 5 ADRs obrigatórios
   └── [GIT] Commits diários da análise

Fase 2 (Semanas 2-3): Planejamento
   ├── Definir arquitetura Clean Architecture
   ├── Estimar esforço (160-200h)
   ├── Criar roadmap detalhado
   ├── Aprovar com tech lead
   └── [GIT] Commits com ADRs e planejamento

Fase 3 (Semanas 3-6): Backend .NET 8
   ├── Setup .NET 8 + EF Core
   ├── Implementar Clean Architecture (Domain, Application, Infrastructure, API)
   ├── Migrar lógica de negócio
   ├── Corrigir SQL Injection (CRÍTICO)
   ├── Testes >80% cobertura
   └── [GIT] Commits frequentes (mínimo 3x/semana)

Fase 4 (Semanas 7-8): Frontend Angular 18+
   ├── Upgrade Angular 12→18
   ├── Implementar Angular Material/PrimeNG
   ├── UI responsiva
   ├── State management estruturado
   └── [GIT] Commits frequentes

Fase 5 (Semanas 9-10): DevOps
   ├── Docker + Docker Compose
   ├── CI/CD (GitHub Actions ou Azure DevOps)
   ├── Testes E2E
   ├── Documentação final
   └── [GIT] Merge request final
```

## 🚀 Fase 1: Análise Inicial (Semanas 1-2)

### 1.1 Leituras Obrigatórias

- [ ] `prd.md` - Requisitos do produto
- [ ] `docsAvaliacao/VERSOES.md` - Comparativo versões
- [ ] `docsAvaliacao/ANALISE_TECNICA.md` - Débitos técnicos
- [ ] `docsAvaliacao/SETUP_AMBIENTE.md` - Setup do ambiente
- [ ] Global Rules (seção 11: Modernização de Legacy)

### 1.2 Inventário de Débitos Técnicos

Crie `docs/INVENTARIO_DEBITOS.md`:

```markdown
# Inventário de Débitos Técnicos - [Seu Nome]

## Backend (4 críticos + 2 altos)
1. [CRÍTICO] SQL Injection em OrdemServicoController.Get()
   - Impacto: Perda de dados
   - Esforço: 2h
   - Solução: EF Core parametrizado

2. [ALTO] Lógica de negócio no controller
   - Impacto: Não testável
   - Esforço: 8h
   - Solução: Clean Architecture

... (continue com TODOS os 8 débitos)
```

**Ferramentas:**
```bash
# Análise estática backend
dotnet tool install -g security-scan
security-scan ./LegacyProcs.sln

# Análise frontend
npm install -g eslint
eslint ./frontend/src

# Vulnerabilidades (OWASP ZAP)
zap-cli quick-scan http://localhost:5000
```

### 1.3 ADRs Obrigatórios

Crie 5 ADRs seguindo o template da seção 33 das Global Rules:

1. `docs/adr/001-migracao-dotnet8.md`
2. `docs/adr/002-escolha-ef-core.md`
3. `docs/adr/003-arquitetura-clean.md`
4. `docs/adr/004-frontend-angular18.md`
5. `docs/adr/005-containerizacao-docker.md`

**Template:**
```markdown
# ADR XXX: Título

## Status
Proposto | Aprovado | Rejeitado

## Contexto
[Descreva o problema]

## Decisão
[Sua decisão]

## Consequências
**Positivas:**
- ...

**Negativas:**
- ...

## Alternativas
- [Opção 1]: Descartado porque...
```

### 1.4 Entregáveis Fase 1

- [ ] Aplicação legada executando localmente
- [ ] `INVENTARIO_DEBITOS.md` (8 débitos)
- [ ] Relatório OWASP ZAP
- [ ] 5 ADRs documentados
- [ ] Apresentação 15min para tech lead

**Gate de Qualidade:**
- ✅ TODOS os 8 débitos identificados
- ✅ SQL Injection com prova de conceito
- ✅ ADRs no formato padrão

---

## 🏗️ Fase 2: Planejamento (Semanas 2-3)

### 2.1 Arquitetura Alvo

**Estrutura Clean Architecture:**
```
backend/
├── src/
│   ├── LegacyProcs.Api/
│   ├── LegacyProcs.Application/
│   ├── LegacyProcs.Domain/
│   └── LegacyProcs.Infrastructure/
├── tests/
│   ├── LegacyProcs.UnitTests/
│   ├── LegacyProcs.IntegrationTests/
│   └── LegacyProcs.E2ETests/
├── Dockerfile
└── docker-compose.yml
```

### 2.2 Roadmap Detalhado

Crie `docs/ROADMAP_DETALHADO.md`:

```markdown
## Sprint 1 (Semanas 3-4): Backend Foundation
- Setup .NET 8 (8h)
- EF Core + Migrations (4h)
- Domain Layer (8h)
- Infrastructure Layer (8h)
- Testes (4h)
**Total:** 32h

## Sprint 2 (Semanas 4-5): Application Layer
- MediatR setup (2h)
- Commands (12h)
- Queries (8h)
- Validators (4h)
- Testes (6h)
**Total:** 32h

... (continue)
```

### 2.3 Entregáveis Fase 2

- [ ] Diagrama arquitetura alvo
- [ ] `ROADMAP_DETALHADO.md`
- [ ] Estimativa 160-200h
- [ ] Aprovação tech lead

**Gate de Qualidade:**
- ✅ Arquitetura Clean
- ✅ Estimativas realistas
- ✅ Sprints bem definidos

---

## 💻 Fase 3: Backend .NET 8 (Semanas 3-6)

### 3.1 Setup Projeto

```bash
dotnet new sln -n LegacyProcs
dotnet new webapi -n LegacyProcs.Api -o src/LegacyProcs.Api
dotnet new classlib -n LegacyProcs.Application -o src/LegacyProcs.Application
dotnet new classlib -n LegacyProcs.Domain -o src/LegacyProcs.Domain
dotnet new classlib -n LegacyProcs.Infrastructure -o src/LegacyProcs.Infrastructure
dotnet new xunit -n LegacyProcs.UnitTests -o tests/LegacyProcs.UnitTests

dotnet sln add src/**/*.csproj tests/**/*.csproj
```

### 3.2 Pacotes Essenciais

```bash
# Application
dotnet add src/LegacyProcs.Application package MediatR
dotnet add src/LegacyProcs.Application package FluentValidation

# Infrastructure
dotnet add src/LegacyProcs.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/LegacyProcs.Infrastructure package Microsoft.EntityFrameworkCore.Tools

# API
dotnet add src/LegacyProcs.Api package Swashbuckle.AspNetCore
dotnet add src/LegacyProcs.Api package Serilog.AspNetCore

# Testes
dotnet add tests/LegacyProcs.UnitTests package Moq
dotnet add tests/LegacyProcs.UnitTests package FluentAssertions
```

### 3.3 Checklist Backend

- [ ] **Domain Layer**
  - [ ] Entidade `OrdemServico` com validações
  - [ ] Enum `StatusOS`
  - [ ] Testes unitários >90%

- [ ] **Infrastructure Layer**
  - [ ] `AppDbContext`
  - [ ] `OrdemServicoConfiguration`
  - [ ] `OrdemServicoRepository` com paginação
  - [ ] Migrations criadas e testadas

- [ ] **Application Layer**
  - [ ] Commands: Criar, AlterarStatus, Excluir
  - [ ] Queries: Listar (paginado), ObterPorId
  - [ ] Validators (FluentValidation)
  - [ ] Testes unitários >80%

- [ ] **API Layer**
  - [ ] Controllers REST
  - [ ] Swagger configurado
  - [ ] Tratamento de erros global
  - [ ] Logs estruturados (Serilog)
  - [ ] CORS configurado

### 3.4 Entregáveis Fase 3

- [ ] Backend .NET 8 funcional
- [ ] Cobertura testes >80%
- [ ] SQL Injection corrigido (comprovado OWASP ZAP)
- [ ] Migrations funcionando
- [ ] Swagger acessível
- [ ] Demo para tech lead

**Gate de Qualidade:**
- ✅ `dotnet test --verbosity normal` (100% passing)
- ✅ `dotnet test /p:CollectCoverage=true` (>80%)
- ✅ OWASP ZAP sem críticos
- ✅ Clean Architecture respeitada

---

## 🎨 Fase 4: Frontend Angular 18+ (Semanas 7-8)

### 4.1 Upgrade Angular

```bash
cd frontend
git checkout -b feature/angular-upgrade

# Upgrade incremental
ng update @angular/core@13 @angular/cli@13
ng update @angular/core@14 @angular/cli@14
# ... até 18
ng update @angular/core@18 @angular/cli@18

# Material
ng add @angular/material
```

### 4.2 Estrutura Moderna

```
frontend/src/app/
├── core/
│   ├── services/
│   ├── interceptors/
│   └── models/
├── features/
│   └── ordem-servico/
│       ├── components/
│       └── ordem-servico.routes.ts
└── shared/
    └── components/
```

### 4.3 Checklist Frontend

- [ ] Upgrade Angular 12→18
- [ ] Angular Material ou PrimeNG
- [ ] Services tipados
- [ ] Signals (Angular 18)
- [ ] UI responsiva (mobile-first)
- [ ] State management estruturado
- [ ] Testes unitários >70%
- [ ] Testes E2E (Playwright)

### 4.4 Entregáveis Fase 4

- [ ] Frontend Angular 18+ funcional
- [ ] UI moderna e responsiva
- [ ] Integração com backend
- [ ] Testes >70%
- [ ] Demo para tech lead

---

## 🐳 Fase 5: DevOps (Semanas 9-10)

### 5.1 Docker

**Dockerfile Backend:**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet test
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "LegacyProcs.Api.dll"]
```

**docker-compose.yml:**
```yaml
version: '3.8'
services:
  backend:
    build: ./backend
    ports: ["5000:80"]
    depends_on: [sqlserver]
  frontend:
    build: ./frontend
    ports: ["4200:80"]
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrong@Passw0rd"
    ports: ["1433:1433"]
```

### 5.2 CI/CD (GitHub Actions)

```yaml
# .github/workflows/ci-cd.yml
name: CI/CD
on:
  push:
    branches: [main]
jobs:
  backend-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
      - name: Run Tests
        run: dotnet test
      
  build:
    needs: backend-tests
    runs-on: ubuntu-latest
    steps:
      - name: Build Docker
        run: docker-compose build
```

### 5.3 Checklist DevOps

- [ ] Dockerfile backend multi-stage
- [ ] Dockerfile frontend otimizado
- [ ] docker-compose.yml funcional
- [ ] CI/CD automatizado
- [ ] Quality gates (testes, cobertura)
- [ ] Documentação completa (README.md atualizado)

### 5.4 Entregáveis Fase 5

- [ ] Aplicação containerizada
- [ ] `docker-compose up` funciona
- [ ] Pipeline CI/CD automatizado
- [ ] Documentação finalizada
- [ ] Apresentação final (30min)

**Gate de Qualidade:**
- ✅ `docker-compose up` (aplicação sobe)
- ✅ Pipeline verde (todos os testes passando)
- ✅ Cobertura >80% backend, >70% frontend
- ✅ Documentação completa

---

## 📊 Critérios de Avaliação

| Categoria | Peso | Requisitos Mínimos |
|-----------|------|-------------------|
| Análise técnica | 20% | Identificar 8 débitos, relatório OWASP ZAP |
| Planejamento | 15% | 5 ADRs, roadmap realista |
| Backend | 25% | .NET 8, EF Core, Clean Architecture, testes >80% |
| Frontend | 20% | Angular 18+, UI moderna, responsivo |
| DevOps | 10% | Docker, CI/CD funcional |
| Qualidade | 10% | Cobertura testes, segurança, documentação |

**Nota mínima:** 70%

## 📚 Recursos

- Global Rules (seções 11, 13, 35)
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Angular 18 Docs](https://angular.dev)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)

## ⚠️ Regras Importantes

1. ✅ Seguir TODAS as Global Rules (especialmente seção 11)
2. ✅ Commits Conventional Commits
3. ✅ Pull Requests com revisão de código
4. ✅ Nunca fazer push direto na main
5. ✅ Testes ANTES de implementar (TDD quando possível)
6. ✅ Documentar TODAS as decisões técnicas
7. ✅ Reuniões semanais com tech lead

## 🆘 Suporte

- **Dúvidas técnicas:** Abra issue no repositório
- **Reuniões:** Toda segunda-feira 10h
- **Code review:** Pull requests obrigatórios
- **Mentoria:** Tech lead disponível segunda a sexta

Boa sorte! 🚀
