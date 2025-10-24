# 🗓️ Roadmap de Modernização - LegacyProcs

**Duração Total:** 10 semanas (160-200 horas)  
**Objetivo:** Modernizar aplicação legada para .NET 8 + Angular 18  
**Nota Mínima:** 70/100 pontos

---

## 📊 Visão Geral

```
Semanas 1-2:  ████████░░░░░░░░░░░░  Análise e Inventário (16h)
Semanas 2-3:  ░░░░░░░░████████░░░░  Planejamento e ADRs (16h)
Semanas 3-6:  ░░░░░░░░░░░░████████  Backend .NET 8 (96h)
Semanas 7-8:  ░░░░░░░░░░░░░░░░████  Frontend Angular 18+ (32h)
Semanas 9-10: ░░░░░░░░░░░░░░░░░░██  DevOps (28h)
────────────────────────────────────────────────────
TOTAL: 188h (média entre 160-200h)
```

---

## 📅 Timeline Detalhada

### Fase 1: Análise e Inventário (Semanas 1-2) - 16h

**Objetivo:** Entender o sistema legado e identificar débitos técnicos

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Setup ambiente local | 2h | Aplicação rodando |
| Executar e testar aplicação | 2h | Testes funcionais US01-US04 |
| Análise de código (backend) | 3h | Lista de débitos backend |
| Análise de código (frontend) | 2h | Lista de débitos frontend |
| Análise de segurança (OWASP ZAP) | 2h | Relatório de vulnerabilidades |
| Documentar inventário | 3h | `docs/INVENTARIO_DEBITOS.md` |
| Commits e revisão | 2h | Branch criada, commits diários |

**Entregáveis:**
- ✅ `docs/INVENTARIO_DEBITOS.md` com 8 débitos identificados
- ✅ Relatório OWASP ZAP
- ✅ 5-7 commits (análise)

**Gate de Qualidade:**
- [ ] Todos os 8 débitos identificados
- [ ] OWASP ZAP executado
- [ ] Aplicação legada funcionando
- [ ] Commits diários

---

### Fase 2: Planejamento e ADRs (Semanas 2-3) - 16h

**Objetivo:** Planejar arquitetura e decisões técnicas

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Estudar Clean Architecture | 2h | Entendimento do padrão |
| Criar ADR-001 (Migração .NET 8) | 1h | `docs/adr/ADR-001.md` |
| Criar ADR-002 (Clean Architecture) | 1h | `docs/adr/ADR-002.md` |
| Criar ADR-003 (CQRS + MediatR) | 1h | `docs/adr/ADR-003.md` |
| Criar ADR-004 (Entity Framework Core) | 1h | `docs/adr/ADR-004.md` |
| Criar ADR-005 (Angular 18 + Material) | 1h | `docs/adr/ADR-005.md` |
| Desenhar arquitetura alvo | 3h | Diagrama de camadas |
| Estimar esforço detalhado | 2h | Planilha de estimativas |
| Criar roadmap detalhado | 2h | Este documento |
| Revisão com tech lead | 2h | Aprovação do plano |

**Entregáveis:**
- ✅ 5 ADRs documentados
- ✅ Diagrama de arquitetura
- ✅ Roadmap detalhado
- ✅ Estimativas de esforço
- ✅ 5-7 commits (planejamento)

**Gate de Qualidade:**
- [ ] 5 ADRs criados e aprovados
- [ ] Arquitetura Clean definida
- [ ] Roadmap aprovado pelo tech lead

---

### Fase 3: Backend .NET 8 (Semanas 3-6) - 96h

#### Sprint 1: Foundation (Semanas 3-4) - 32h

