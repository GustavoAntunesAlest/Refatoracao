---
title: "Git Workflow - Guia Completo para Estagiários"
project: "LegacyProcs"
version: "1.0"
date: "2025-10-11"
---

# 🔀 Git Workflow - LegacyProcs

## ⚠️ ATENÇÃO: Versionamento é OBRIGATÓRIO!

O uso de Git é **requisito obrigatório** para aprovação. Você será avaliado também pela qualidade dos commits.

---

## 📦 Repositório Base

```
https://github.com/alest-github/TesteTimeLegado.git
```

**Conteúdo:**
- ✅ Aplicação legada (.NET Framework 4.8 + Angular 12)
- ✅ Banco de dados (script SQL)
- ✅ Documentação para estagiários (`docsAvaliacao/`)
- ❌ Gabaritos (confidenciais, não versionados)

---

## 🚀 Setup Inicial (Primeira vez)

### 1. Clone o Repositório

```bash
git clone https://github.com/alest-github/TesteTimeLegado.git
cd TesteTimeLegado
```

### 2. Crie Sua Branch de Trabalho

**Nomenclatura:** `<seu-nome>/modernizacao`

```bash
# Exemplo para João Silva:
git checkout -b joao-silva/modernizacao

# Exemplo para Maria Santos:
git checkout -b maria-santos/modernizacao
```

### 3. Configure Git (se ainda não fez)

```bash
git config user.name "João Silva"
git config user.email "joao.silva@alest.com.br"
```

### 4. Verifique a Branch

```bash
git branch
# Saída esperada:
# * joao-silva/modernizacao
#   main
```

---

## 📝 Fluxo de Trabalho Diário

### Antes de Começar a Trabalhar

```bash
# 1. Certifique-se de estar na sua branch
git status

# 2. Atualize sua branch com mudanças do main (se houver)
git fetch origin
git merge origin/main
```

### Durante o Desenvolvimento

```bash
# 1. Verifique arquivos modificados
git status

# 2. Adicione arquivos específicos (RECOMENDADO)
git add backend/src/Domain/Entities/OrdemServico.cs
git add backend/tests/UnitTests/OrdemServicoTests.cs

# OU adicione tudo (use com cuidado)
git add .

# 3. Commit com mensagem descritiva (Conventional Commits)
git commit -m "feat(backend): implementa entidade OrdemServico com validações"

# 4. Push para o GitHub
git push origin joao-silva/modernizacao
```

### Fim do Dia

**✅ Checklist diário:**
- [ ] Código compila sem erros
- [ ] Testes passando (se tiver testes nessa fase)
- [ ] Commit feito com mensagem descritiva
- [ ] Push realizado

---

## 📏 Conventional Commits (Obrigatório)

### Formato

```
<tipo>(<escopo>): <descrição curta>

[corpo opcional - detalhes]

[footer opcional - breaking changes, issues]
```

### Tipos Permitidos

| Tipo | Quando Usar | Exemplo |
|------|-------------|---------|
| `feat` | Nova funcionalidade | `feat(backend): adiciona Repository Pattern` |
| `fix` | Correção de bug | `fix(backend): corrige SQL Injection no filtro` |
| `docs` | Documentação | `docs(adr): adiciona ADR-001 sobre arquitetura` |
| `test` | Testes | `test(backend): adiciona testes para OrdemServicoService` |
| `refactor` | Refatoração | `refactor(frontend): extrai lógica HTTP para service` |
| `chore` | Configurações | `chore(docker): configura Docker Compose` |
| `style` | Formatação | `style(backend): aplica EditorConfig` |
| `perf` | Performance | `perf(backend): otimiza query de listagem` |

### Escopos Comuns

- `backend` - Código C# (.NET)
- `frontend` - Código TypeScript (Angular)
- `database` - Scripts SQL, migrations
- `docker` - Dockerfiles, compose
- `ci` - CI/CD (GitHub Actions)
- `docs` - Documentação
- `adr` - Architectural Decision Records

### Exemplos Bons ✅

