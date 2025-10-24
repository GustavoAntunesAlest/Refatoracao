# 📊 Comparativo: Versão Legada vs Versão Moderna

**Objetivo:** Entender as diferenças entre as versões para guiar a modernização  
**Público:** Estagiários em treinamento

---

## 🎯 Visão Geral

| Aspecto | Legado | Moderno | Ganho |
|---------|--------|---------|-------|
| **Backend Framework** | .NET Framework 4.8 | .NET 8 LTS | Performance 2-3x, cross-platform |
| **Frontend Framework** | Angular 12 | Angular 18+ | Signals, standalone components |
| **Arquitetura** | Monolítica | Clean Architecture | Manutenibilidade, testabilidade |
| **Padrões** | Nenhum | CQRS + MediatR | Separação de responsabilidades |
| **ORM** | ADO.NET (raw SQL) | Entity Framework Core 9 | Produtividade, type-safety |
| **Testes** | 0% cobertura | >80% backend, >70% frontend | Qualidade, confiança |
| **Deploy** | Manual (Publish) | Docker + CI/CD | Automação, consistência |
| **Segurança** | SQL Injection, hardcoded | Parametrizado, appsettings | Conformidade OWASP |

---

## 1️⃣ Stack Tecnológica

### Backend

| Componente | Legado | Moderno | Justificativa |
|------------|--------|---------|---------------|
| **Runtime** | .NET Framework 4.8 | .NET 8 LTS | Performance, cross-platform, suporte até 2026 |
| **API Framework** | ASP.NET Web API 2 | ASP.NET Core Minimal APIs | Simplicidade, performance |
| **Data Access** | ADO.NET | Entity Framework Core 9 | Produtividade, LINQ, migrations |
| **Dependency Injection** | Manual (new) | Built-in DI | Testabilidade, IoC |
| **Logging** | Console.WriteLine | Serilog | Estruturado, níveis, sinks |
| **Validation** | Manual (if/else) | FluentValidation | Declarativo, reutilizável |
| **API Documentation** | Nenhum | Swagger/OpenAPI | Auto-documentação |
| **Testing** | Nenhum | xUnit + Moq + FluentAssertions | Qualidade, TDD |

### Frontend

| Componente | Legado | Moderno | Justificativa |
|------------|--------|---------|---------------|
| **Framework** | Angular 12 | Angular 18+ | Signals, performance, standalone |
| **TypeScript** | 4.3 | 5.4+ | Type safety melhorado |
| **State Management** | Nenhum | Signals | Reatividade nativa |
| **UI Library** | CSS puro | Angular Material | Componentes prontos, acessibilidade |
| **HTTP** | HttpClient direto | Services + Repository | Separação de responsabilidades |
| **Forms** | Template-driven | Reactive Forms | Validação, testabilidade |
| **Testing** | Nenhum | Jasmine + Karma | Qualidade, TDD |
| **E2E Testing** | Nenhum | Playwright | Testes de integração |

### DevOps

| Componente | Legado | Moderno | Justificativa |
|------------|--------|---------|---------------|
| **Containerização** | Nenhum | Docker + Docker Compose | Consistência, portabilidade |
| **CI/CD** | Nenhum | GitHub Actions | Automação, quality gates |
| **Monitoramento** | Nenhum | Logs estruturados | Observabilidade |
| **Deploy** | Manual (FTP/Publish) | Automatizado (CI/CD) | Velocidade, confiabilidade |

---

## 2️⃣ Arquitetura

### Legado: Monolítica

```
┌─────────────────────────────────┐
│         Controllers             │
│  (Lógica + Data Access + UI)    │
│                                 │
│  ┌──────────────────────────┐  │
│  │  OrdemServicoController  │  │
│  │  - GetAll()              │  │
│  │  - Create()              │  │
│  │  - Update()              │  │
│  │  - Delete()              │  │
│  │                          │  │
│  │  SQL direto no controller│  │
│  │  Validação no controller │  │
│  │  Lógica no controller    │  │
│  └──────────────────────────┘  │
└─────────────────────────────────┘
         ↓
    SQL Server
```

**Problemas:**
- ❌ Tudo em um lugar (God Object)
- ❌ Difícil de testar
- ❌ Difícil de manter
- ❌ Viola SOLID

---

### Moderno: Clean Architecture

