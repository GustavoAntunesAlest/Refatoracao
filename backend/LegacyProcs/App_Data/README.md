# Configurações da Aplicação

## 📁 Estrutura de Arquivos

```
App_Data/
├── appsettings.json                      # Configurações padrão (versionado)
├── appsettings.Development.json          # Configurações de desenvolvimento (versionado)
├── appsettings.Production.example.json   # Exemplo para produção (versionado)
└── appsettings.Production.json           # Configurações de produção (NÃO versionado)
```

## 🔒 Segurança

### Arquivos Versionados (Git)
- ✅ `appsettings.json` - Configurações base sem credenciais sensíveis
- ✅ `appsettings.Development.json` - Usa Integrated Security (sem senha)
- ✅ `appsettings.Production.example.json` - Template para produção

### Arquivos NÃO Versionados (Git)
- ❌ `appsettings.Production.json` - Contém credenciais reais
- ❌ Qualquer arquivo com credenciais sensíveis

## 🚀 Como Usar

### Desenvolvimento Local

1. Use `appsettings.Development.json` (já configurado)
2. Connection string usa **Integrated Security** (Windows Authentication)
3. Nenhuma senha necessária

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

### Produção

1. Copie `appsettings.Production.example.json` para `appsettings.Production.json`
2. Edite `appsettings.Production.json` com credenciais reais
3. **NUNCA** faça commit deste arquivo

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=LegacyProcs;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=true;"
  },
  "AppSettings": {
    "Environment": "Production",
    "EnableDetailedErrors": false
  }
}
```

## 🔧 Variável de Ambiente

Para alternar entre ambientes, defina a variável:

```powershell
# Windows
$env:ASPNET_ENVIRONMENT = "Production"

# Ou no IIS, configure em Application Settings
```

## 📝 Migração do Web.config

### Antes (❌ Inseguro)
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=localhost\SQLEXPRESS;Database=LegacyProcs;User Id=sa;Password=Admin123!;" />
</connectionStrings>
```

### Depois (✅ Seguro)
```csharp
// No controller
private readonly string connString;

public OrdemServicoController()
{
    connString = ConfigurationHelper.GetConnectionString("DefaultConnection");
}
```

## ✅ Checklist de Segurança

- [x] Credenciais removidas do código-fonte
- [x] Credenciais removidas do Web.config
- [x] appsettings.Production.json no .gitignore
- [x] Integrated Security para desenvolvimento
- [x] Template de exemplo criado
- [x] Documentação atualizada

## 🔄 Fallback

O `ConfigurationHelper` tem fallback automático:

1. Tenta ler de `appsettings.{Environment}.json`
2. Se não existir, tenta `appsettings.json`
3. Se não existir, tenta `Web.config` (compatibilidade)
4. Se nenhum existir, lança exceção

Isso garante compatibilidade durante a transição.

## 📚 Referências

- [ASP.NET Configuration Best Practices](https://learn.microsoft.com/aspnet/core/security/app-secrets)
- [OWASP - Secure Configuration](https://owasp.org/www-project-top-ten/)
- [12-Factor App - Config](https://12factor.net/config)