```bash
# Fase 1 - Análise
git commit -m "docs(adr): cria ADR-001 sobre escolha de Clean Architecture"
git commit -m "docs(analysis): documenta 8 débitos técnicos identificados"
git commit -m "feat(backend): adiciona script OWASP ZAP para análise de segurança"

# Fase 2 - Planejamento
git commit -m "docs(roadmap): define estimativas de esforço por fase"
git commit -m "docs(adr): cria ADR-002 sobre escolha EF Core vs Dapper"

# Fase 3 - Backend
git commit -m "feat(backend): setup projeto .NET 8 com Clean Architecture"
git commit -m "feat(backend): implementa camada Domain com entidade OrdemServico"
git commit -m "feat(backend): adiciona Repository Pattern na camada Infrastructure"
git commit -m "fix(backend): corrige SQL Injection usando parametrização"
git commit -m "test(backend): adiciona testes unitários para Application layer"

# Fase 4 - Frontend
git commit -m "feat(frontend): upgrade Angular 12 para Angular 18"
git commit -m "feat(frontend): implementa service OrdemServicoService"
git commit -m "refactor(frontend): extrai lógica HTTP do componente"
git commit -m "style(frontend): adiciona Angular Material"

# Fase 5 - DevOps
git commit -m "chore(docker): adiciona Dockerfile multi-stage para backend"
git commit -m "ci(github): configura pipeline de testes automatizados"
```

### Exemplos Ruins ❌

```bash
# Muito vago
git commit -m "fix"
git commit -m "atualização"
git commit -m "wip"

# Sem contexto
git commit -m "mudanças"
git commit -m "arrumei o bug"

# Sem tipo
git commit -m "adiciona testes"
git commit -m "backend: corrige erro"
```

---

## 🎯 Commits por Fase

### Fase 1 (Semanas 1-2) - Mínimo 10 commits
- Documentação de análise
- ADRs iniciais
- Scripts de teste/análise

### Fase 2 (Semanas 2-3) - Mínimo 5 commits
- ADRs de decisões técnicas
- Roadmap detalhado
- Documentação de planejamento

### Fase 3 (Semanas 3-6) - Mínimo 30 commits
- Setup .NET 8
- Camadas da Clean Architecture
- Testes unitários/integração
- Correção de vulnerabilidades

### Fase 4 (Semanas 7-8) - Mínimo 20 commits
- Upgrade Angular
- Componentes modernos
- Services estruturados
- UI responsiva

### Fase 5 (Semanas 9-10) - Mínimo 10 commits
- Dockerfiles
- CI/CD pipelines
- Testes E2E
- Documentação final

**Total esperado: 75+ commits**

---

## ✅ Boas Práticas

### ✅ FAZER

1. **Commits pequenos e frequentes**
   ```bash
   # Ruim: 1 commit gigante
   git add .
   git commit -m "feat(backend): implementa backend completo"
   
   # Bom: vários commits incrementais
   git commit -m "feat(backend): adiciona entidade OrdemServico"
   git commit -m "feat(backend): adiciona interface IOrdemServicoRepository"
   git commit -m "feat(backend): implementa OrdemServicoRepository"
   git commit -m "test(backend): adiciona testes para OrdemServicoRepository"
   ```

2. **Commitar código funcional**
   - Compile antes de commitar
   - Execute testes básicos
   - Não commite código quebrado

3. **Mensagens em português (projeto brasileiro)**
   ```bash
   git commit -m "feat(backend): adiciona validação de CPF"
   # Não: "feat(backend): adds CPF validation"
   ```

4. **Push diário**
   - Fim do dia: `git push`
   - Backup automático no GitHub
   - Tech lead acompanha progresso

### ❌ NÃO FAZER

1. **Não commite arquivos grandes**
   ```bash
   # .gitignore já cobre isso, mas cuidado:
   node_modules/     ❌
   bin/, obj/        ❌
   *.mdf, *.ldf      ❌
   packages/         ❌
   ```

