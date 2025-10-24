import { defineConfig, devices } from '@playwright/test';

/**
 * Configuração Playwright para Testes E2E
 * Projeto: LegacyProcs Modernização
 */
export default defineConfig({
  testDir: './e2e',
  
  // Timeout padrão para cada teste
  timeout: 30 * 1000,
  
  // Timeout para expects
  expect: {
    timeout: 5000
  },
  
  // Configuração de paralelismo
  fullyParallel: true,
  
  // Falhar se houver only() no CI
  forbidOnly: !!process.env.CI,
  
  // Retries no CI
  retries: process.env.CI ? 2 : 0,
  
  // Workers (1 no CI, undefined para usar todos os cores localmente)
  workers: process.env.CI ? 1 : undefined,
  
  // Reporter
  reporter: [
    ['html'],
    ['json', { outputFile: 'test-results/results.json' }],
    ['junit', { outputFile: 'test-results/junit.xml' }]
  ],
  
  // Configurações compartilhadas
  use: {
    // URL base da aplicação
    baseURL: 'http://localhost:4200',
    
    // Coleta de traces em falhas
    trace: 'on-first-retry',
    
    // Screenshots em falhas
    screenshot: 'only-on-failure',
    
    // Vídeos em falhas
    video: 'retain-on-failure',
    
    // Timeout de navegação
    navigationTimeout: 15 * 1000,
  },

  // Configuração de projetos (browsers)
  projects: [
    {
      name: 'chromium',
      use: { 
        ...devices['Desktop Chrome'],
        // Viewport padrão
        viewport: { width: 1280, height: 720 }
      },
    },

    {
      name: 'firefox',
      use: { 
        ...devices['Desktop Firefox'],
        viewport: { width: 1280, height: 720 }
      },
    },

    {
      name: 'webkit',
      use: { 
        ...devices['Desktop Safari'],
        viewport: { width: 1280, height: 720 }
      },
    },

    // Testes mobile (opcional)
    // {
    //   name: 'Mobile Chrome',
    //   use: { ...devices['Pixel 5'] },
    // },
    // {
    //   name: 'Mobile Safari',
    //   use: { ...devices['iPhone 12'] },
    // },
  ],

  // Web Server (inicia automaticamente a aplicação)
  webServer: {
    command: 'npm start',
    url: 'http://localhost:4200',
    reuseExistingServer: !process.env.CI,
    timeout: 120 * 1000,
    stdout: 'ignore',
    stderr: 'pipe',
  },
});

