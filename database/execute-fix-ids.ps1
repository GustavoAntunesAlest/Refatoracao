# Script PowerShell para executar fix-ids-order.sql
# Data: 17/10/2025

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Reorganização de IDs - LegacyProcs" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Ler o arquivo SQL
$sqlScript = Get-Content -Path "database\fix-ids-simple.sql" -Raw

# Conectar ao SQL Server
$connectionString = "Server=localhost\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"

try {
    Write-Host "Conectando ao banco de dados..." -ForegroundColor Yellow
    
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $connection.Open()
    
    Write-Host "✅ Conectado com sucesso!" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Executando script de reorganização..." -ForegroundColor Yellow
    
    $command = $connection.CreateCommand()
    $command.CommandText = $sqlScript
    $command.CommandTimeout = 300
    
    $reader = $command.ExecuteReader()
    
    # Ler e exibir resultados
    while ($reader.Read()) {
        for ($i = 0; $i -lt $reader.FieldCount; $i++) {
            Write-Host "$($reader.GetName($i)): $($reader.GetValue($i))"
        }
        Write-Host ""
    }
    
    $reader.Close()
    
    Write-Host "✅ Script executado com sucesso!" -ForegroundColor Green
    Write-Host ""
    
    # Verificar IDs reorganizados
    Write-Host "Verificando IDs reorganizados..." -ForegroundColor Yellow
    Write-Host ""
    
    # OrdemServico
    $command.CommandText = "SELECT Id, Titulo, Status FROM OrdemServico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Ordens de Serviço:" -ForegroundColor Cyan
    Write-Host "ID | Título | Status" -ForegroundColor Gray
    Write-Host "---|--------|-------" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1]) | $($reader[2])"
    }
    
    $reader.Close()
    Write-Host ""
    
    # Tecnico
    $command.CommandText = "SELECT Id, Nome, Status FROM Tecnico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Técnicos:" -ForegroundColor Cyan
    Write-Host "ID | Nome | Status" -ForegroundColor Gray
    Write-Host "---|------|-------" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1]) | $($reader[2])"
    }
    
    $reader.Close()
    Write-Host ""
    
    # Cliente
    $command.CommandText = "SELECT Id, RazaoSocial, CNPJ FROM Cliente ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Clientes:" -ForegroundColor Cyan
    Write-Host "ID | Razão Social | CNPJ" -ForegroundColor Gray
    Write-Host "---|--------------|-----" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1]) | $($reader[2])"
    }
    
    $reader.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "✅ Reorganização concluída com sucesso!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    
} catch {
    Write-Host ""
    Write-Host "❌ Erro ao executar script:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    exit 1
} finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
        Write-Host ""
        Write-Host "Conexão fechada." -ForegroundColor Gray
    }
}
