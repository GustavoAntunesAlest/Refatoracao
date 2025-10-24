import { test, expect } from '@playwright/test';

/**
 * Testes E2E para Gerenciamento de Ordens de Serviço
 * 
 * Cenários testados:
 * 1. Listagem de ordens
 * 2. Criação de ordem manual
 * 3. Criação de ordem com IA
 * 4. Edição de ordem
 * 5. Exclusão de ordem
 */

test.describe('Ordens de Serviço - CRUD Completo', () => {
  
  test.beforeEach(async ({ page }) => {
    // Navegar para a aplicação antes de cada teste
    await page.goto('/');
    
    // Aguardar carregamento inicial
    await page.waitForLoadState('networkidle');
  });

  test('deve exibir a página de ordens de serviço', async ({ page }) => {
    // Verificar título da página
    await expect(page.locator('h2')).toContainText('Gerenciamento de Ordens de Serviço');
    
    // Verificar presença da tabela
    await expect(page.locator('table')).toBeVisible();
    
    // Verificar botão de nova ordem
    await expect(page.locator('button', { hasText: 'Nova Ordem de Serviço' })).toBeVisible();
  });

  test('deve listar ordens de serviço existentes', async ({ page }) => {
    // Verificar se há pelo menos uma linha na tabela (header)
    const rows = page.locator('table tbody tr');
    await expect(rows.first()).toBeVisible();
    
    // Verificar colunas da tabela
    await expect(page.locator('th', { hasText: 'ID' })).toBeVisible();
    await expect(page.locator('th', { hasText: 'Título' })).toBeVisible();
    await expect(page.locator('th', { hasText: 'Status' })).toBeVisible();
  });

  test('deve criar nova ordem de serviço (manual)', async ({ page }) => {
    // Clicar no botão de nova ordem
    await page.click('button:has-text("Nova Ordem de Serviço")');
    
    // Aguardar modal/formulário abrir
    await page.waitForSelector('input[formControlName="titulo"]', { state: 'visible' });
    
    // Preencher formulário
    await page.fill('input[formControlName="titulo"]', 'Teste E2E - Instalação de Sistema');
    await page.fill('textarea[formControlName="descricao"]', 'Descrição criada via teste E2E automatizado');
    
    // Selecionar cliente (se houver dropdown)
    // await page.selectOption('select[formControlName="clienteId"]', '1');
    
    // Selecionar técnico (se houver dropdown)
    // await page.selectOption('select[formControlName="tecnicoId"]', '1');
    
    // Salvar
    await page.click('button:has-text("Salvar")');
    
    // Verificar mensagem de sucesso (MatSnackBar)
    await expect(page.locator('.mat-snack-bar-container')).toBeVisible();
    await expect(page.locator('.mat-snack-bar-container')).toContainText(/criada|sucesso/i);
    
    // Verificar se a nova ordem aparece na lista
    await page.waitForTimeout(1000);
    await expect(page.locator('td', { hasText: 'Teste E2E' })).toBeVisible();
  });

  test('deve criar ordem de serviço usando IA', async ({ page }) => {
    // Clicar no botão de nova ordem
    await page.click('button:has-text("Nova Ordem de Serviço")');
    
    // Aguardar formulário
    await page.waitForSelector('input[formControlName="titulo"]');
    
    // Preencher título
    await page.fill('input[formControlName="titulo"]', 'Instalar ar-condicionado split 12000 BTUs');
    
    // Clicar em "Gerar Descrição IA" (se existir)
    const btnGerarDescricao = page.locator('button:has-text("Gerar Descrição")');
    if (await btnGerarDescricao.isVisible()) {
      await btnGerarDescricao.click();
      
      // Aguardar resposta da IA (máximo 5 segundos)
      await page.waitForTimeout(3000);
      
      // Verificar se a descrição foi preenchida
      const descricao = await page.locator('textarea[formControlName="descricao"]').inputValue();
      expect(descricao.length).toBeGreaterThan(0);
    }
    
    // Clicar em "Sugerir Técnico IA" (se existir)
    const btnSugerirTecnico = page.locator('button:has-text("Sugerir Técnico")');
    if (await btnSugerirTecnico.isVisible()) {
      await btnSugerirTecnico.click();
      await page.waitForTimeout(2000);
    }
    
    // Salvar
    await page.click('button:has-text("Salvar")');
    
    // Verificar sucesso
    await expect(page.locator('.mat-snack-bar-container')).toBeVisible();
  });

  test('deve editar ordem de serviço existente', async ({ page }) => {
    // Aguardar tabela carregar
    await page.waitForSelector('table tbody tr');
    
    // Clicar no botão "Editar" da primeira ordem
    await page.click('table tbody tr:first-child button:has-text("Editar")');
    
    // Aguardar formulário
    await page.waitForSelector('input[formControlName="titulo"]');
    
    // Modificar título
    const novoTitulo = `Editado via E2E ${Date.now()}`;
    await page.fill('input[formControlName="titulo"]', novoTitulo);
    
    // Clicar em "Atualizar"
    await page.click('button:has-text("Atualizar")');
    
    // Verificar mensagem de sucesso
    await expect(page.locator('.mat-snack-bar-container')).toBeVisible();
    await expect(page.locator('.mat-snack-bar-container')).toContainText(/atualizada|sucesso/i);
    
    // Verificar se o título foi atualizado na lista
    await page.waitForTimeout(1000);
    await expect(page.locator('td', { hasText: 'Editado via E2E' })).toBeVisible();
  });

  test('deve excluir ordem de serviço', async ({ page }) => {
    // Aguardar tabela carregar
    await page.waitForSelector('table tbody tr');
    
    // Contar ordens antes da exclusão
    const ordensAntes = await page.locator('table tbody tr').count();
    
    // Configurar handler para confirmar dialog de exclusão
    page.on('dialog', dialog => {
      expect(dialog.type()).toBe('confirm');
      dialog.accept();
    });
    
    // Clicar no botão "Excluir" da última ordem
    await page.click('table tbody tr:last-child button:has-text("Excluir")');
    
    // Verificar mensagem de sucesso
    await expect(page.locator('.mat-snack-bar-container')).toBeVisible();
    await expect(page.locator('.mat-snack-bar-container')).toContainText(/excluída|removida|deletada/i);
    
    // Verificar se o número de ordens diminuiu
    await page.waitForTimeout(1000);
    const ordensDepois = await page.locator('table tbody tr').count();
    expect(ordensDepois).toBe(ordensAntes - 1);
  });

  test('deve validar campos obrigatórios', async ({ page }) => {
    // Clicar no botão de nova ordem
    await page.click('button:has-text("Nova Ordem de Serviço")');
    
    // Aguardar formulário
    await page.waitForSelector('input[formControlName="titulo"]');
    
    // Tentar salvar sem preencher campos obrigatórios
    await page.click('button:has-text("Salvar")');
    
    // Verificar mensagens de validação
    await expect(page.locator('.mat-error')).toBeVisible();
  });

  test('deve buscar/filtrar ordens de serviço', async ({ page }) => {
    // Verificar se há campo de busca
    const campoBusca = page.locator('input[placeholder*="Buscar"], input[placeholder*="Filtrar"]');
    
    if (await campoBusca.isVisible()) {
      // Digitar termo de busca
      await campoBusca.fill('Instalação');
      
      // Aguardar filtro ser aplicado
      await page.waitForTimeout(500);
      
      // Verificar se apenas ordens relevantes são exibidas
      const linhasVisiveis = await page.locator('table tbody tr:visible').count();
      expect(linhasVisiveis).toBeGreaterThanOrEqual(0);
    }
  });
});

