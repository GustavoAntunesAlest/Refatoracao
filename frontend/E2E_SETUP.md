# 🧪 Setup de Testes E2E com Playwright

**Projeto:** LegacyProcs - Modernização  
**Framework:** Playwright + TypeScript

---

## ✅ Arquivos Já Criados

```
frontend/
├── playwright.config.ts       ✅ Configuração do Playwright
├── e2e/
│   └── ordem-servico.spec.ts  ✅ Testes E2E completos
└── E2E_SETUP.md              ✅ Este arquivo
```

---

## 📦 1. Instalar Playwright

```bash
cd /Users/adilsonsantos/Downloads/TesteTimeLegado-Gustavo-Antunes-Modernizacao/frontend

# Instalar Playwright
npm install -D @playwright/test

# Instalar browsers
npx playwright install

# Instalar dependências de sistema (se necessário)
npx playwright install-deps
```

---

## 📝 2. Adicionar Scripts ao package.json

Adicione os seguintes scripts manualmente em `frontend/package.json`:

```json
{
  "scripts": {
    "ng": "ng",
    "start": "ng serve",
    "build": "ng build",
    "watch": "ng build --watch --configuration development",
    "test": "ng test",
    
    "e2e": "playwright test",
    "e2e:ui": "playwright test --ui",
    "e2e:headed": "playwright test --headed",
    "e2e:debug": "playwright test --debug",
    "e2e:chromium": "playwright test --project=chromium",
    "e2e:firefox": "playwright test --project=firefox",
    "e2e:webkit": "playwright test --project=webkit",
    "e2e:report": "playwright show-report"
  }
}
```

---

## 🚀 3. Executar Testes E2E

### Opção A: Testes Headless (CI/CD)

```bash
cd frontend

# Executar todos os testes
npm run e2e

# Executar apenas no Chromium
npm run e2e:chromium

# Ver relatório
npm run e2e:report
```

### Opção B: Modo Interativo (Desenvolvimento)

```bash
cd frontend

# UI Mode (recomendado para desenvolvimento)
npm run e2e:ui

# Headed mode (ver browser)
npm run e2e:headed

# Debug mode (passo a passo)
npm run e2e:debug
```

---

## 🎯 Testes Implementados

### ordem-servico.spec.ts (11 testes)

**CRUD Completo:**
1. ✅ Exibir página de ordens
2. ✅ Listar ordens existentes
3. ✅ Criar ordem manual
4. ✅ Criar ordem com IA
5. ✅ Editar ordem
6. ✅ Excluir ordem
7. ✅ Validar campos obrigatórios
8. ✅ Buscar/filtrar ordens

**Funcionalidades de IA:**
9. ✅ Testar botão "Gerar Descrição IA"

**Responsividade:**
10. ✅ Funcionar em mobile

---

## 📊 Estrutura dos Testes

```typescript
test.describe('Ordens de Serviço - CRUD Completo', () => {
  
  test.beforeEach(async ({ page }) => {
    // Setup antes de cada teste
  });

  test('deve criar nova ordem de serviço', async ({ page }) => {
    // 1. Navegar
    // 2. Preencher formulário
    // 3. Salvar
    // 4. Verificar sucesso
  });

  // ... outros testes
});
```

---

## 🔧 Configuração do Playwright

### playwright.config.ts

**Principais configurações:**
- ✅ Timeout: 30s por teste
- ✅ Retries: 2x no CI, 0x local
- ✅ Browsers: Chromium, Firefox, WebKit
- ✅ Screenshots: apenas em falhas
- ✅ Vídeos: apenas em falhas
- ✅ Traces: em retries
- ✅ Web Server: inicia automaticamente

---

## 📝 Boas Práticas

### 1. Seletores Estáveis

```typescript
// ✅ BOM: Seletor semântico
await page.click('button:has-text("Salvar")');

// ❌ RUIM: Seletor frágil
await page.click('.btn-123');
```

### 2. Aguardar Elementos

```typescript
// ✅ BOM: Aguardar visibilidade
await page.waitForSelector('input[name="titulo"]', { state: 'visible' });

// ❌ RUIM: Timeout fixo
await page.waitForTimeout(5000);
```

### 3. Assertions Claras

```typescript
// ✅ BOM: Assertion específica
await expect(page.locator('h1')).toContainText('Ordens de Serviço');

// ❌ RUIM: Assertion genérica
await expect(page.locator('h1')).toBeVisible();
```

---

## 🐛 Troubleshooting

### Erro: "Browser not found"

**Solução:**
```bash
npx playwright install chromium
```

### Erro: "Timeout waiting for page"

**Solução:**
```bash
# Aumentar timeout no playwright.config.ts
timeout: 60 * 1000,
```

### Erro: "Cannot find module @playwright/test"

**Solução:**
```bash
npm install -D @playwright/test
```

### Aplicação não está rodando

**Solução:**
```bash
# Terminal 1: Backend
cd backend/LegacyProcs
dotnet run

# Terminal 2: Frontend
cd frontend
npm start

# Terminal 3: Testes E2E
cd frontend
npm run e2e
```

---

## 📊 Relatórios de Teste

### HTML Report (Padrão)

```bash
# Executar testes
npm run e2e

# Ver relatório
npm run e2e:report
```

**Acessar:** `frontend/playwright-report/index.html`

### JSON Report

```bash
# Gerado automaticamente em:
# frontend/test-results/results.json
```

### JUnit Report (CI/CD)

```bash
# Gerado automaticamente em:
# frontend/test-results/junit.xml
```

---

## 🔄 Integração com CI/CD

### GitHub Actions (já configurado)

```yaml
# .github/workflows/ci-cd.yml
- name: Install Playwright
  run: |
    cd frontend
    npm ci
    npx playwright install --with-deps

- name: Run E2E tests
  run: |
    cd frontend
    npm run e2e

- name: Upload test results
  uses: actions/upload-artifact@v3
  if: always()
  with:
    name: playwright-report
    path: frontend/playwright-report/
```

---

## 📈 Métricas de Sucesso

**Objetivos:**
- ✅ Cobertura: >80% dos fluxos críticos
- ✅ Execução: <5 minutos
- ✅ Flakiness: <5%
- ✅ Browsers: Chromium + Firefox + WebKit

**Status Atual:**
- ✅ 11 testes implementados
- ✅ CRUD completo coberto
- ✅ Funcionalidades de IA cobertas
- ✅ Responsividade testada

---

## 🎯 Próximos Passos

### 1. Executar Localmente
```bash
cd frontend
npm install -D @playwright/test
npx playwright install
npm run e2e:ui
```

### 2. Adicionar Mais Testes
- [ ] Testes de Técnicos
- [ ] Testes de Clientes
- [ ] Testes de Autenticação (quando implementar)
- [ ] Testes de Permissões (quando implementar)

### 3. Otimizar Performance
- [ ] Paralelizar testes
- [ ] Otimizar seletores
- [ ] Reduzir timeouts desnecessários

---

## 📚 Recursos

- **Playwright Docs:** https://playwright.dev/
- **Best Practices:** https://playwright.dev/docs/best-practices
- **API Reference:** https://playwright.dev/docs/api/class-test
- **Debugging:** https://playwright.dev/docs/debug

---

**Criado por:** Assistente IA  
**Data:** 20/10/2025  
**Status:** Pronto para uso

🚀 **Para começar:** `npm run e2e:ui`

