---
title: "README - Documentação de Avaliação"
project: "LegacyProcs"
version: "1.0"
date: "2025-10-11"
audience: "Estagiários"
---

# 📚 Documentação de Avaliação - LegacyProcs

## 👋 Bem-vindo!

Esta pasta contém **toda a documentação necessária** para você completar com sucesso o projeto de modernização LegacyProcs.

**Você PODE e DEVE consultar estes documentos durante todo o processo!**

---

## 📋 Ordem de Leitura Recomendada

Leia os documentos nesta ordem para melhor compreensão:

### 1️⃣ Primeiro (Antes de começar) - **OBRIGATÓRIO**
- **[../prd.md](../prd.md)** - Documento de Requisitos do Produto
- **[GIT_WORKFLOW.md](./GIT_WORKFLOW.md)** - ⚠️ **Git é OBRIGATÓRIO!**
- **[SETUP_AMBIENTE.md](./SETUP_AMBIENTE.md)** - Como rodar a aplicação legada

### 2️⃣ Segundo (Fase 1 - Análise)
- **[VERSOES.md](./VERSOES.md)** - Comparativo legado vs moderno
- **[INSTRUCOES_ESTAGIARIOS.md](./INSTRUCOES_ESTAGIARIOS.md)** - Guia completo

### 3️⃣ Terceiro (Fase 2 - Planejamento)
- **[ROADMAP_MODERNIZACAO.md](./ROADMAP_MODERNIZACAO.md)** - Timeline detalhada

### 4️⃣ Durante Todo o Projeto (Referência)
- Todos os documentos acima são materiais de **consulta constante**
- Não decore, entenda!

### ⚠️ IMPORTANTE: Versionamento
**Você DEVE criar uma branch e commitar regularmente!**  
Leia: **[GIT_WORKFLOW.md](./GIT_WORKFLOW.md)** para instruções completas.

---

## 📁 Descrição dos Documentos

### 1. SETUP_AMBIENTE.md (15.0 KB)
**Como Rodar a Aplicação Legada**

**Use quando:** Configurar seu ambiente (Fase 1, Dia 1)

**Contém:**
- ✅ Pré-requisitos (software necessário)
- ✅ Setup SQL Server e banco de dados
- ✅ Configuração backend (.NET Framework 4.8)
- ✅ Configuração frontend (Angular 12)
- ✅ Testes funcionais (US01-US04)
- ✅ Troubleshooting (erros comuns)

**Comandos principais:**
```bash
# Backend
# Abrir LegacyProcs.sln no Visual Studio → F5

# Frontend
cd frontend
npm install
ng serve
```

---

### 2. VERSOES.md (9.5 KB)
**Comparativo: Legada vs Moderna**

**Use quando:** Entender o que deve ser modernizado

**Contém:**
- ✅ Tabela comparativa de tecnologias
- ✅ Código legado vs código moderno (exemplos)
- ✅ Arquitetura legada vs Clean Architecture
- ✅ Checklist de modernização

**Exemplo:**
```
Legado:  .NET Framework 4.8, ADO.NET, Angular 12, CSS puro
Moderno: .NET 8, EF Core, Angular 18+, Angular Material
```

---

### 3. INSTRUCOES_ESTAGIARIOS.md (11.4 KB)
**Guia Passo a Passo Completo**

**Use quando:** Durante TODO o projeto (seu roteiro principal)

**Contém:**
- ✅ Processo completo em 5 fases
- ✅ Atividades detalhadas por fase
- ✅ Checklists de entrega
- ✅ Gates de qualidade
- ✅ Comandos úteis (dotnet test, ng build, docker-compose up)

**Estrutura:**
```
Fase 1 (Semanas 1-2): Análise e Inventário
Fase 2 (Semanas 2-3): Planejamento e ADRs
Fase 3 (Semanas 3-6): Backend .NET 8
Fase 4 (Semanas 7-8): Frontend Angular 18+
Fase 5 (Semanas 9-10): DevOps (Docker + CI/CD)
```

---

### 4. ROADMAP_MODERNIZACAO.md (20.4 KB)
**Timeline Detalhada com Estimativas**

**Use quando:** Planejar sprints (Fase 2)

**Contém:**
- ✅ Timeline visual (10 semanas)
- ✅ Detalhamento por sprint
- ✅ Estimativas de esforço (horas por atividade)
- ✅ Marcos (milestones)
- ✅ Riscos e mitigações
- ✅ Planilha de burn-down

**Exemplo:**
```
Sprint 1 (Semanas 3-4): Backend Foundation (32h)
  - Setup .NET 8 (2h)
  - Entidade OrdemServico (4h)
  - Testes unitários (4h)
  - AppDbContext (2h)
  - ...
```

---

## 🎯 Perguntas Frequentes (FAQ)

### Q: Posso consultar estes documentos durante a avaliação?
**A:** ✅ **SIM!** Estes documentos são materiais de apoio. Consulte sempre que precisar.

### Q: Posso copiar código dos exemplos?
**A:** ⚠️ Você pode se **inspirar** nos exemplos, mas deve **entender e adaptar** para seu contexto. Cópia literal sem entendimento é contraproducente.

### Q: Tem gabarito dos débitos técnicos?
**A:** ❌ Não há gabarito público. O objetivo é você **descobrir** os débitos através de análise crítica do código legado.