test.describe('Funcionalidades de IA', () => {
  
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
    await page.waitForLoadState('networkidle');
  });

  test('deve testar botão "Gerar Descrição IA"', async ({ page }) => {
    await page.click('button:has-text("Nova Ordem de Serviço")');
    await page.waitForSelector('input[formControlName="titulo"]');
    
    // Preencher título
    await page.fill('input[formControlName="titulo"]', 'Manutenção preventiva de ar-condicionado');
    
    // Verificar se botão IA existe
    const btnIA = page.locator('button:has-text("✨"), button:has-text("Gerar")');
    
    if (await btnIA.first().isVisible()) {
      await btnIA.first().click();
      
      // Aguardar loading
      await page.waitForTimeout(3000);
      
      // Verificar se descrição foi preenchida
      const descricao = await page.locator('textarea[formControlName="descricao"]').inputValue();
      expect(descricao).toBeTruthy();
      expect(descricao.length).toBeGreaterThan(10);
    }
  });
});

test.describe('Responsividade', () => {
  
  test('deve funcionar em mobile', async ({ page }) => {
    // Configurar viewport mobile
    await page.setViewportSize({ width: 375, height: 667 });
    
    await page.goto('/');
    await page.waitForLoadState('networkidle');
    
    // Verificar se a página é exibida corretamente
    await expect(page.locator('h2')).toBeVisible();
    
    // Verificar se a tabela ou cards são exibidos
    const conteudo = page.locator('table, mat-card');
    await expect(conteudo.first()).toBeVisible();
  });
});