**Objetivo:** Migrar para .NET 8 e criar base da Clean Architecture

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Setup projeto .NET 8 | 2h | `LegacyProcs.csproj` (net8.0) |
| Configurar DI e Serilog | 2h | `Program.cs` |
| Criar entidade OrdemServico | 2h | `Models/OrdemServico.cs` |
| Criar DbContext (EF Core) | 3h | `Data/LegacyProcsDbContext.cs` |
| Criar Repository Pattern | 4h | `IOrdemServicoRepository` |
| Implementar Repository | 4h | `OrdemServicoRepository.cs` |
| Migrar controller básico | 3h | `OrdemServicoController.cs` |
| Configurar Swagger | 1h | Documentação API |
| Testes unitários (Models) | 4h | `Models.Tests` |
| Testes unitários (Repository) | 4h | `Repository.Tests` |
| Commits e code review | 3h | 10-12 commits |

**Entregáveis:**
- ✅ Projeto .NET 8 funcional
- ✅ EF Core configurado
- ✅ Repository Pattern implementado
- ✅ 15+ testes unitários
- ✅ Swagger funcionando

**Gate de Qualidade:**
- [ ] Compila sem erros
- [ ] Testes passando (>80%)
- [ ] Swagger acessível

---

#### Sprint 2: Application Layer (Semanas 4-5) - 32h

**Objetivo:** Implementar CQRS com MediatR

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Instalar MediatR | 1h | Package instalado |
| Criar Commands (Create, Update, Delete) | 4h | `Application/Commands/` |
| Criar Queries (GetAll, GetById) | 3h | `Application/Queries/` |
| Criar Handlers para Commands | 6h | `Application/Handlers/` |
| Criar Handlers para Queries | 4h | `Application/Handlers/` |
| Refatorar Controllers (usar MediatR) | 4h | Controllers simplificados |
| Adicionar FluentValidation | 3h | Validadores |
| Testes unitários (Commands) | 3h | `Commands.Tests` |
| Testes unitários (Queries) | 2h | `Queries.Tests` |
| Testes unitários (Handlers) | 4h | `Handlers.Tests` |
| Commits e code review | 2h | 8-10 commits |

**Entregáveis:**
- ✅ CQRS implementado
- ✅ MediatR configurado
- ✅ FluentValidation adicionado
- ✅ 20+ testes unitários

**Gate de Qualidade:**
- [ ] CQRS funcionando
- [ ] Cobertura >80%
- [ ] Validações implementadas

---

#### Sprint 3: API Completa + Segurança (Semanas 5-6) - 32h

**Objetivo:** Completar API e corrigir vulnerabilidades

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Adicionar entidades Tecnico e Cliente | 4h | Models completos |
| Criar Repositories (Tecnico, Cliente) | 4h | Repositories |
| Criar Commands/Queries (Tecnico, Cliente) | 6h | Application layer |
| Criar Controllers (Tecnico, Cliente) | 3h | Controllers |
| Implementar paginação | 3h | `GetPagedAsync` |
| Corrigir SQL Injection | 2h | Queries parametrizadas |
| Externalizar credenciais | 1h | `appsettings.json` |
| Adicionar logging estruturado | 2h | Serilog configurado |
| Testes de integração | 4h | Integration tests |
| Aumentar cobertura para >80% | 2h | Testes adicionais |
| Code review final | 1h | Revisão completa |

**Entregáveis:**
- ✅ API completa (3 entidades)
- ✅ Paginação implementada
- ✅ SQL Injection corrigido
- ✅ Cobertura >80%
- ✅ 49+ testes passando

**Gate de Qualidade:**
- [ ] Todas as entidades funcionando
- [ ] 0 vulnerabilidades críticas
- [ ] Cobertura >80%
- [ ] OWASP ZAP sem críticos

---

### Fase 4: Frontend Angular 18+ (Semanas 7-8) - 32h

#### Sprint 4: Migração Angular (Semana 7) - 16h

**Objetivo:** Migrar Angular 12 → 18

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Migrar Angular 12 → 13 | 2h | `package.json` atualizado |
| Migrar Angular 13 → 14 | 2h | Standalone components |
| Migrar Angular 14 → 15 | 2h | Functional guards |
| Migrar Angular 15 → 16 | 2h | Signals disponíveis |
| Migrar Angular 16 → 17 | 2h | Control flow syntax |
| Migrar Angular 17 → 18 | 2h | Versão final |
| Testar aplicação após migração | 2h | Testes funcionais |
| Corrigir breaking changes | 2h | Ajustes necessários |