### Q: E se eu não identificar todos os débitos?
**A:** Identifique o máximo que conseguir. Use ferramentas de análise (OWASP ZAP, linters) e teste manualmente. A nota é proporcional.

### Q: Posso pedir ajuda ao tech lead?
**A:** ✅ SIM para dúvidas de **ferramentas e processo**  
❌ NÃO para **respostas diretas** (ex: "onde está o SQL Injection?")

### Q: Tem exemplo de projeto pronto?
**A:** ❌ Não. O desafio é justamente você implementar do zero seguindo as melhores práticas.

### Q: Posso usar bibliotecas externas (AutoMapper, etc.)?
**A:** ✅ SIM, desde que sejam adequadas e você saiba justificar a escolha.

### Q: E se eu atrasar?
**A:** Comunique ao tech lead IMEDIATAMENTE. Extensões de 1 semana podem ser concedidas com justificativa.

---

## 📊 Checklist de Leitura

Antes de iniciar a Fase 1, confirme que leu:

- [ ] PRD.md (Requisitos do produto)
- [ ] SETUP_AMBIENTE.md (Setup completo)
- [ ] VERSOES.md (Comparativo legado vs moderno)
- [ ] INSTRUCOES_ESTAGIARIOS.md (Processo completo)
- [ ] ROADMAP_MODERNIZACAO.md (Timeline detalhada)

**Tempo estimado de leitura:** 2-3 horas

---

## 🛠️ Ferramentas Essenciais

Durante o projeto você usará:

### Desenvolvimento
- **Visual Studio 2022** (backend .NET 8)
- **Visual Studio Code** (frontend Angular)
- **SQL Server Management Studio** (banco de dados)
- **Postman** (testar APIs)

### Análise
- **OWASP ZAP** (análise de vulnerabilidades)
- **SonarQube** (opcional - análise estática)

### DevOps
- **Docker Desktop** (containerização)
- **Git** (controle de versão)
- **GitHub/Azure DevOps** (CI/CD)

---

## 📚 Recursos Complementares

### Documentação Oficial
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Angular 18 Docs](https://angular.dev)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Docker Docs](https://docs.docker.com)

### Artigos Recomendados
- [Clean Architecture (Uncle Bob)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [OWASP Top 10 2021](https://owasp.org/www-project-top-ten/)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)

### Global Rules (Workspace)
Consulte as **Global Rules** no workspace, especialmente:
- Seção 6: Testes Unificados
- Seção 9: Segurança (OWASP Top 10)
- Seção 11: Modernização de Legacy
- Seção 33: Template ADR
- Seção 49: SOLID
- Seção 52: Clean Architecture

---

## 🆘 Como Pedir Ajuda

### Durante Reuniões Semanais (Segundas, 10h)
- Prepare suas dúvidas com antecedência
- Mostre o que você já tentou
- Seja específico (não "não funciona", mas "erro X ao executar comando Y")

### Fora das Reuniões
- **Issues no repositório:** Para dúvidas técnicas documentadas
- **Email:** Para questões administrativas
- **Slack:** Para dúvidas rápidas (resposta em até 24h)

### O que o Tech Lead PODE ajudar:
✅ Explicar conceitos (Clean Architecture, CQRS, etc.)  
✅ Troubleshooting de ferramentas (OWASP ZAP, Docker)  
✅ Review de código (pull requests)  
✅ Esclarecer requisitos  

### O que o Tech Lead NÃO vai fazer:
❌ Dar respostas diretas ("o SQL Injection está aqui")  
❌ Escrever código por você  
❌ Tomar decisões de arquitetura por você  

---

## 🎓 Mindset para Sucesso

### ✅ Faça:
- Leia a documentação ANTES de perguntar
- Teste localmente ANTES de commitar
- Escreva testes DURANTE a implementação (não depois)
- Commite frequentemente (Conventional Commits)
- Documente decisões técnicas (ADRs)
- Peça code review ANTES de considerar completo

### ❌ Não Faça:
- Copiar código sem entender
- Pular testes ("vou fazer depois")
- Ignorar warnings de build
- Fazer push direto na main
- Esperar até o último dia para pedir ajuda

---

## 📅 Datas Importantes

| Milestone | Semana | Data Limite |
|-----------|--------|-------------|
| **M1:** Análise Aprovada | Semana 2 | [Definir com tech lead] |
| **M2:** Planejamento Aprovado | Semana 3 | [Definir com tech lead] |
| **M3:** Backend Foundation | Semana 4 | [Definir com tech lead] |
| **M4:** Backend Application | Semana 5 | [Definir com tech lead] |
| **M5:** Backend Completo | Semana 6 | [Definir com tech lead] |
| **M6:** Frontend Moderno | Semana 8 | [Definir com tech lead] |
| **M7:** DevOps Completo | Semana 9 | [Definir com tech lead] |
| **M8:** Apresentação Final | Semana 10 | [Definir com tech lead] |

---

## 🚀 Próximos Passos

Agora que você leu este README:

1. ✅ Leia **PRD.md** (requisitos do produto)
2. ✅ Siga **SETUP_AMBIENTE.md** (configure seu ambiente)
3. ✅ Execute a aplicação legada
4. ✅ Teste todas as funcionalidades (US01-US04)
5. ✅ Inicie a análise técnica (Fase 1)

**Boa sorte! 🎯**

---

**Dúvidas?** Fale com seu tech lead!  
**Última atualização:** 2025-10-11  
**Versão:** 1.0