2. **Não commite credenciais**
   ```bash
   # ❌ NUNCA
   appsettings.json (com connection strings reais)
   .env (com senhas)
   chaves SSH/API
   ```

3. **Não use `git add . ` sem revisar**
   ```bash
   # Sempre verifique o que vai commitar
   git status
   git diff
   # Depois sim:
   git add .
   ```

4. **Não force push**
   ```bash
   # ❌ NUNCA
   git push --force
   ```

---

## 🔍 Comandos Úteis

### Ver Histórico

```bash
# Log completo
git log

# Log resumido
git log --oneline

# Log gráfico
git log --oneline --graph --all

# Últimos 10 commits
git log -n 10
```

### Desfazer Mudanças

```bash
# Descartar mudanças não commitadas (CUIDADO!)
git checkout -- arquivo.cs

# Desfazer último commit (mantém mudanças)
git reset --soft HEAD~1

# Ver diferenças
git diff
git diff arquivo.cs
```

### Resolver Conflitos

```bash
# 1. Atualizar main
git fetch origin

# 2. Merge main na sua branch
git merge origin/main

# 3. Se houver conflitos, resolva manualmente
# Abra os arquivos marcados como conflito
# Edite, remova <<<<<, =====, >>>>>

# 4. Adicione arquivos resolvidos
git add arquivo-resolvido.cs

# 5. Complete o merge
git commit -m "merge: resolve conflitos com main"
```

---

## 📊 Monitoramento pelo Tech Lead

Seu tech lead vai acompanhar:

1. **Frequência de commits**
   - Esperado: mínimo 3-5 commits/semana
   - Ideal: commits diários

2. **Qualidade das mensagens**
   - Conventional Commits seguidos?
   - Mensagens descritivas?

3. **Tamanho dos commits**
   - Commits atômicos? (1 funcionalidade/commit)
   - Ou commits gigantes? (ruim)

4. **Histórico linear**
   - Branch limpa?
   - Sem commits desnecessários (merge spam)?

---

## 🆘 Problemas Comuns

### "Permission denied" ao fazer push

```bash
# Verifique autenticação GitHub
# Use HTTPS com token pessoal OU SSH

# HTTPS:
git remote set-url origin https://TOKEN@github.com/alest-github/TesteTimeLegado.git

# SSH (preferido):
git remote set-url origin git@github.com:alest-github/TesteTimeLegado.git
```

### "Rejected push" (out of date)

```bash
# Alguém fez push antes de você
git pull origin joao-silva/modernizacao
# Resolva conflitos se houver
git push origin joao-silva/modernizacao
```

### Commitei arquivo errado

```bash
# Se não fez push ainda:
git reset HEAD~1 --soft
# Re-adicione só o que precisa
git add arquivo-correto.cs
git commit -m "..."

# Se já fez push:
# Peça ajuda ao tech lead
```

---

## 📚 Recursos

### Tutoriais
- [Git Book (Português)](https://git-scm.com/book/pt-br/v2)
- [Conventional Commits](https://www.conventionalcommits.org/pt-br/)
- [GitHub Flow](https://guides.github.com/introduction/flow/)

### Ferramentas
- **GitKraken** - GUI visual (recomendado para iniciantes)
- **GitHub Desktop** - GUI simples da GitHub
- **VS Code** - Integração Git nativa (Extension: GitLens)

### Ajuda
- **Tech Lead** - Reuniões semanais
- **Slack #git-help** - Dúvidas rápidas
- **Stack Overflow** - Problemas gerais

---

## ✅ Checklist Final

Antes de cada reunião semanal com tech lead:

- [ ] Branch atualizada (`git push`)
- [ ] Mínimo 3-5 commits na semana
- [ ] Mensagens seguem Conventional Commits
- [ ] Código compila sem erros
- [ ] Testes passando (se aplicável à fase)
- [ ] Sem arquivos grandes/credenciais commitados

**Falta de versionamento adequado = penalização de até 20 pontos na nota final!**

---

**Última atualização:** 2025-10-11  
**Versão:** 1.0
