-- =====================================================
-- Script: Reorganização de IDs (Versão Simplificada)
-- Data: 17/10/2025
-- =====================================================

USE LegacyProcs;

-- =====================================================
-- 1. ORDENS DE SERVIÇO
-- =====================================================
PRINT 'Reorganizando IDs de OrdemServico...';

-- Criar tabela temporária
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

-- Limpar tabela
DELETE FROM OrdemServico;

-- Resetar identity
DBCC CHECKIDENT ('OrdemServico', RESEED, 0);

-- Inserir com nova ordem
SET IDENTITY_INSERT OrdemServico ON;

INSERT INTO OrdemServico (Id, Titulo, Descricao, Tecnico, Status, DataCriacao, DataAtualizacao)
SELECT NovoId, Titulo, Descricao, Tecnico, Status, DataCriacao, DataAtualizacao
FROM #TempOrdemServico
ORDER BY NovoId;

SET IDENTITY_INSERT OrdemServico OFF;

-- Reabilitar constraints
ALTER TABLE OrdemServico CHECK CONSTRAINT ALL;

-- Limpar temp
DROP TABLE #TempOrdemServico;

PRINT 'OrdemServico: IDs reorganizados!';

-- =====================================================
-- 2. TÉCNICOS
-- =====================================================
PRINT 'Reorganizando IDs de Tecnico...';

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

ALTER TABLE Tecnico NOCHECK CONSTRAINT ALL;
DELETE FROM Tecnico;
DBCC CHECKIDENT ('Tecnico', RESEED, 0);

SET IDENTITY_INSERT Tecnico ON;

INSERT INTO Tecnico (Id, Nome, Email, Telefone, Especialidade, Status, DataCadastro)
SELECT NovoId, Nome, Email, Telefone, Especialidade, Status, DataCadastro
FROM #TempTecnico
ORDER BY NovoId;

SET IDENTITY_INSERT Tecnico OFF;

ALTER TABLE Tecnico CHECK CONSTRAINT ALL;
DROP TABLE #TempTecnico;

PRINT 'Tecnico: IDs reorganizados!';

-- =====================================================
-- 3. CLIENTES
-- =====================================================
PRINT 'Reorganizando IDs de Cliente...';

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

ALTER TABLE Cliente NOCHECK CONSTRAINT ALL;
DELETE FROM Cliente;
DBCC CHECKIDENT ('Cliente', RESEED, 0);

SET IDENTITY_INSERT Cliente ON;

INSERT INTO Cliente (Id, RazaoSocial, NomeFantasia, CNPJ, Email, Telefone, Endereco, Cidade, Estado, CEP, DataCadastro)
SELECT NovoId, RazaoSocial, NomeFantasia, CNPJ, Email, Telefone, Endereco, Cidade, Estado, CEP, DataCadastro
FROM #TempCliente
ORDER BY NovoId;

SET IDENTITY_INSERT Cliente OFF;

ALTER TABLE Cliente CHECK CONSTRAINT ALL;
DROP TABLE #TempCliente;

PRINT 'Cliente: IDs reorganizados!';

-- =====================================================
-- VERIFICAÇÃO FINAL
-- =====================================================
PRINT 'Verificação Final:';

SELECT 'OrdemServico' AS Tabela, COUNT(*) AS Total, MIN(Id) AS MenorId, MAX(Id) AS MaiorId FROM OrdemServico
UNION ALL
SELECT 'Tecnico', COUNT(*), MIN(Id), MAX(Id) FROM Tecnico
UNION ALL
SELECT 'Cliente', COUNT(*), MIN(Id), MAX(Id) FROM Cliente;

SELECT Id, Titulo, Status FROM OrdemServico ORDER BY Id;
SELECT Id, Nome, Status FROM Tecnico ORDER BY Id;
SELECT Id, RazaoSocial, CNPJ FROM Cliente ORDER BY Id;

PRINT 'Reorganização concluída!';
