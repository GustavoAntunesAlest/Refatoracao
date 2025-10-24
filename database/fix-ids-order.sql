-- =====================================================
-- Script: Reorganização de IDs em Ordem Crescente
-- Data: 17/10/2025
-- Objetivo: Ordenar IDs de forma sequencial e organizada
-- =====================================================

USE LegacyProcs;
GO

PRINT '========================================';
PRINT 'Iniciando reorganização de IDs';
PRINT '========================================';
GO

-- =====================================================
-- 1. ORDENS DE SERVIÇO
-- =====================================================
PRINT '';
PRINT '1. Reorganizando IDs de OrdemServico...';

-- Criar tabela temporária com nova ordem
SELECT 
    ROW_NUMBER() OVER (ORDER BY DataCriacao ASC, Id ASC) AS NovoId,
    Id AS IdAntigo,
    Titulo,
    Descricao,
    Tecnico,
    Status,
    DataCriacao,
    DataAtualizacao
INTO #TempOrdemServico
FROM OrdemServico;

-- Desabilitar constraints
ALTER TABLE OrdemServico NOCHECK CONSTRAINT ALL;

-- Limpar tabela original
DELETE FROM OrdemServico;

-- Resetar identity
DBCC CHECKIDENT ('OrdemServico', RESEED, 0);

-- Inserir dados com nova ordem
SET IDENTITY_INSERT OrdemServico ON;

INSERT INTO OrdemServico (Id, Titulo, Descricao, Tecnico, Status, DataCriacao, DataAtualizacao)
SELECT 
    NovoId,
    Titulo,
    Descricao,
    Tecnico,
    Status,
    DataCriacao,
    DataAtualizacao
FROM #TempOrdemServico
ORDER BY NovoId;

SET IDENTITY_INSERT OrdemServico OFF;

-- Reabilitar constraints
ALTER TABLE OrdemServico CHECK CONSTRAINT ALL;

-- Limpar tabela temporária
DROP TABLE #TempOrdemServico;

PRINT 'OrdemServico: IDs reorganizados com sucesso!';
PRINT 'Total de registros: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =====================================================
-- 2. TÉCNICOS
-- =====================================================
PRINT '';
PRINT '2. Reorganizando IDs de Tecnico...';

-- Criar tabela temporária com nova ordem
SELECT 
    ROW_NUMBER() OVER (ORDER BY DataCadastro ASC, Id ASC) AS NovoId,
    Id AS IdAntigo,
    Nome,
    Email,
    Telefone,
    Especialidade,
    Status,
    DataCadastro
INTO #TempTecnico
FROM Tecnico;

-- Desabilitar constraints
ALTER TABLE Tecnico NOCHECK CONSTRAINT ALL;

-- Limpar tabela original
DELETE FROM Tecnico;

-- Resetar identity
DBCC CHECKIDENT ('Tecnico', RESEED, 0);

-- Inserir dados com nova ordem
SET IDENTITY_INSERT Tecnico ON;

INSERT INTO Tecnico (Id, Nome, Email, Telefone, Especialidade, Status, DataCadastro)
SELECT 
    NovoId,
    Nome,
    Email,
    Telefone,
    Especialidade,
    Status,
    DataCadastro
FROM #TempTecnico
ORDER BY NovoId;

SET IDENTITY_INSERT Tecnico OFF;

-- Reabilitar constraints
ALTER TABLE Tecnico CHECK CONSTRAINT ALL;

-- Limpar tabela temporária
DROP TABLE #TempTecnico;

PRINT 'Tecnico: IDs reorganizados com sucesso!';
PRINT 'Total de registros: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =====================================================
-- 3. CLIENTES
-- =====================================================
PRINT '';
PRINT '3. Reorganizando IDs de Cliente...';

-- Criar tabela temporária com nova ordem
SELECT 
    ROW_NUMBER() OVER (ORDER BY DataCadastro ASC, Id ASC) AS NovoId,
    Id AS IdAntigo,
    RazaoSocial,
    NomeFantasia,
    CNPJ,
    Email,
    Telefone,
    Endereco,
    Cidade,
    Estado,
    CEP,
    DataCadastro
INTO #TempCliente
FROM Cliente;

-- Desabilitar constraints
ALTER TABLE Cliente NOCHECK CONSTRAINT ALL;

-- Limpar tabela original
DELETE FROM Cliente;

-- Resetar identity
DBCC CHECKIDENT ('Cliente', RESEED, 0);

-- Inserir dados com nova ordem
SET IDENTITY_INSERT Cliente ON;

INSERT INTO Cliente (Id, RazaoSocial, NomeFantasia, CNPJ, Email, Telefone, Endereco, Cidade, Estado, CEP, DataCadastro)
SELECT 
    NovoId,
    RazaoSocial,
    NomeFantasia,
    CNPJ,
    Email,
    Telefone,
    Endereco,
    Cidade,
    Estado,
    CEP,
    DataCadastro
FROM #TempCliente
ORDER BY NovoId;

SET IDENTITY_INSERT Cliente OFF;

-- Reabilitar constraints
ALTER TABLE Cliente CHECK CONSTRAINT ALL;

-- Limpar tabela temporária
DROP TABLE #TempCliente;

PRINT 'Cliente: IDs reorganizados com sucesso!';
PRINT 'Total de registros: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =====================================================
-- 4. VERIFICAÇÃO FINAL
-- =====================================================
PRINT '';
PRINT '========================================';
PRINT 'Verificação Final';
PRINT '========================================';

PRINT '';
PRINT 'OrdemServico:';
SELECT 
    COUNT(*) AS TotalRegistros,
    MIN(Id) AS MenorId,
    MAX(Id) AS MaiorId
FROM OrdemServico;

PRINT '';
PRINT 'Tecnico:';
SELECT 
    COUNT(*) AS TotalRegistros,
    MIN(Id) AS MenorId,
    MAX(Id) AS MaiorId
FROM Tecnico;

PRINT '';
PRINT 'Cliente:';
SELECT 
    COUNT(*) AS TotalRegistros,
    MIN(Id) AS MenorId,
    MAX(Id) AS MaiorId
FROM Cliente;

PRINT '';
PRINT '========================================';
PRINT 'Reorganização concluída com sucesso!';
PRINT '========================================';
GO

-- =====================================================
-- 5. EXIBIR DADOS REORGANIZADOS
-- =====================================================
PRINT '';
PRINT 'Ordens de Serviço (ordenadas por ID):';
SELECT Id, Titulo, Tecnico, Status, DataCriacao
FROM OrdemServico
ORDER BY Id ASC;

PRINT '';
PRINT 'Técnicos (ordenados por ID):';
SELECT Id, Nome, Especialidade, Status, DataCadastro
FROM Tecnico
ORDER BY Id ASC;

PRINT '';
PRINT 'Clientes (ordenados por ID):';
SELECT Id, RazaoSocial, CNPJ, DataCadastro
FROM Cliente
ORDER BY Id ASC;

PRINT '';
PRINT '✅ Script executado com sucesso!';
GO
