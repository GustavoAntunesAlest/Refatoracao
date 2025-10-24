# Script para verificar se IDs estão ordenados por DataCriacao
# Data: 17/10/2025

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Verificação de Ordem dos IDs" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$connectionString = "Server=localhost\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $connectionString
    $connection.Open()
    
    Write-Host "✅ Conectado ao banco de dados" -ForegroundColor Green
    Write-Host ""
    
    # Verificar OrdemServico
    Write-Host "ORDENS DE SERVIÇO (ordenadas por ID):" -ForegroundColor Cyan
    Write-Host "ID | Título | Data Criação | Status" -ForegroundColor Gray
    Write-Host "---|--------|--------------|-------" -ForegroundColor Gray
    
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT Id, Titulo, DataCriacao, Status FROM OrdemServico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    $ordens = @()
    while ($reader.Read()) {
        $ordem = @{
            Id = $reader[0]
            Titulo = $reader[1]
            DataCriacao = $reader[2]
            Status = $reader[3]
        }
        $ordens += $ordem
        Write-Host "$($ordem.Id) | $($ordem.Titulo) | $($ordem.DataCriacao.ToString('dd/MM/yyyy HH:mm')) | $($ordem.Status)"
    }
    $reader.Close()
    
    Write-Host ""
    Write-Host "Verificando se IDs estão em ordem cronológica..." -ForegroundColor Yellow
    
    $ordenado = $true
    for ($i = 0; $i -lt ($ordens.Count - 1); $i++) {
        if ($ordens[$i].DataCriacao -gt $ordens[$i + 1].DataCriacao) {
            Write-Host "❌ ERRO: ID $($ordens[$i].Id) (criado em $($ordens[$i].DataCriacao)) vem antes de ID $($ordens[$i + 1].Id) (criado em $($ordens[$i + 1].DataCriacao))" -ForegroundColor Red
            $ordenado = $false
        }
    }
    
    if ($ordenado) {
        Write-Host "✅ IDs estão corretamente ordenados por data de criação!" -ForegroundColor Green
    } else {
        Write-Host "❌ IDs NÃO estão ordenados por data de criação!" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "----------------------------------------" -ForegroundColor Gray
    Write-Host ""
    
    # Verificar Tecnico
    Write-Host "TÉCNICOS (ordenados por ID):" -ForegroundColor Cyan
    Write-Host "ID | Nome | Data Cadastro | Status" -ForegroundColor Gray
    Write-Host "---|------|---------------|-------" -ForegroundColor Gray
    
    $command.CommandText = "SELECT Id, Nome, DataCadastro, Status FROM Tecnico ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    $tecnicos = @()
    while ($reader.Read()) {
        $tecnico = @{
            Id = $reader[0]
            Nome = $reader[1]
            DataCadastro = $reader[2]
            Status = $reader[3]
        }
        $tecnicos += $tecnico
        Write-Host "$($tecnico.Id) | $($tecnico.Nome) | $($tecnico.DataCadastro.ToString('dd/MM/yyyy HH:mm')) | $($tecnico.Status)"
    }
    $reader.Close()
    
    Write-Host ""
    Write-Host "Verificando se IDs estão em ordem cronológica..." -ForegroundColor Yellow
    
    $ordenado = $true
    for ($i = 0; $i -lt ($tecnicos.Count - 1); $i++) {
        if ($tecnicos[$i].DataCadastro -gt $tecnicos[$i + 1].DataCadastro) {
            Write-Host "❌ ERRO: ID $($tecnicos[$i].Id) vem antes de ID $($tecnicos[$i + 1].Id) mas foi criado depois" -ForegroundColor Red
            $ordenado = $false
        }
    }
    
    if ($ordenado) {
        Write-Host "✅ IDs estão corretamente ordenados por data de cadastro!" -ForegroundColor Green
    } else {
        Write-Host "❌ IDs NÃO estão ordenados por data de cadastro!" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "----------------------------------------" -ForegroundColor Gray
    Write-Host ""
    
    # Verificar Cliente
    Write-Host "CLIENTES (ordenados por ID):" -ForegroundColor Cyan
    Write-Host "ID | Razão Social | Data Cadastro | CNPJ" -ForegroundColor Gray
    Write-Host "---|--------------|---------------|-----" -ForegroundColor Gray
    
    $command.CommandText = "SELECT Id, RazaoSocial, DataCadastro, CNPJ FROM Cliente ORDER BY Id"
    $reader = $command.ExecuteReader()
    
    $clientes = @()
    while ($reader.Read()) {
        $cliente = @{
            Id = $reader[0]
            RazaoSocial = $reader[1]
            DataCadastro = $reader[2]
            CNPJ = $reader[3]
        }
        $clientes += $cliente
        Write-Host "$($cliente.Id) | $($cliente.RazaoSocial) | $($cliente.DataCadastro.ToString('dd/MM/yyyy HH:mm')) | $($cliente.CNPJ)"
    }
    $reader.Close()
    
    Write-Host ""
    Write-Host "Verificando se IDs estão em ordem cronológica..." -ForegroundColor Yellow
    
    $ordenado = $true
    for ($i = 0; $i -lt ($clientes.Count - 1); $i++) {
        if ($clientes[$i].DataCadastro -gt $clientes[$i + 1].DataCadastro) {
            Write-Host "❌ ERRO: ID $($clientes[$i].Id) vem antes de ID $($clientes[$i + 1].Id) mas foi criado depois" -ForegroundColor Red
            $ordenado = $false
        }
    }
    
    if ($ordenado) {
        Write-Host "✅ IDs estão corretamente ordenados por data de cadastro!" -ForegroundColor Green
    } else {
        Write-Host "❌ IDs NÃO estão ordenados por data de cadastro!" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Verificação Concluída" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    
} catch {
    Write-Host ""
    Write-Host "❌ Erro:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
} finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}
