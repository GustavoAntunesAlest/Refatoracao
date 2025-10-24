# Inventário de Débitos Técnicos - LegacyProcs

**Data da Análise:** 14/10/2025  
**Responsável:** Gustavo Antunes  
**Branch:** Gustavo-Antunes/Modernizacao

---

## 📊 Resumo Executivo

| Categoria | Quantidade | Severidade Média |
|-----------|------------|------------------|
| **Críticos** | 6 | 🔴 Alta |
| **Altos** | 4 | 🟠 Média-Alta |
| **Médios** | 5 | 🟡 Média |
| **TOTAL** | **15** | - |

---

## 🔴 Débitos Críticos (Prioridade Máxima)

### DT-001: SQL Injection em Múltiplos Endpoints
**Severidade:** 🔴 CRÍTICA  
**Localização:** `backend/LegacyProcs/Controllers/OrdemServicoController.cs`  
**Linhas:** 45, 94, 156-164, 200-205, 238

**Descrição:**
Todas as queries SQL utilizam concatenação de strings sem parametrização, permitindo ataques de SQL Injection.

**Exemplos de Código Vulnerável:**
```csharp
// Linha 45 - GET com filtro
sql = "SELECT * FROM OrdemServico WHERE Titulo LIKE '%" + filtro + "%'";

// Linha 94 - GET por ID
sql = "SELECT * FROM OrdemServico WHERE Id = " + id;

// Linha 156-164 - POST (INSERT)
sql = string.Format(
    "INSERT INTO OrdemServico (Titulo, Descricao, Tecnico, Status, DataCriacao) " +
    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
    os.Titulo, os.Descricao, os.Tecnico, os.Status, os.DataCriacao
);
```

**Impacto:**
- Exposição total do banco de dados
- Possibilidade de leitura, modificação e exclusão de dados
- Execução de comandos arbitrários no servidor SQL
- Violação OWASP Top 10 #1 (Injection)

**Solução Proposta:**
- Implementar queries parametrizadas com `SqlParameter`
- Migrar para Entity Framework Core com LINQ
- Adicionar validação e sanitização de entrada

**Estimativa de Esforço:** 4 horas

---

### DT-002: Credenciais Hardcoded e Versionadas
**Severidade:** 🔴 CRÍTICA  
**Localização:** 
- `backend/LegacyProcs/Controllers/OrdemServicoController.cs` (linha 17)
- `backend/LegacyProcs/Web.config` (linhas 10-12)

**Descrição:**
Connection strings com credenciais em texto plano versionadas no Git.

**Código Vulnerável:**
```csharp
// OrdemServicoController.cs
private string connString = "Server=localhost\\SQLEXPRESS;Database=LegacyProcs;User Id=sa;Password=Admin123!;";

// Web.config
<add name="DefaultConnection" 
     connectionString="Server=localhost\SQLEXPRESS;Database=LegacyProcs;User Id=sa;Password=Admin123!;" 
     providerName="System.Data.SqlClient" />
```

**Impacto:**
- Exposição de credenciais de banco de dados
- Acesso não autorizado ao sistema
- Violação de compliance (LGPD, PCI-DSS)

**Solução Proposta:**
- Mover credenciais para variáveis de ambiente
- Implementar User Secrets para desenvolvimento
- Usar Azure Key Vault ou similar em produção
- Adicionar `.env` ao `.gitignore`

**Estimativa de Esforço:** 2 horas

---

### DT-003: Ausência Total de Testes
**Severidade:** 🔴 CRÍTICA  
**Localização:** Todo o projeto

**Descrição:**
0% de cobertura de testes (unitários, integração ou E2E).

**Impacto:**
- Impossibilidade de garantir qualidade do código
- Risco alto de regressões
- Dificuldade de manutenção e refatoração
- Violação de boas práticas de desenvolvimento

**Solução Proposta:**
- **Backend:** Implementar xUnit + Moq (meta: >80% cobertura)
- **Frontend:** Implementar Jasmine/Jest (meta: >70% cobertura)
- Adicionar testes E2E com Playwright
- Configurar CI/CD com quality gates

