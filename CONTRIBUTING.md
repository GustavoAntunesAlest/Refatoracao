# 🤝 Guia de Contribuição - LegacyProcs

Obrigado por considerar contribuir com o projeto LegacyProcs! Este documento fornece diretrizes para contribuições.

---

## 📋 Índice

1. [Código de Conduta](#código-de-conduta)
2. [Como Contribuir](#como-contribuir)
3. [Padrões de Código](#padrões-de-código)
4. [Processo de Pull Request](#processo-de-pull-request)
5. [Conventional Commits](#conventional-commits)
6. [Testes](#testes)
7. [Documentação](#documentação)

---

## 📜 Código de Conduta

Este projeto segue as **Global Rules** da Alest. Todos os contribuidores devem:

- ✅ Ser respeitoso e profissional
- ✅ Aceitar feedback construtivo
- ✅ Focar no melhor para o projeto
- ✅ Demonstrar empatia com outros contribuidores

---

## 🚀 Como Contribuir

### 1. Fork e Clone

```bash
# Fork no GitHub
# Depois clone seu fork
git clone https://github.com/SEU-USUARIO/Refatoracao.git
cd Refatoracao
```

### 2. Crie uma Branch

```bash
# Padrão: tipo/descricao-curta
git checkout -b feat/nova-funcionalidade
git checkout -b fix/correcao-bug
git checkout -b docs/atualizar-readme
```

### 3. Faça suas Alterações

- Siga os [Padrões de Código](#padrões-de-código)
- Adicione testes para novas funcionalidades
- Atualize a documentação se necessário

### 4. Commit

```bash
# Use Conventional Commits
git add .
git commit -m "feat: adiciona validação de email"
```

### 5. Push e Pull Request

```bash
git push origin feat/nova-funcionalidade
# Abra Pull Request no GitHub
```

---

## 💻 Padrões de Código

### Backend (.NET 8)

#### Estrutura
```
LegacyProcs/
├── Application/      # CQRS (Commands, Queries, Handlers)
├── Controllers/      # API REST
├── Data/            # DbContext
├── Models/          # Entities, DTOs
└── Repositories/    # Data Access
```

#### Convenções
- ✅ **Async/await** para operações I/O
- ✅ **Repository Pattern** para acesso a dados
- ✅ **CQRS** com MediatR
- ✅ **DTOs** para transferência de dados
- ✅ **FluentValidation** para validações
- ✅ **Serilog** para logging

#### Exemplo
```csharp
// ✅ BOM
public async Task<OrdemServico> GetByIdAsync(int id)
{
    _logger.LogInformation("Buscando ordem {Id}", id);
    return await _repository.GetByIdAsync(id);
}

// ❌ RUIM
public OrdemServico GetById(int id)
{
    Console.WriteLine("Buscando ordem " + id);
    return _repository.GetById(id);
}
```

### Frontend (Angular 18)

#### Estrutura
```
src/app/
├── cliente/          # Componente Clientes
├── ordem-servico/    # Componente Ordens
├── tecnico/          # Componente Técnicos
└── services/         # HTTP Services
```

#### Convenções
- ✅ **Signals** para reatividade
- ✅ **Services** para lógica de negócio
- ✅ **MatSnackBar** para feedback (não alert!)
- ✅ **Interfaces** para tipagem
- ✅ **RxJS** para operações assíncronas

#### Exemplo
```typescript
// ✅ BOM
salvar(): void {
  this.ordemServicoService.criar(this.ordem).subscribe({
    next: () => {
      this.snackBar.open('Ordem criada com sucesso!', 'OK', { duration: 3000 });
      this.carregarOrdens(); // Sem reload!
    },
    error: (err) => {
      this.snackBar.open('Erro ao criar ordem', 'OK', { duration: 3000 });
    }
  });
}

// ❌ RUIM
salvar(): void {
  this.http.post('http://localhost:5000/api/ordemservico', this.ordem).subscribe({
    next: () => {
      alert('Ordem criada com sucesso!');
      location.reload();
    }
  });
}
```

---

## 🔄 Processo de Pull Request

### Checklist Antes de Abrir PR

- [ ] Código segue os padrões do projeto
- [ ] Testes adicionados/atualizados
- [ ] Todos os testes passando
- [ ] Documentação atualizada
- [ ] Commits seguem Conventional Commits
- [ ] Branch atualizada com main

### Template de PR

```markdown
## Descrição
Breve descrição das mudanças

## Tipo de Mudança
- [ ] Bug fix (correção)
- [ ] Nova funcionalidade
- [ ] Breaking change
- [ ] Documentação

## Como Testar
1. Passo 1
2. Passo 2
3. Resultado esperado

## Checklist
- [ ] Testes passando
- [ ] Documentação atualizada
- [ ] Conventional Commits
```

### Revisão de Código

Todos os PRs passam por:
1. ✅ Revisão de código (code review)
2. ✅ Testes automatizados (CI)
3. ✅ Aprovação de pelo menos 1 revisor

---

## 📝 Conventional Commits

### Formato

```
<tipo>(<escopo>): <descrição>

[corpo opcional]

[rodapé opcional]
```

### Tipos

| Tipo | Descrição | Exemplo |
|------|-----------|---------|
| `feat` | Nova funcionalidade | `feat: adiciona validação de CPF` |
| `fix` | Correção de bug | `fix: corrige SQL injection` |
| `docs` | Documentação | `docs: atualiza README` |
| `style` | Formatação | `style: formata código` |
| `refactor` | Refatoração | `refactor: extrai método` |
| `test` | Testes | `test: adiciona teste unitário` |
| `chore` | Manutenção | `chore: atualiza dependências` |

### Exemplos

```bash
# Nova funcionalidade
git commit -m "feat(backend): adiciona endpoint de relatórios"

# Correção de bug
git commit -m "fix(frontend): corrige validação de CNPJ"

# Documentação
git commit -m "docs: adiciona guia de contribuição"

# Breaking change
git commit -m "feat(api)!: altera estrutura de resposta da API

BREAKING CHANGE: campo 'data' renomeado para 'timestamp'"
```

---

## 🧪 Testes

### Backend

```bash
# Executar todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true

# Teste específico
dotnet test --filter "FullyQualifiedName~OrdemServicoTests"
```

#### Padrões
- ✅ xUnit para testes
- ✅ FluentAssertions para assertions
- ✅ Moq para mocks
- ✅ Cobertura mínima: 80%

#### Exemplo
```csharp
[Fact]
public async Task GetByIdAsync_DeveRetornarOrdem_QuandoExistir()
{
    // Arrange
    var id = 1;
    var ordemEsperada = new OrdemServico { Id = id };
    _mockRepository.Setup(r => r.GetByIdAsync(id))
        .ReturnsAsync(ordemEsperada);

    // Act
    var resultado = await _handler.Handle(new GetOrdemQuery(id));

    // Assert
    resultado.Should().NotBeNull();
    resultado.Id.Should().Be(id);
}
```

### Frontend

```bash
# Executar testes
npm test

# Com cobertura
npm run test:coverage

# Teste específico
npm test -- --include='**/ordem-servico.component.spec.ts'
```

#### Padrões
- ✅ Jasmine/Karma para testes
- ✅ Cobertura mínima: 70%

---

## 📚 Documentação

### O Que Documentar

1. **Código**
   - Métodos públicos complexos
   - Classes e interfaces
   - Decisões não óbvias

2. **ADRs (Architecture Decision Records)**
   - Decisões arquiteturais importantes
   - Template em `docs/ADRs/`

3. **README**
   - Atualizar se mudar setup
   - Adicionar novas funcionalidades

### Exemplo de Documentação

```csharp
/// <summary>
/// Valida um CNPJ usando o algoritmo da Receita Federal.
/// </summary>
/// <param name="cnpj">CNPJ a ser validado (com ou sem formatação)</param>
/// <returns>true se válido, false caso contrário</returns>
/// <remarks>
/// Implementa verificação de dígitos verificadores conforme
/// especificação da Receita Federal do Brasil.
/// </remarks>
public bool ValidarCNPJ(string cnpj)
{
    // Implementação...
}
```

---

## 🔒 Segurança

### Nunca Commitar

- ❌ Senhas ou credenciais
- ❌ Tokens de API
- ❌ Chaves privadas
- ❌ Dados sensíveis

### Sempre Usar

- ✅ Variáveis de ambiente
- ✅ .env.example (sem valores reais)
- ✅ appsettings.json (sem credenciais)
- ✅ Secrets management

---

## 🐛 Reportar Bugs

### Template de Issue

```markdown
## Descrição do Bug
Descrição clara do problema

## Passos para Reproduzir
1. Passo 1
2. Passo 2
3. Erro ocorre

## Comportamento Esperado
O que deveria acontecer

## Comportamento Atual
O que está acontecendo

## Ambiente
- OS: Windows/Linux/Mac
- .NET: 8.0
- Angular: 18.x
- Browser: Chrome 120

## Screenshots
Se aplicável
```

---

## 💡 Sugerir Melhorias

### Template de Feature Request

```markdown
## Problema
Qual problema esta feature resolve?

## Solução Proposta
Como você resolveria?

## Alternativas Consideradas
Outras abordagens pensadas

## Contexto Adicional
Qualquer informação relevante
```

---

## 📞 Contato

- **Issues:** https://github.com/GustavoAntunesAlest/Refatoracao/issues
- **Discussões:** https://github.com/GustavoAntunesAlest/Refatoracao/discussions
- **Email:** gustavo.antunes@alest.com.br

---

## 🎓 Recursos

### Documentação do Projeto
- [README.md](./README.md)
- [DEPLOY.md](./DEPLOY.md)
- [CHANGELOG.md](./CHANGELOG.md)
- [docs/](./docs/)

### Referências Externas
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Angular 18 Docs](https://angular.dev)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## ✅ Checklist Final

Antes de submeter seu PR:

- [ ] Código testado localmente
- [ ] Testes passando (backend e frontend)
- [ ] Documentação atualizada
- [ ] Conventional Commits
- [ ] Branch atualizada com main
- [ ] PR description completa
- [ ] Sem credenciais ou dados sensíveis

---

**Obrigado por contribuir! 🚀**

---

**Última Atualização:** 24/10/2025  
**Versão:** 1.0.0