**Entregáveis:**
- ✅ Angular 18.2.14
- ✅ Aplicação funcionando
- ✅ 6 commits (migração)

---

#### Sprint 5: UI Moderna (Semana 8) - 16h

**Objetivo:** Adicionar Angular Material e melhorar UX

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Instalar Angular Material | 1h | Material instalado |
| Configurar tema | 1h | Tema aplicado |
| Criar Services (OrdemServico) | 2h | `ordem-servico.service.ts` |
| Criar Services (Tecnico, Cliente) | 2h | Services completos |
| Refatorar componentes (usar Services) | 3h | Componentes limpos |
| Substituir alert() por MatSnackBar | 2h | UX melhorada |
| Remover location.reload() | 1h | Atualização sem reload |
| Implementar Signals | 2h | Reatividade |
| Testes unitários (Services) | 2h | 19+ testes |

**Entregáveis:**
- ✅ Angular Material configurado
- ✅ Services implementados
- ✅ UX melhorada
- ✅ 19+ testes unitários

**Gate de Qualidade:**
- [ ] Material funcionando
- [ ] Services usados nos componentes
- [ ] Sem alert() ou location.reload()
- [ ] Testes criados

---

### Fase 5: DevOps (Semanas 9-10) - 28h

#### Sprint 6: Containerização (Semana 9) - 14h

**Objetivo:** Dockerizar aplicação

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Criar Dockerfile backend | 3h | Multi-stage build |
| Criar Dockerfile frontend | 2h | Nginx |
| Criar docker-compose.yml | 3h | Orquestração |
| Testar build local | 2h | `docker-compose up` |
| Otimizar imagens | 2h | Reduzir tamanho |
| Documentar setup Docker | 2h | README atualizado |

**Entregáveis:**
- ✅ Dockerfile backend
- ✅ Dockerfile frontend
- ✅ docker-compose.yml
- ✅ Aplicação rodando em containers

---

#### Sprint 7: CI/CD (Semana 10) - 14h

**Objetivo:** Automatizar deploy

| Atividade | Duração | Entregável |
|-----------|---------|------------|
| Criar workflow GitHub Actions | 4h | `.github/workflows/ci-cd.yml` |
| Configurar build automatizado | 2h | Build on push |
| Configurar testes automatizados | 2h | Tests on PR |
| Configurar quality gates | 2h | Cobertura, linting |
| Configurar deploy automatizado | 2h | Deploy on merge |
| Testar pipeline completo | 2h | Pipeline verde |

**Entregáveis:**
- ✅ CI/CD configurado
- ✅ Quality gates
- ✅ Deploy automatizado
- ✅ Pipeline verde

**Gate de Qualidade:**
- [ ] Pipeline executando
- [ ] Testes automatizados
- [ ] Deploy funcionando

---

## 📊 Marcos (Milestones)

| Milestone | Semana | Critérios de Aceite |
|-----------|--------|---------------------|
| **M1: Análise Aprovada** | 2 | 8 débitos identificados, OWASP ZAP executado |
| **M2: Planejamento Aprovado** | 3 | 5 ADRs criados, arquitetura definida |
| **M3: Backend Foundation** | 4 | .NET 8 + EF Core + Repository Pattern |
| **M4: Backend Application** | 5 | CQRS + MediatR implementado |
| **M5: Backend Completo** | 6 | 3 entidades, >80% cobertura, 0 vulnerabilidades |
| **M6: Frontend Moderno** | 8 | Angular 18 + Material + Services |
| **M7: DevOps Completo** | 9 | Docker + docker-compose funcionando |
| **M8: Apresentação Final** | 10 | Projeto completo, documentado, deployado |

---