```
┌─────────────────────────────────────────────┐
│              Presentation                    │
│         (Controllers/API)                    │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│             Application                      │
│  (Commands, Queries, Handlers, DTOs)        │
│                                              │
│  ┌──────────────┐    ┌──────────────┐      │
│  │  Commands    │    │   Queries    │      │
│  │  (Write)     │    │   (Read)     │      │
│  └──────────────┘    └──────────────┘      │
│         │                    │               │
│         ▼                    ▼               │
│  ┌──────────────┐    ┌──────────────┐      │
│  │   Handlers   │    │   Handlers   │      │
│  └──────────────┘    └──────────────┘      │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│               Domain                         │
│         (Entities, Rules)                    │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│          Infrastructure                      │
│    (Repositories, DbContext, External)       │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
            SQL Server
```

**Benefícios:**
- ✅ Separação de responsabilidades
- ✅ Testável (cada camada isolada)
- ✅ Manutenível
- ✅ Segue SOLID

---

## 3️⃣ Exemplos de Código

### Exemplo 1: Data Access

**Legado (ADO.NET):**
```csharp
// ❌ SQL Injection, sem parametrização, verboso
public List<OrdemServico> GetAll(string filtro)
{
    var lista = new List<OrdemServico>();
    var connString = "Server=localhost;Database=..."; // ❌ Hardcoded
    
    using (var conn = new SqlConnection(connString))
    {
        conn.Open();
        // ❌ SQL Injection!
        var sql = $"SELECT * FROM OrdemServico WHERE Titulo LIKE '%{filtro}%'";
        var cmd = new SqlCommand(sql, conn);
        
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                lista.Add(new OrdemServico
                {
                    Id = (int)reader["Id"],
                    Titulo = reader["Titulo"].ToString(),
                    // ... manual mapping
                });
            }
        }
    }
    return lista;
}
```

**Moderno (EF Core + Repository):**
```csharp
// ✅ Parametrizado, LINQ, type-safe, testável
public async Task<IEnumerable<OrdemServico>> GetAllAsync(string? filtro = null)
{
    var query = _context.OrdensServico.AsQueryable();
    
    if (!string.IsNullOrEmpty(filtro))
    {
        query = query.Where(os => os.Titulo.Contains(filtro)); // ✅ Parametrizado
    }
    
    return await query
        .OrderByDescending(os => os.DataCriacao)
        .ToListAsync();
}
```

---

### Exemplo 2: Controller

**Legado:**
```csharp
// ❌ Lógica + Data Access + Validação tudo junto
[HttpPost]
public IHttpActionResult Create(OrdemServico ordem)
{
    // ❌ Validação no controller
    if (string.IsNullOrEmpty(ordem.Titulo))
    {
        return BadRequest("Título é obrigatório");
    }
    
    // ❌ Data access no controller
    var connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
    using (var conn = new SqlConnection(connString))
    {
        conn.Open();
        // ❌ SQL Injection
        var sql = $"INSERT INTO OrdemServico (Titulo, Descricao) VALUES ('{ordem.Titulo}', '{ordem.Descricao}')";
        var cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }
    
    return Ok();
}
```

**Moderno (CQRS + MediatR):**
```csharp
// ✅ Apenas coordenação, lógica delegada
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateOrdemServicoCommand command)
{
    if (!ModelState.IsValid) // ✅ Validação automática
    {
        return BadRequest(ModelState);
    }
    
    var id = await _mediator.Send(command); // ✅ Delegado para handler
    
    return CreatedAtAction(nameof(GetById), new { id }, new { id });
}

// Handler separado (testável)
public class CreateOrdemServicoHandler : IRequestHandler<CreateOrdemServicoCommand, int>
{
    private readonly IOrdemServicoRepository _repository;
    
    public async Task<int> Handle(CreateOrdemServicoCommand request, CancellationToken ct)
    {
        var ordem = new OrdemServico
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Tecnico = request.Tecnico,
            Status = "Pendente",
            DataCriacao = DateTime.Now
        };
        
        return await _repository.CreateAsync(ordem);
    }
}
```

---

### Exemplo 3: Frontend Component