**Estimativa de Esforço:** 16 horas

---

### DT-004: Violação Massiva de SOLID/SRP
**Severidade:** 🔴 CRÍTICA  
**Localização:** `backend/LegacyProcs/Controllers/OrdemServicoController.cs`

**Descrição:**
Controllers contêm lógica de negócio, acesso a dados, validação e mapeamento.

**Problemas Identificados:**
- Lógica de negócio no controller (linhas 148-149)
- Acesso direto ao banco de dados (todo o arquivo)
- Mapeamento manual de objetos (linhas 53-66)
- Validações no controller (linhas 136-143)

**Impacto:**
- Código não testável
- Alto acoplamento
- Dificuldade de manutenção
- Impossibilidade de reutilização

**Solução Proposta:**
- Implementar Clean Architecture (4 camadas)
- Criar camada de Domain com entidades ricas
- Implementar Repository Pattern
- Criar camada de Application com Use Cases
- Adicionar AutoMapper para mapeamentos

**Estimativa de Esforço:** 12 horas

---

### DT-005: Model Anêmico (Domain Model Anêmico)
**Severidade:** 🔴 CRÍTICA  
**Localização:** `backend/LegacyProcs/Models/OrdemServico.cs`

**Descrição:**
Modelo contém apenas getters/setters sem lógica de negócio ou validações.

**Código Atual:**
```csharp
public class OrdemServico
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Tecnico { get; set; }
    public string Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
```

**Impacto:**
- Lógica de negócio espalhada pelo código
- Falta de encapsulamento
- Estados inválidos possíveis
- Violação de Domain-Driven Design

**Solução Proposta:**
- Criar entidade rica com validações
- Implementar Value Objects (Status, Titulo)
- Adicionar métodos de negócio (AlterarStatus, Atualizar)
- Implementar validações no construtor

**Estimativa de Esforço:** 4 horas

---

### DT-006: Exposição de Detalhes Internos em Erros
**Severidade:** 🔴 CRÍTICA  
**Localização:** `backend/LegacyProcs/Controllers/OrdemServicoController.cs` (múltiplas linhas)

**Descrição:**
Exceções completas são retornadas ao cliente, expondo stack traces e informações sensíveis.

**Código Vulnerável:**
```csharp
catch (Exception ex)
{
    return InternalServerError(ex); // Expõe stack trace completo
}
```

**Impacto:**
- Exposição de estrutura interna do sistema
- Facilita ataques direcionados
- Violação OWASP Top 10 #5 (Security Misconfiguration)

**Solução Proposta:**
- Implementar middleware de tratamento de erros
- Retornar mensagens genéricas ao cliente
- Logar detalhes internamente (Serilog)
- Configurar `customErrors mode="On"` em produção

**Estimativa de Esforço:** 3 horas

---

## 🟠 Débitos Altos (Prioridade Alta)

### DT-007: Ausência de Paginação
**Severidade:** 🟠 ALTA  
**Localização:** `backend/LegacyProcs/Controllers/OrdemServicoController.cs` (linha 40)

**Descrição:**
Endpoint GET retorna todos os registros sem paginação.

**Código Atual:**
```csharp
sql = "SELECT * FROM OrdemServico ORDER BY DataCriacao DESC";
```

**Impacto:**
- Performance degradada com muitos registros
- Alto consumo de memória
- Timeout em grandes volumes
- Experiência ruim do usuário

**Solução Proposta:**
- Implementar paginação com `PagedList`
- Adicionar parâmetros `page` e `pageSize`
- Retornar metadados (totalCount, totalPages)
- Implementar cursor-based pagination para escala

**Estimativa de Esforço:** 3 horas

---

### DT-008: Frontend Não Testável (HTTP Direto no Componente)
**Severidade:** 🟠 ALTA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.ts`

**Descrição:**
HttpClient injetado diretamente no componente, impossibilitando testes unitários.

**Código Atual:**
```typescript
constructor(private http: HttpClient) { }

