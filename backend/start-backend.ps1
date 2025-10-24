# Script para rodar o backend sem Visual Studio
# Usa MSBuild + IIS Express diretamente

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Iniciando Backend - LegacyProcs" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Procurar MSBuild
Write-Host "1. Procurando MSBuild..." -ForegroundColor Yellow

$msbuildPaths = @(
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
)

$msbuild = $null
foreach ($path in $msbuildPaths) {
    if (Test-Path $path) {
        $msbuild = $path
        Write-Host "   OK: MSBuild encontrado em $path" -ForegroundColor Green
        break
    }
}

if (-not $msbuild) {
    Write-Host "   ERRO: MSBuild nao encontrado!" -ForegroundColor Red
    Write-Host "   Instale o Visual Studio 2022 ou Build Tools" -ForegroundColor Yellow
    Write-Host "   URL: https://visualstudio.microsoft.com/downloads/" -ForegroundColor Cyan
    Read-Host "Pressione Enter para sair"
    exit 1
}

Write-Host ""

# Procurar IIS Express
Write-Host "2. Procurando IIS Express..." -ForegroundColor Yellow

$iisExpressPaths = @(
    "C:\Program Files\IIS Express\iisexpress.exe",
    "C:\Program Files (x86)\IIS Express\iisexpress.exe"
)

$iisExpress = $null
foreach ($path in $iisExpressPaths) {
    if (Test-Path $path) {
        $iisExpress = $path
        Write-Host "   OK: IIS Express encontrado" -ForegroundColor Green
        break
    }
}

if (-not $iisExpress) {
    Write-Host "   ERRO: IIS Express nao encontrado!" -ForegroundColor Red
    Write-Host "   Instale o IIS Express" -ForegroundColor Yellow
    Write-Host "   URL: https://www.microsoft.com/web/downloads/platform.aspx" -ForegroundColor Cyan
    Read-Host "Pressione Enter para sair"
    exit 1
}

Write-Host ""

# Compilar o projeto
Write-Host "3. Compilando o projeto..." -ForegroundColor Yellow

$solutionPath = Join-Path $PSScriptRoot "LegacyProcs.sln"

if (-not (Test-Path $solutionPath)) {
    Write-Host "   ERRO: LegacyProcs.sln nao encontrado!" -ForegroundColor Red
    exit 1
}

Write-Host "   Executando MSBuild..." -ForegroundColor Cyan
& $msbuild $solutionPath /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /v:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Host "   ERRO: Falha ao compilar o projeto" -ForegroundColor Red
    Read-Host "Pressione Enter para sair"
    exit 1
}

Write-Host "   OK: Projeto compilado com sucesso!" -ForegroundColor Green
Write-Host ""

# Iniciar IIS Express
Write-Host "4. Iniciando IIS Express..." -ForegroundColor Yellow

$projectPath = Join-Path $PSScriptRoot "LegacyProcs"
$port = 5000

Write-Host "   Porta: $port" -ForegroundColor Cyan
Write-Host "   Caminho: $projectPath" -ForegroundColor Cyan
Write-Host ""

# Iniciar IIS Express
& $iisExpress /path:$projectPath /port:$port

Write-Host ""
Write-Host "Backend encerrado." -ForegroundColor Yellow