**Legado (Angular 12):**
```typescript
// ❌ HTTP direto, sem service, sem tipagem forte
export class OrdemServicoComponent {
  ordens: any[] = []; // ❌ any
  
  constructor(private http: HttpClient) {}
  
  ngOnInit() {
    // ❌ HTTP direto no componente
    this.http.get('http://localhost:5000/api/ordemservico').subscribe(
      (data: any) => {
        this.ordens = data;
      },
      (error) => {
        console.error(error); // ❌ Tratamento ruim
        alert('Erro!'); // ❌ UX ruim
      }
    );
  }
  
  criar() {
    // ❌ Validação manual
    if (!this.novaOrdem.titulo) {
      alert('Título é obrigatório!');
      return;
    }
    
    // ❌ HTTP direto
    this.http.post('http://localhost:5000/api/ordemservico', this.novaOrdem).subscribe(
      () => {
        alert('Criado!');
        location.reload(); // ❌ Reload da página
      }
    );
  }
}
```

**Moderno (Angular 18 + Signals):**
```typescript
// ✅ Service, tipagem, signals, reactive
export class OrdemServicoComponent {
  ordens = signal<OrdemServico[]>([]);
  loading = signal(false);
  
  constructor(
    private ordemService: OrdemServicoService, // ✅ Service
    private snackBar: MatSnackBar // ✅ Material
  ) {}
  
  ngOnInit() {
    this.carregarOrdens();
  }
  
  carregarOrdens() {
    this.loading.set(true);
    this.ordemService.getAll().subscribe({
      next: (data) => {
        this.ordens.set(data);
        this.loading.set(false);
      },
      error: (error) => {
        this.snackBar.open('Erro ao carregar ordens', 'Fechar', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }
  
  criar(form: FormGroup) {
    if (form.invalid) { // ✅ Reactive Forms
      return;
    }
    
    this.ordemService.create(form.value).subscribe({
      next: () => {
        this.snackBar.open('Ordem criada com sucesso!', 'Fechar', { duration: 3000 });
        this.carregarOrdens(); // ✅ Atualiza lista sem reload
        form.reset();
      }
    });
  }
}
```

---

## 4️⃣ Testes

### Legado: Sem Testes

```
❌ Nenhum teste
❌ Cobertura: 0%
❌ Confiança: Baixa
❌ Refatoração: Arriscada
```

### Moderno: Testes Automatizados

**Backend (xUnit):**
```csharp
public class OrdemServicoRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrdens()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LegacyProcsDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        var context = new LegacyProcsDbContext(options);
        var repository = new OrdemServicoRepository(context);
        
        // Act
        var result = await repository.GetAllAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }
}
```

**Frontend (Jasmine):**
```typescript
describe('OrdemServicoService', () => {
  let service: OrdemServicoService;
  let httpMock: HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrdemServicoService]
    });
    service = TestBed.inject(OrdemServicoService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  
  it('should get all ordens', () => {
    const mockOrdens = [{ id: 1, titulo: 'Test' }];
    
    service.getAll().subscribe(ordens => {
      expect(ordens).toEqual(mockOrdens);
    });
    
    const req = httpMock.expectOne('http://localhost:5000/api/ordemservico');
    expect(req.request.method).toBe('GET');
    req.flush(mockOrdens);
  });
});
```

**Cobertura:**
- ✅ Backend: >80%
- ✅ Frontend: >70%
- ✅ Confiança: Alta
- ✅ Refatoração: Segura

---

## 5️⃣ Segurança

### Legado: Vulnerável

| Vulnerabilidade | Exemplo | OWASP Top 10 |
|-----------------|---------|--------------|
| **SQL Injection** | `$"SELECT * FROM ... WHERE Id = {id}"` | A03:2021 |
| **Credenciais Hardcoded** | `"Server=localhost;User=sa;Password=123"` | A07:2021 |
| **Sem Validação** | Aceita qualquer input | A04:2021 |
| **CORS Aberto** | `*` | A05:2021 |
| **Logs Sensíveis** | `Console.WriteLine(password)` | A09:2021 |

### Moderno: Seguro

| Proteção | Implementação | OWASP |
|----------|---------------|-------|
| **Queries Parametrizadas** | EF Core LINQ | ✅ A03 |
| **Configuração Externa** | appsettings.json | ✅ A07 |
| **Validação** | FluentValidation | ✅ A04 |
| **CORS Restrito** | `WithOrigins("http://localhost:4200")` | ✅ A05 |
| **Logs Estruturados** | Serilog (sem dados sensíveis) | ✅ A09 |