carregarOrdens() {
    this.http.get(this.apiUrl).subscribe(...);
}
```

**Impacto:**
- Componentes não testáveis
- Alto acoplamento
- Duplicação de lógica HTTP
- Violação de Single Responsibility

**Solução Proposta:**
- Criar `OrdemServicoService`
- Mover toda lógica HTTP para o service
- Implementar interfaces para testabilidade
- Adicionar testes unitários com mocks

**Estimativa de Esforço:** 4 horas

---

### DT-009: URLs Hardcoded no Frontend
**Severidade:** 🟠 ALTA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.ts` (linha 18)

**Descrição:**
URLs da API hardcoded no código, dificultando deploy em diferentes ambientes.

**Código Atual:**
```typescript
private apiUrl = 'http://localhost:5000/api/ordemservico';
```

**Impacto:**
- Impossibilidade de deploy em múltiplos ambientes
- Necessidade de rebuild para mudanças de URL
- Violação de 12-Factor App

**Solução Proposta:**
- Criar `environment.ts` e `environment.prod.ts`
- Centralizar configurações de API
- Implementar interceptor para base URL
- Adicionar variáveis de ambiente no build

**Estimativa de Esforço:** 2 horas

---

### DT-010: CORS Configurado de Forma Insegura
**Severidade:** 🟠 ALTA  
**Localização:** `backend/LegacyProcs/Web.config` (linhas 36-39)

**Descrição:**
CORS configurado para aceitar requisições de qualquer origem (`*`).

**Código Atual:**
```xml
<add name="Access-Control-Allow-Origin" value="*" />
```

**Impacto:**
- Vulnerabilidade a ataques CSRF
- Exposição da API para qualquer domínio
- Violação de segurança

**Solução Proposta:**
- Configurar origens específicas permitidas
- Implementar CORS policy no .NET 8
- Adicionar validação de tokens
- Implementar CSRF protection

**Estimativa de Esforço:** 2 horas

---

## 🟡 Débitos Médios (Prioridade Média)

### DT-011: Deploy Manual Sem CI/CD
**Severidade:** 🟡 MÉDIA  
**Localização:** Todo o projeto

**Descrição:**
Ausência de pipeline de CI/CD automatizado.

**Impacto:**
- Processo de deploy lento e propenso a erros
- Falta de automação de testes
- Inconsistência entre ambientes

**Solução Proposta:**
- Implementar GitHub Actions
- Criar pipeline de build, test e deploy
- Adicionar quality gates (cobertura, linting)
- Implementar Docker + Docker Compose

**Estimativa de Esforço:** 8 horas

---

### DT-012: UI Não Responsiva
**Severidade:** 🟡 MÉDIA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.css`

**Descrição:**
Interface com larguras fixas, não adaptada para mobile/tablet.

**Impacto:**
- Experiência ruim em dispositivos móveis
- Perda de usuários mobile
- Violação de boas práticas de UX

**Solução Proposta:**
- Implementar Angular Material ou PrimeNG
- Adicionar media queries
- Usar flexbox/grid layout
- Testar em múltiplos dispositivos

**Estimativa de Esforço:** 6 horas

---

### DT-013: Ausência de Tipagem no Frontend
**Severidade:** 🟡 MÉDIA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.ts`

**Descrição:**
Uso excessivo de `any` em vez de interfaces tipadas.

**Código Atual:**
```typescript
ordens: any[] = [];
novaOrdem: any = { ... };
```

**Impacto:**
- Perda de type safety
- Erros em runtime
- Dificuldade de manutenção
- Perda de IntelliSense

**Solução Proposta:**
- Criar interfaces/models TypeScript
- Remover todos os `any`
- Habilitar `strict: true` no tsconfig
- Adicionar validação de tipos

**Estimativa de Esforço:** 2 horas

---

