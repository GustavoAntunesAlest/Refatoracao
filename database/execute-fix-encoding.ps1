# Script PowerShell para executar correção de encoding
# Data: 17/10/2025

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Correção COMPLETA de Encoding UTF-8" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Ler o arquivo SQL
$sqlScript = Get-Content -Path "database\fix-encoding-completo.sql" -Raw -Encoding UTF8

# Conectar ao SQL Server
$connectionString = "Server=localhost\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"

try {
    Write-Host "Conectando ao banco de dados..." -ForegroundColor Yellow
    
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $connection.Open()
    
    Write-Host "✅ Conectado com sucesso!" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Executando correções de encoding..." -ForegroundColor Yellow
    
    # Dividir por GO e executar cada batch
    $batches = $sqlScript -split '\bGO\b'
    
    foreach ($batch in $batches) {
        $batch = $batch.Trim()
        if ($batch -ne "") {
            $command = $connection.CreateCommand()
            $command.CommandText = $batch
            $command.CommandTimeout = 300
            
            try {
                $command.ExecuteNonQuery() | Out-Null
            } catch {
                # Ignorar erros de PRINT
                if (-not $_.Exception.Message.Contains("PRINT")) {
                    Write-Host "Aviso: $($_.Exception.Message)" -ForegroundColor Yellow
                }
            }
        }
    }
    
    Write-Host "✅ Correções aplicadas com sucesso!" -ForegroundColor Green
    Write-Host ""
    
    # Verificar dados corrigidos
    Write-Host "Verificando dados corrigidos..." -ForegroundColor Yellow
    Write-Host ""
    
    # OrdemServico
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT Id, Titulo FROM OrdemServico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Ordens de Serviço:" -ForegroundColor Cyan
    Write-Host "ID | Título" -ForegroundColor Gray
    Write-Host "---|-------" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1])"
    }
    
    $reader.Close()
    Write-Host ""
    
    # Tecnico
    $command.CommandText = "SELECT Id, Nome, Especialidade FROM Tecnico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Técnicos:" -ForegroundColor Cyan
    Write-Host "ID | Nome | Especialidade" -ForegroundColor Gray
    Write-Host "---|------|---------------" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1]) | $($reader[2])"
    }
    
    $reader.Close()
    Write-Host ""
    
    # Cliente
    $command.CommandText = "SELECT Id, RazaoSocial FROM Cliente ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    Write-Host "Clientes:" -ForegroundColor Cyan
    Write-Host "ID | Razão Social" -ForegroundColor Gray
    Write-Host "---|-------------" -ForegroundColor Gray
    
    while ($reader.Read()) {
        Write-Host "$($reader[0]) | $($reader[1])"
    }
    
    $reader.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "✅ Encoding corrigido com sucesso!" -ForegroundColor Green
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
