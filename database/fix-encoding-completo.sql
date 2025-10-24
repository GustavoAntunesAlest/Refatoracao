-- =====================================================
-- Script: Correção COMPLETA de Encoding UTF-8
-- Data: 17/10/2025
-- Objetivo: Corrigir TODOS os problemas de encoding
-- =====================================================

USE LegacyProcs;
GO

PRINT '========================================';
PRINT 'Correção COMPLETA de Encoding UTF-8';
PRINT '========================================';
GO

-- =====================================================
-- 1. ORDENS DE SERVIÇO
-- =====================================================
PRINT '';
PRINT '1. Corrigindo OrdemServico...';
GO

-- Trocar fechadura
UPDATE OrdemServico SET Titulo = 'Trocar fechadura' WHERE Id = 1;

-- Substituir tomada
UPDATE OrdemServico SET Titulo = 'Substituir tomada' WHERE Id = 2;

-- Verificar vazamento
UPDATE OrdemServico SET Titulo = 'Verificar vazamento' WHERE Id = 3;

-- Limpar filtro ar condicionado
UPDATE OrdemServico SET Titulo = 'Limpar filtro ar condicionado' WHERE Id = 4;

-- Consertar impressora
UPDATE OrdemServico SET Titulo = 'Consertar impressora' WHERE Id = 5;

-- Trocar lâmpada (com acento)
UPDATE OrdemServico SET Titulo = N'Trocar lâmpada' WHERE Id = 6;

-- Limpar o escritório (com acento)
UPDATE OrdemServico SET Titulo = N'Limpar o escritório' WHERE Id = 7;

-- Verificar se existe ID 8
IF EXISTS (SELECT 1 FROM OrdemServico WHERE Id = 8)
BEGIN
    UPDATE OrdemServico SET Titulo = N'Limpar a cozinha' WHERE Id = 8;
END

PRINT 'OrdemServico: Títulos corrigidos!';
GO

-- =====================================================
-- 2. TÉCNICOS
-- =====================================================
PRINT '';
PRINT '2. Corrigindo Tecnico...';
GO

-- Verificar e corrigir nomes
UPDATE Tecnico SET Nome = 'Pedro Oliveira' WHERE Id = 1;
UPDATE Tecnico SET Nome = 'Maria Santos' WHERE Id = 2;
UPDATE Tecnico SET Nome = N'João Silva' WHERE Id = 3;
UPDATE Tecnico SET Nome = 'Gustavo Antunes' WHERE Id = 4;

-- Corrigir especialidades
UPDATE Tecnico SET Especialidade = N'Elétrica' WHERE Id = 1;
UPDATE Tecnico SET Especialidade = N'Hidráulica' WHERE Id = 2;
UPDATE Tecnico SET Especialidade = N'Ar Condicionado' WHERE Id = 3;
UPDATE Tecnico SET Especialidade = N'Manutenção Geral' WHERE Id = 4;

PRINT 'Tecnico: Nomes e especialidades corrigidos!';
GO

-- =====================================================
-- 3. CLIENTES
-- =====================================================
PRINT '';
PRINT '3. Corrigindo Cliente...';
GO

-- Corrigir razões sociais
UPDATE Cliente SET 
    RazaoSocial = N'Serviços Mega Ltda',
    NomeFantasia = 'Mega'
WHERE Id = 1;

UPDATE Cliente SET 
    RazaoSocial = N'Indústrias XYZ Ltda',
    NomeFantasia = 'XYZ'
WHERE Id = 2;

UPDATE Cliente SET 
    RazaoSocial = 'Comercial ABC S.A.',
    NomeFantasia = 'ABC'
WHERE Id = 3;

UPDATE Cliente SET 
    RazaoSocial = N'Tech Solutions Ltda',
    NomeFantasia = 'Tech Solutions'
WHERE Id = 4;

PRINT 'Cliente: Razões sociais corrigidas!';
GO

-- =====================================================
-- 4. DESCRIÇÕES DAS ORDENS
-- =====================================================
PRINT '';
PRINT '4. Corrigindo descrições...';
GO

UPDATE OrdemServico SET Descricao = N'Trocar fechadura da porta principal' WHERE Id = 1;
UPDATE OrdemServico SET Descricao = N'Substituir tomada queimada na sala' WHERE Id = 2;
UPDATE OrdemServico SET Descricao = N'Verificar vazamento no banheiro' WHERE Id = 3;
UPDATE OrdemServico SET Descricao = N'Limpar filtro do ar condicionado' WHERE Id = 4;
UPDATE OrdemServico SET Descricao = N'Consertar impressora que não está imprimindo' WHERE Id = 5;
UPDATE OrdemServico SET Descricao = N'Trocar lâmpada queimada do corredor' WHERE Id = 6;
UPDATE OrdemServico SET Descricao = N'Limpeza geral do escritório' WHERE Id = 7;

IF EXISTS (SELECT 1 FROM OrdemServico WHERE Id = 8)
BEGIN
    UPDATE OrdemServico SET Descricao = N'Limpeza completa da cozinha' WHERE Id = 8;
END

PRINT 'Descrições corrigidas!';
GO

-- =====================================================
-- 5. VERIFICAÇÃO FINAL
-- =====================================================
PRINT '';
PRINT '========================================';
PRINT 'Verificação Final';
PRINT '========================================';
GO

PRINT '';
PRINT 'Ordens de Serviço:';
SELECT Id, Titulo, Descricao, Status FROM OrdemServico ORDER BY Id;

PRINT '';
PRINT 'Técnicos:';
SELECT Id, Nome, Especialidade, Status FROM Tecnico ORDER BY Id;

PRINT '';
PRINT 'Clientes:';
SELECT Id, RazaoSocial, NomeFantasia, CNPJ FROM Cliente ORDER BY Id;

PRINT '';
PRINT '========================================';
PRINT 'Correção de encoding concluída!';
PRINT '========================================';
GO
