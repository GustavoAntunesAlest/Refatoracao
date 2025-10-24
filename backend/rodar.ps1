# Script simplificado para rodar o backend
# Compila e inicia o servidor

Write-Host "Iniciando Backend..." -ForegroundColor Cyan

# MSBuild path
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

if (-not (Test-Path $msbuild)) {
    Write-Host "ERRO: Visual Studio 2022 nao encontrado!" -ForegroundColor Red
    Write-Host "Abra o projeto backend\LegacyProcs.sln no Visual Studio e pressione F5" -ForegroundColor Yellow
    exit 1
}

# Compilar
Write-Host "Compilando projeto..." -ForegroundColor Yellow
& $msbuild "LegacyProcs.sln" /p:Configuration=Debug /v:minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "Projeto compilado com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Para rodar o backend:" -ForegroundColor Cyan
    Write-Host "1. Abra o Visual Studio 2022" -ForegroundColor White
    Write-Host "2. Abra o arquivo: backend\LegacyProcs.sln" -ForegroundColor White
    Write-Host "3. Pressione F5" -ForegroundColor White
    Write-Host ""
    Write-Host "OU instale o IIS Express:" -ForegroundColor Cyan
    Write-Host "https://www.microsoft.com/web/downloads/platform.aspx" -ForegroundColor White
} else {
    Write-Host "Erro ao compilar!" -ForegroundColor Red
}
