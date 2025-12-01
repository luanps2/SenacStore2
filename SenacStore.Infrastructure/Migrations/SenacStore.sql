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
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL
);
GO

-- ===========================================
-- TABELA Usuario
-- ===========================================
CREATE TABLE Usuario (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Nome VARCHAR(150) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Senha VARCHAR(200) NOT NULL,
    TipoUsuarioId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario(Id)
);
GO

-- ===========================================
-- TABELA Categoria
-- ===========================================
CREATE TABLE Categoria (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Nome VARCHAR(120) NOT NULL
);
GO

-- ===========================================
-- TABELA Produto
-- ===========================================
CREATE TABLE Produto (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Nome VARCHAR(150) NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    CategoriaId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)
);
GO

-- ===========================================
-- SEED (estilo marketplace)
-- - Roles: Administrador, Vendedor, Comprador
-- - Users: admin, seller, buyer
-- - Categories: categorias típicas de marketplace
-- - Produtos: exemplos por categoria
-- Observação: IDs são explícitos (GUIDs) para reprodutibilidade
-- ===========================================

-- Tipos de usuário
INSERT INTO TipoUsuario (Id, Nome) VALUES
('11111111-1111-1111-1111-111111111111', 'Administrador'),
('22222222-2222-2222-2222-222222222222', 'Vendedor'),
('33333333-3333-3333-3333-333333333333', 'Comprador');
GO

-- Usuários (senhas em texto plano apenas para ambiente de desenvolvimento)
INSERT INTO Usuario (Id, Nome, Email, Senha, TipoUsuarioId) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Admin Plataforma', 'admin@marketplace.com', 'adminpass', '11111111-1111-1111-1111-111111111111'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Loja Exemplo (Vendedor)', 'vendedor@loja.com', 'sellerpass', '22222222-2222-2222-2222-222222222222'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Cliente Teste', 'cliente@exemplo.com', 'buyerpass', '33333333-3333-3333-3333-333333333333');
GO

-- Categorias típicas de marketplace
INSERT INTO Categoria (Id, Nome) VALUES
('aaaaaaaa-0000-0000-0000-000000000001', 'Celulares e Smartphones'),
('aaaaaaaa-0000-0000-0000-000000000002', 'Informática'),
('aaaaaaaa-0000-0000-0000-000000000003', 'Eletrodomésticos'),
('aaaaaaaa-0000-0000-0000-000000000004', 'Moda'),
('aaaaaaaa-0000-0000-0000-000000000005', 'Casa e Decoração'),
('aaaaaaaa-0000-0000-0000-000000000006', 'Brinquedos');
GO

-- Produtos de exemplo (cada produto aponta para a categoria correspondente)
INSERT INTO Produto (Id, Nome, Preco, CategoriaId) VALUES
(NEWID(), 'Smartphone X Pro 128GB', 2499.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
(NEWID(), 'Capa Silicone para Smartphone X', 59.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
(NEWID(), 'Notebook Gamer 16GB / RTX 3060', 5999.00, 'aaaaaaaa-0000-0000-0000-000000000002'),
(NEWID(), 'SSD NVMe 1TB', 429.90, 'aaaaaaaa-0000-0000-0000-000000000002'),
(NEWID(), 'Geladeira Inox 375L', 2899.00, 'aaaaaaaa-0000-0000-0000-000000000003'),
(NEWID(), 'Micro-ondas 25L', 499.90, 'aaaaaaaa-0000-0000-0000-000000000003'),
(NEWID(), 'Tênis Esportivo Modelo Z', 199.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
(NEWID(), 'Vestido Casual Floral', 129.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
(NEWID(), 'Conjunto de Cama King 4 peças', 249.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
(NEWID(), 'Conjunto de Panelas Antiaderente 5 peças', 179.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
(NEWID(), 'Carrinho de Controle Remoto', 149.90, 'aaaaaaaa-0000-0000-0000-000000000006'),
(NEWID(), 'Boneca Interativa', 89.90, 'aaaaaaaa-0000-0000-0000-000000000006');
GO

-- RESULTADOS (verificação rápida)
SELECT 'Banco criado com sucesso (seed marketplace)!' AS Mensagem;
SELECT * FROM TipoUsuario;
SELECT * FROM Usuario;
SELECT * FROM Categoria;
SELECT * FROM Produto;SELECT * FROM Produto;