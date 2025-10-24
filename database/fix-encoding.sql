-- =============================================
-- Script de Correção de Encoding UTF-8
-- =============================================
-- Este script corrige dados que foram inseridos com encoding incorreto
-- Problema: "João" aparece como "JoÃ£o", "Concluída" como "ConcluÃ­da"

USE LegacyProcs;
GO

PRINT 'Iniciando correção de encoding...';
GO

-- Corrigir OrdemServico
UPDATE OrdemServico 
SET Tecnico = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    Tecnico,
    'Ã£', 'ã'),
    'Ã§', 'ç'),
    'Ã­', 'í'),
    'Ã³', 'ó'),
    'Ãº', 'ú')
WHERE Tecnico LIKE '%Ã%';

UPDATE OrdemServico 
SET Status = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    Status,
    'Ã£', 'ã'),
    'Ã§', 'ç'),
    'Ã­', 'í'),
    'Ã³', 'ó'),
    'Ãº', 'ú')
WHERE Status LIKE '%Ã%';

UPDATE OrdemServico 
SET Titulo = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    Titulo,
    'Ã£', 'ã'),
    'Ã§', 'ç'),
    'Ã­', 'í'),
    'Ã³', 'ó'),
    'Ãº', 'ú')
WHERE Titulo LIKE '%Ã%';

UPDATE OrdemServico 
SET Descricao = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    Descricao,
    'Ã£', 'ã'),
    'Ã§', 'ç'),
    'Ã­', 'í'),
    'Ã³', 'ó'),
    'Ãº', 'ú')
WHERE Descricao LIKE '%Ã%';

-- Corrigir Tecnico (se existir)
IF OBJECT_ID('dbo.Tecnico', 'U') IS NOT NULL
BEGIN
    UPDATE Tecnico 
    SET Nome = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
        Nome,
        'Ã£', 'ã'),
        'Ã§', 'ç'),
        'Ã­', 'í'),
        'Ã³', 'ó'),
        'Ãº', 'ú')
    WHERE Nome LIKE '%Ã%';
END
GO

-- Corrigir Cliente (se existir)
IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL
BEGIN
    UPDATE Cliente 
    SET RazaoSocial = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
        RazaoSocial,
        'Ã£', 'ã'),
        'Ã§', 'ç'),
        'Ã­', 'í'),
        'Ã³', 'ó'),
        'Ãº', 'ú')
    WHERE RazaoSocial LIKE '%Ã%';
    
    UPDATE Cliente 
    SET NomeFantasia = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
        NomeFantasia,
        'Ã£', 'ã'),
        'Ã§', 'ç'),
        'Ã­', 'í'),
        'Ã³', 'ó'),
        'Ãº', 'ú')
    WHERE NomeFantasia LIKE '%Ã%';
END
GO

-- Verificar resultados
PRINT 'Correção concluída!';
PRINT '';
PRINT 'Verificando dados corrigidos:';
SELECT Id, Titulo, Tecnico, Status FROM OrdemServico;
GO
