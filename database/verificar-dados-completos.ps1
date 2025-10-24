# Verificar todos os dados
$connectionString = "Server=localhost\SQLEXPRESS;Database=LegacyProcs;Integrated Security=true;TrustServerCertificate=true;"

$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$connection.Open()

Write-Host "Contagem de registros:" -ForegroundColor Cyan
$command = $connection.CreateCommand()

$command.CommandText = "SELECT COUNT(*) FROM OrdemServico"
$count = $command.ExecuteScalar()
Write-Host "OrdemServico: $count registros"

$command.CommandText = "SELECT COUNT(*) FROM Tecnico"
$count = $command.ExecuteScalar()
Write-Host "Tecnico: $count registros"

$command.CommandText = "SELECT COUNT(*) FROM Cliente"
$count = $command.ExecuteScalar()
Write-Host "Cliente: $count registros"

Write-Host ""
Write-Host "Todos os registros de OrdemServico:" -ForegroundColor Cyan
$command.CommandText = "SELECT Id, Titulo, DataCriacao FROM OrdemServico ORDER BY DataCriacao"
$reader = $command.ExecuteReader()

while ($reader.Read()) {
    Write-Host "ID: $($reader[0]) | Título: $($reader[1]) | Data: $($reader[2])"
}
$reader.Close()

$connection.Close()