### DT-014: Tratamento de Erro com Alert()
**Severidade:** 🟡 MÉDIA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.ts` (múltiplas linhas)

**Descrição:**
Uso de `alert()` e `confirm()` nativos para feedback ao usuário.

**Código Atual:**
```typescript
alert('Erro ao carregar ordens de serviço!');
if (confirm('Tem certeza?')) { ... }
```

**Impacto:**
- UX datada e não profissional
- Bloqueio da interface
- Impossibilidade de customização

**Solução Proposta:**
- Implementar componente de notificação (toast)
- Usar Angular Material Dialog
- Criar serviço de notificações
- Adicionar feedback visual moderno

**Estimativa de Esforço:** 4 horas

---

### DT-015: Reload Completo da Página (location.reload)
**Severidade:** 🟡 MÉDIA  
**Localização:** `frontend/src/app/ordem-servico/ordem-servico.component.ts` (linhas 100, 127, 144, 161)

**Descrição:**
Uso de `location.reload()` após operações CRUD, perdendo estado da aplicação.

**Código Atual:**
```typescript
this.http.post(this.apiUrl, this.novaOrdem).subscribe(
    (response) => {
        alert('Criada com sucesso!');
        location.reload(); // ❌ Reload completo
    }
);
```

**Impacto:**
- Perda de estado da aplicação
- Performance ruim
- UX degradada
- Perda de dados não salvos

**Solução Proposta:**
- Atualizar lista localmente após operações
- Implementar state management (RxJS/Signals)
- Remover todos os `location.reload()`
- Adicionar loading states

**Estimativa de Esforço:** 3 horas

---

## 📋 Resumo de Esforço Estimado

| Categoria | Débitos | Horas Estimadas |
|-----------|---------|-----------------|
| Críticos | 6 | 41h |
| Altos | 4 | 11h |
| Médios | 5 | 23h |
| **TOTAL** | **15** | **75h** |

---

## 🎯 Priorização Recomendada

### Sprint 1 (Semana 1-2): Segurança Crítica
1. DT-001: SQL Injection (4h)
2. DT-002: Credenciais Hardcoded (2h)
3. DT-006: Exposição de Erros (3h)
4. DT-010: CORS Inseguro (2h)

**Total Sprint 1:** 11h

### Sprint 2 (Semana 3-4): Arquitetura Backend
1. DT-004: Violação SOLID (12h)
2. DT-005: Model Anêmico (4h)
3. DT-007: Paginação (3h)

**Total Sprint 2:** 19h

### Sprint 3 (Semana 5-6): Arquitetura Frontend
1. DT-008: HTTP no Componente (4h)
2. DT-009: URLs Hardcoded (2h)
3. DT-013: Tipagem (2h)
4. DT-015: Location Reload (3h)

**Total Sprint 3:** 11h

### Sprint 4 (Semana 7-8): Testes e UX
1. DT-003: Testes (16h)
2. DT-012: UI Responsiva (6h)
3. DT-014: Alert/Confirm (4h)

**Total Sprint 4:** 26h

### Sprint 5 (Semana 9-10): DevOps
1. DT-011: CI/CD (8h)

**Total Sprint 5:** 8h

---

## 📊 Métricas de Qualidade Atuais

| Métrica | Valor Atual | Meta |
|---------|-------------|------|
| Cobertura de Testes Backend | 0% | >80% |
| Cobertura de Testes Frontend | 0% | >70% |
| Vulnerabilidades Críticas (OWASP ZAP) | 8+ | 0 |
| Code Smells (SonarQube) | N/A | <50 |
| Duplicação de Código | N/A | <3% |
| Complexidade Ciclomática | N/A | <10 |

---

## 🔍 Ferramentas de Análise Utilizadas

- ✅ Análise manual de código
- ✅ Revisão de documentação do projeto
- ⏳ OWASP ZAP (pendente)
- ⏳ SonarQube (pendente)
- ⏳ Testes de penetração (pendente)

---

## 📝 Observações Finais

Este inventário documenta débitos técnicos **intencionais** criados para fins educacionais. A refatoração seguirá uma abordagem incremental, priorizando segurança e arquitetura antes de UX e DevOps.

**Próximos Passos:**
1. Executar OWASP ZAP para análise de segurança
2. Criar ADRs (Architecture Decision Records)
3. Iniciar refatoração pela Sprint 1

---

**Documento Vivo:** Este inventário será atualizado conforme novos débitos forem identificados durante a refatoração.