---

## 6️⃣ Performance

| Métrica | Legado | Moderno | Melhoria |
|---------|--------|---------|----------|
| **Tempo de Resposta (p50)** | ~500ms | ~100ms | 5x mais rápido |
| **Tempo de Resposta (p90)** | ~2s | ~300ms | 6.6x mais rápido |
| **Throughput** | 50 req/s | 500 req/s | 10x mais |
| **Memória** | 200 MB | 50 MB | 4x menos |
| **Startup Time** | 10s | 2s | 5x mais rápido |

---

## 7️⃣ DevOps

### Legado: Manual

```
1. Compilar no Visual Studio
2. Publish para pasta local
3. Copiar arquivos via FTP
4. Configurar IIS manualmente
5. Testar em produção 🤞
```

**Problemas:**
- ❌ Demorado (30-60 min)
- ❌ Propenso a erros
- ❌ Sem rollback
- ❌ Sem testes automatizados

### Moderno: Automatizado

```yaml
# .github/workflows/ci-cd.yml
on: [push]
jobs:
  build-test-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Build
        run: dotnet build
      - name: Test
        run: dotnet test
      - name: Docker Build
        run: docker build -t app .
      - name: Deploy
        run: docker push app
```

**Benefícios:**
- ✅ Rápido (5-10 min)
- ✅ Consistente
- ✅ Rollback automático
- ✅ Quality gates

---

## 8️⃣ Manutenibilidade

### Legado

```
Linhas de Código: ~2.000
Complexidade Ciclomática: Alta (>20)
Duplicação: ~30%
Débitos Técnicos: 8
Tempo para Adicionar Feature: 2-3 dias
```

### Moderno

```
Linhas de Código: ~3.500 (mais estruturado)
Complexidade Ciclomática: Baixa (<10)
Duplicação: <5%
Débitos Técnicos: 0 críticos
Tempo para Adicionar Feature: 4-8 horas
```

---

## 9️⃣ Checklist de Modernização

Use este checklist durante a modernização:

### Backend
- [ ] Migrar .NET Framework 4.8 → .NET 8
- [ ] Implementar Clean Architecture (4 camadas)
- [ ] Adicionar Entity Framework Core
- [ ] Implementar CQRS com MediatR
- [ ] Adicionar Dependency Injection
- [ ] Implementar Repository Pattern
- [ ] Adicionar Serilog (logging estruturado)
- [ ] Adicionar FluentValidation
- [ ] Configurar Swagger
- [ ] Corrigir SQL Injection (parametrização)
- [ ] Externalizar credenciais (appsettings.json)
- [ ] Adicionar testes unitários (>80%)
- [ ] Adicionar paginação

### Frontend
- [ ] Migrar Angular 12 → Angular 18
- [ ] Adicionar Angular Material
- [ ] Implementar Signals
- [ ] Criar Services (separar HTTP)
- [ ] Implementar Reactive Forms
- [ ] Adicionar tratamento de erro
- [ ] Substituir alert() por MatSnackBar
- [ ] Remover location.reload()
- [ ] Adicionar loading states
- [ ] Adicionar testes unitários (>70%)
- [ ] Adicionar testes E2E

### DevOps
- [ ] Criar Dockerfile backend
- [ ] Criar Dockerfile frontend
- [ ] Criar docker-compose.yml
- [ ] Configurar CI/CD (GitHub Actions)
- [ ] Adicionar quality gates
- [ ] Configurar ambientes (dev, staging, prod)

---

## 🎯 Resultado Esperado

Após a modernização, você terá:

✅ **Backend moderno** (.NET 8 + Clean Architecture + CQRS)  
✅ **Frontend moderno** (Angular 18 + Material + Signals)  
✅ **Testes automatizados** (>80% backend, >70% frontend)  
✅ **Segurança** (0 vulnerabilidades críticas)  
✅ **DevOps** (Docker + CI/CD)  
✅ **Documentação** (README, ADRs, Swagger)  
✅ **Performance** (5-10x mais rápido)  
✅ **Manutenibilidade** (código limpo, testável)

---

**Última atualização:** 2025-10-15  
**Versão:** 1.0  
**Autor:** Tech Lead Alest
