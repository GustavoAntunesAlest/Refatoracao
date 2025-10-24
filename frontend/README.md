# Frontend LegacyProcs - Angular 12

## ⚠️ ATENÇÃO
Este é um projeto **INTENCIONALMENTE LEGADO** com problemas de arquitetura e UX para fins educacionais.
**NÃO use em produção!**

## Pré-requisitos

- Node.js 16.x (LTS)
- npm 7+ ou yarn
- Angular CLI 12

## Setup

```bash
# Instalar Angular CLI 12 globalmente
npm install -g @angular/cli@12

# Instalar dependências do projeto
npm install

# Se houver erro de dependências
npm install --legacy-peer-deps
```

## Como Executar

```bash
# Modo desenvolvimento (com hot-reload)
ng serve

# Ou usando npm script
npm start
```

Acesse: **http://localhost:4200**

## Build de Produção

```bash
# Build otimizado
ng build --configuration production

# Arquivos gerados em dist/legacyprocs-frontend/
```

## Estrutura

```
frontend/
├── src/
│   ├── app/
│   │   ├── ordem-servico/
│   │   │   ├── ordem-servico.component.ts    # ⚠️ HTTP direto!
│   │   │   ├── ordem-servico.component.html  # ⚠️ Não responsivo!
│   │   │   └── ordem-servico.component.css   # ⚠️ CSS puro!
│   │   ├── app.component.ts
│   │   ├── app.component.html
│   │   ├── app.component.css
│   │   └── app.module.ts
│   ├── environments/
│   │   ├── environment.ts                     # ⚠️ URL hardcoded!
│   │   └── environment.prod.ts
│   ├── index.html
│   ├── main.ts
│   ├── polyfills.ts
│   └── styles.css
├── angular.json
├── package.json
└── tsconfig.json
```

## Problemas Intencionais

Este código contém **vários débitos técnicos**:

1. ❌ HTTP direto no componente (deveria usar service)
2. ❌ Tipagem `any` (deveria ter interfaces)
3. ❌ URL hardcoded (deveria vir de config)
4. ❌ `alert()` e `confirm()` (UX datada)
5. ❌ `location.reload()` (deveria ser reativo)
6. ❌ CSS puro, não responsivo (larguras fixas)
7. ❌ Sem loading states
8. ❌ Sem paginação
9. ❌ Sem tratamento adequado de erros
10. ❌ Lógica de negócio no componente

**Seu desafio:** Identificar TODOS e modernizar para Angular 18+!

## Funcionalidades

- ✅ Listar ordens de serviço
- ✅ Criar nova ordem de serviço
- ✅ Editar ordem de serviço
- ✅ Alterar status (Pendente, Em Andamento, Concluída)
- ✅ Excluir ordem de serviço

## Troubleshooting

### Erro: "Incompatible Node.js version"
```bash
# Use Node.js 16.x
nvm use 16

# Ou instale Node 16 via nvm
nvm install 16
```

### Erro: "ERESOLVE unable to resolve dependency tree"
```bash
npm install --legacy-peer-deps
```

### Erro: "Cannot GET /"
- Verifique se `ng serve` está rodando
- Confirme que está acessando http://localhost:4200

### Backend não responde (CORS error)
- Verifique se o backend está rodando em http://localhost:5000
- Confirme que CORS está habilitado no `Web.config` do backend

### UI quebrada em mobile
- ✅ **ESPERADO!** Este é um problema intencional que você deve corrigir na modernização

## Próximos Passos

1. Execute a aplicação e teste todas as funcionalidades
2. Identifique os débitos técnicos
3. Documente os problemas encontrados
4. Planeje a modernização para Angular 18+
5. Implemente as melhorias conforme o roadmap

## Comandos Úteis

```bash
# Gerar novo componente
ng generate component nome-componente

# Gerar novo service
ng generate service nome-service

# Executar testes (não há testes no legado!)
ng test

# Análise de código
ng lint
```
