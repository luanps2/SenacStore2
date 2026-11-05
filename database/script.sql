-- ===========================================
-- SenacStore: criação do banco, tabelas e dados de exemplo (SQL Server)
--
-- ATENÇÃO: este script APAGA as tabelas existentes e os dados delas.
-- Rode apenas para criar ou recriar o banco do zero.
--
-- Azure SQL: não aceita o comando USE. Crie o banco pelo portal, conecte-se
-- a ele e rode o script a partir da seção "DROP TABLES".
-- ===========================================

-- ===========================================
-- CRIAÇÃO DO BANCO
-- ===========================================
IF DB_ID('SenacStore') IS NULL
    CREATE DATABASE SenacStore;
GO

USE SenacStore;
GO

-- ===========================================
-- DROP TABLES (para recriação)
-- A ordem importa: primeiro as tabelas que têm chave estrangeira.
-- ===========================================
IF OBJECT_ID('Produto', 'U') IS NOT NULL DROP TABLE Produto;
IF OBJECT_ID('Categoria', 'U') IS NOT NULL DROP TABLE Categoria;
IF OBJECT_ID('Usuario', 'U') IS NOT NULL DROP TABLE Usuario;
IF OBJECT_ID('TipoUsuario', 'U') IS NOT NULL DROP TABLE TipoUsuario;
GO

-- ===========================================
-- TABELA TipoUsuario
-- ===========================================
CREATE TABLE TipoUsuario (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_TipoUsuario PRIMARY KEY
        CONSTRAINT DF_TipoUsuario_Id DEFAULT NEWID(),
    Nome NVARCHAR(100) NOT NULL
);
GO

-- ===========================================
-- TABELA Usuario
-- Senha guarda o hash bcrypt, nunca a senha em texto puro.
-- ===========================================
CREATE TABLE Usuario (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Usuario PRIMARY KEY
        CONSTRAINT DF_Usuario_Id DEFAULT NEWID(),
    Nome NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL
        CONSTRAINT UQ_Usuario_Email UNIQUE,
    Senha NVARCHAR(200) NOT NULL,
    TipoUsuarioId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_Usuario_TipoUsuario
        FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario(Id)
);
GO

-- ===========================================
-- TABELA Categoria
-- ===========================================
CREATE TABLE Categoria (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Categoria PRIMARY KEY
        CONSTRAINT DF_Categoria_Id DEFAULT NEWID(),
    Nome NVARCHAR(120) NOT NULL
);
GO

-- ===========================================
-- TABELA Produto
-- ===========================================
CREATE TABLE Produto (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Produto PRIMARY KEY
        CONSTRAINT DF_Produto_Id DEFAULT NEWID(),
    Nome NVARCHAR(150) NOT NULL,
    Preco DECIMAL(10,2) NOT NULL
        CONSTRAINT CK_Produto_Preco CHECK (Preco >= 0),
    CategoriaId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_Produto_Categoria
        FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)
);
GO

-- ===========================================
-- DADOS DE EXEMPLO (estilo marketplace)
-- Os IDs são fixos para os exemplos do docs/API.md funcionarem em qualquer máquina.
-- ===========================================

-- Tipos de usuário
INSERT INTO TipoUsuario (Id, Nome) VALUES
('11111111-1111-1111-1111-111111111111', N'Administrador'),
('22222222-2222-2222-2222-222222222222', N'Vendedor'),
('33333333-3333-3333-3333-333333333333', N'Comprador');
GO

-- Usuários. Senhas para login (somente para desenvolvimento):
--   admin@marketplace.com  adminpass
--   vendedor@loja.com      sellerpass
--   cliente@exemplo.com    buyerpass
INSERT INTO Usuario (Id, Nome, Email, Senha, TipoUsuarioId) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', N'Admin Plataforma', N'admin@marketplace.com', N'$2b$10$SGZKjrnnred./N8lJ1DYCuaNLDKL9wX5HjPRfSvn8l9mhf9YgE/wC', '11111111-1111-1111-1111-111111111111'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', N'Loja Exemplo (Vendedor)', N'vendedor@loja.com', N'$2b$10$pLAODWPsYnCfYGsOAh7TwOJna.OsYASyv5N06Ix3LUDrta8M222zu', '22222222-2222-2222-2222-222222222222'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', N'Cliente Teste', N'cliente@exemplo.com', N'$2b$10$y7lHj7VKZLuG2NYqTjTUoucBZa8kyraJnzlmVknGc6A2xPk89L7T.', '33333333-3333-3333-3333-333333333333');
GO

-- Categorias
INSERT INTO Categoria (Id, Nome) VALUES
('aaaaaaaa-0000-0000-0000-000000000001', N'Celulares e Smartphones'),
('aaaaaaaa-0000-0000-0000-000000000002', N'Informática'),
('aaaaaaaa-0000-0000-0000-000000000003', N'Eletrodomésticos'),
('aaaaaaaa-0000-0000-0000-000000000004', N'Moda'),
('aaaaaaaa-0000-0000-0000-000000000005', N'Casa e Decoração'),
('aaaaaaaa-0000-0000-0000-000000000006', N'Brinquedos');
GO

-- Produtos (cada produto aponta para a categoria correspondente)
INSERT INTO Produto (Id, Nome, Preco, CategoriaId) VALUES
('bbbbbbbb-0000-0000-0000-000000000001', N'Smartphone X Pro 128GB', 2499.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
('bbbbbbbb-0000-0000-0000-000000000002', N'Capa Silicone para Smartphone X', 59.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
('bbbbbbbb-0000-0000-0000-000000000003', N'Notebook Gamer 16GB / RTX 3060', 5999.00, 'aaaaaaaa-0000-0000-0000-000000000002'),
('bbbbbbbb-0000-0000-0000-000000000004', N'SSD NVMe 1TB', 429.90, 'aaaaaaaa-0000-0000-0000-000000000002'),
('bbbbbbbb-0000-0000-0000-000000000005', N'Geladeira Inox 375L', 2899.00, 'aaaaaaaa-0000-0000-0000-000000000003'),
('bbbbbbbb-0000-0000-0000-000000000006', N'Micro-ondas 25L', 499.90, 'aaaaaaaa-0000-0000-0000-000000000003'),
('bbbbbbbb-0000-0000-0000-000000000007', N'Tênis Esportivo Modelo Z', 199.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
('bbbbbbbb-0000-0000-0000-000000000008', N'Vestido Casual Floral', 129.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
('bbbbbbbb-0000-0000-0000-000000000009', N'Conjunto de Cama King 4 peças', 249.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
('bbbbbbbb-0000-0000-0000-00000000000a', N'Conjunto de Panelas Antiaderente 5 peças', 179.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
('bbbbbbbb-0000-0000-0000-00000000000b', N'Carrinho de Controle Remoto', 149.90, 'aaaaaaaa-0000-0000-0000-000000000006'),
('bbbbbbbb-0000-0000-0000-00000000000c', N'Boneca Interativa', 89.90, 'aaaaaaaa-0000-0000-0000-000000000006');
GO

-- RESULTADOS (verificação rápida)
SELECT 'Banco criado com sucesso!' AS Mensagem;
SELECT * FROM TipoUsuario;
SELECT Id, Nome, Email, TipoUsuarioId FROM Usuario;
SELECT * FROM Categoria;
SELECT Id, Nome, Preco, CategoriaId FROM Produto;
GO