## ⚠️ Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| **Não identificar todos os débitos** | Alta | Médio | Usar OWASP ZAP, linters, code review |
| **Prazo insuficiente** | Média | Alto | Priorizar funcionalidades core, pedir extensão |
| **Falta de conhecimento .NET 8** | Alta | Médio | Documentação, reuniões semanais, tutoriais |
| **Breaking changes na migração Angular** | Média | Médio | Migrar incrementalmente (12→13→...→18) |
| **Bugs em produção** | Baixa | Alto | Testes automatizados >80%, code review |
| **Dependências incompatíveis** | Baixa | Médio | Testar em ambiente isolado primeiro |
| **Performance ruim após migração** | Baixa | Médio | Benchmarks, profiling, otimizações |

---

## 📈 Estimativas de Esforço

### Por Fase

| Fase | Horas | % do Total |
|------|-------|------------|
| Fase 1: Análise | 16h | 8.5% |
| Fase 2: Planejamento | 16h | 8.5% |
| Fase 3: Backend | 96h | 51% |
| Fase 4: Frontend | 32h | 17% |
| Fase 5: DevOps | 28h | 15% |
| **TOTAL** | **188h** | **100%** |

### Por Categoria

| Categoria | Horas | % do Total |
|-----------|-------|------------|
| Desenvolvimento | 120h | 64% |
| Testes | 40h | 21% |
| Documentação | 20h | 11% |
| Code Review | 8h | 4% |
| **TOTAL** | **188h** | **100%** |

---

## ✅ Checklist de Entrega Final

Antes da apresentação final (Semana 10):

### Backend
- [ ] .NET 8 funcional
- [ ] Clean Architecture (4 camadas)
- [ ] Entity Framework Core
- [ ] MediatR/CQRS
- [ ] Cobertura >80%
- [ ] SQL Injection corrigido
- [ ] Swagger acessível

### Frontend
- [ ] Angular 18+ funcional
- [ ] Angular Material
- [ ] UI responsiva
- [ ] Signals implementados
- [ ] Testes >70%
- [ ] Testes E2E (opcional)

### DevOps
- [ ] Dockerfile backend
- [ ] Dockerfile frontend
- [ ] docker-compose up funciona
- [ ] Pipeline CI/CD verde
- [ ] Quality gates configurados

### Documentação
- [ ] README.md atualizado
- [ ] 5 ADRs documentados
- [ ] INVENTARIO_DEBITOS.md
- [ ] Swagger documentado

---

## 🎯 Critérios de Avaliação

| Categoria | Peso | Nota Mínima |
|-----------|------|-------------|
| Análise Técnica | 20% | 14/20 |
| Planejamento | 15% | 10/15 |
| Backend .NET 8 | 25% | 18/25 |
| Frontend Angular 18+ | 20% | 14/20 |
| DevOps | 10% | 7/10 |
| Qualidade | 10% | 7/10 |
| **TOTAL** | **100%** | **70/100** |

---

## 📅 Calendário Sugerido

```
Semana 1:  Análise (Fase 1)
Semana 2:  Análise + Planejamento (Fase 1-2)
Semana 3:  Backend Foundation (Sprint 1)
Semana 4:  Backend Foundation (Sprint 1)
Semana 5:  Backend Application (Sprint 2)
Semana 6:  Backend Completo (Sprint 3)
Semana 7:  Frontend Migração (Sprint 4)
Semana 8:  Frontend UI Moderna (Sprint 5)
Semana 9:  DevOps Containerização (Sprint 6)
Semana 10: DevOps CI/CD + Apresentação (Sprint 7)
```

---

## 🆘 Quando Pedir Ajuda

**Peça ajuda se:**
- ❌ Travou por mais de 2 horas no mesmo problema
- ❌ Não entende um conceito fundamental
- ❌ Está atrasado >1 semana
- ❌ Encontrou bug bloqueante

**Como pedir ajuda:**
1. Descreva o problema claramente
2. Mostre o que já tentou
3. Inclua mensagens de erro
4. Prepare perguntas específicas

---

## 📚 Recursos Úteis

### Documentação Oficial
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Angular 18 Docs](https://angular.dev)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [Docker Docs](https://docs.docker.com)

### Tutoriais
- [Clean Architecture (Uncle Bob)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)

---

**Última atualização:** 2025-10-15  
**Versão:** 1.0  
**Autor:** Tech Lead Alest

**Boa sorte! 🚀**
