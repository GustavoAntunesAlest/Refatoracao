# Correção de Encoding UTF-8 no Banco de Dados

## 🐛 Problema Identificado

Os dados no banco de dados foram inseridos com encoding incorreto, causando:
- "João" → "JoÃ£o"
- "Concluída" → "ConcluÃ­da"
- "lâmpada" → "lÃ¢mpada"

## ✅ Solução

Execute o script `fix-encoding.sql` para corrigir todos os dados existentes.

### Como Executar

#### Opção 1: SQL Server Management Studio (SSMS)
1. Abra o SSMS
2. Conecte ao servidor
3. Abra o arquivo `fix-encoding.sql`
4. Execute (F5)

#### Opção 2: sqlcmd (Linha de Comando)
```bash
sqlcmd -S localhost\SQLEXPRESS -d LegacyProcs -i fix-encoding.sql
```

#### Opção 3: Azure Data Studio
1. Abra o Azure Data Studio
2. Conecte ao servidor
3. Abra o arquivo `fix-encoding.sql`
4. Execute

## 📊 O Que o Script Faz

1. Corrige caracteres especiais em `OrdemServico`:
   - Titulo
   - Descricao
   - Tecnico
   - Status

2. Corrige caracteres especiais em `Tecnico` (se existir):
   - Nome

3. Corrige caracteres especiais em `Cliente` (se existir):
   - RazaoSocial
   - NomeFantasia

## 🔍 Verificação

Após executar o script, você verá:
```
Correção concluída!

Verificando dados corrigidos:
Id  Titulo              Tecnico         Status
1   Trocar lâmpada      João Silva      Pendente
2   Consertar impressora Maria Santos   Em Andamento
3   Limpar filtro...    Pedro Oliveira  Concluída
...
```

## ⚠️ Importante

- Este script é **idempotente** (pode ser executado múltiplas vezes sem problemas)
- Faz backup automático? **NÃO** - faça backup manual se necessário
- Afeta dados existentes? **SIM** - apenas corrige encoding

## 🎯 Quando Executar

Execute este script se:
1. Você vê caracteres estranhos (Ã£, Ã­, Ã§, etc.)
2. Após restaurar um backup antigo
3. Após migrar de outro servidor

## ✅ Prevenção Futura

O backend já está configurado para UTF-8 correto:
```csharp
// Program.cs
options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
```

Novos dados inseridos **NÃO** terão este problema.
