-- ===========================================
-- SenacStore: criação do banco, tabelas e dados de exemplo (MySQL 8)
--
-- ATENÇÃO: este script APAGA as tabelas existentes e os dados delas.
-- Rode apenas para criar ou recriar o banco do zero.
--
-- Azure Database for MySQL: crie o banco pelo portal (ou deixe o CREATE DATABASE
-- abaixo criar) e rode o script inteiro.
-- ===========================================

-- ===========================================
-- CRIAÇÃO DO BANCO
-- utf8mb4 guarda qualquer caractere (acentos, emojis).
-- A collation _ai_ci faz a busca ignorar maiúsculas, minúsculas e acentos.
-- ===========================================
CREATE DATABASE IF NOT EXISTS SenacStore
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci;

USE SenacStore;

-- ===========================================
-- DROP TABLES (para recriação)
-- A ordem importa: primeiro as tabelas que têm chave estrangeira.
-- ===========================================
DROP TABLE IF EXISTS Produto;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS TipoUsuario;

-- ===========================================
-- TABELA TipoUsuario
-- Os IDs são GUIDs guardados como texto (CHAR(36)), no formato
-- 11111111-1111-1111-1111-111111111111. A API gera o ID ao inserir.
-- ===========================================
CREATE TABLE TipoUsuario (
    Id CHAR(36) NOT NULL DEFAULT (UUID()),
    Nome VARCHAR(100) NOT NULL,
    CONSTRAINT PK_TipoUsuario PRIMARY KEY (Id)
);

-- ===========================================
-- TABELA Usuario
-- Senha guarda o hash bcrypt, nunca a senha em texto puro.
-- ===========================================
CREATE TABLE Usuario (
    Id CHAR(36) NOT NULL DEFAULT (UUID()),
    Nome VARCHAR(150) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Senha VARCHAR(200) NOT NULL,
    TipoUsuarioId CHAR(36) NOT NULL,
    CONSTRAINT PK_Usuario PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuario_Email UNIQUE (Email),
    CONSTRAINT FK_Usuario_TipoUsuario
        FOREIGN KEY (TipoUsuarioId) REFERENCES TipoUsuario(Id)
);

-- ===========================================
-- TABELA Categoria
-- ===========================================
CREATE TABLE Categoria (
    Id CHAR(36) NOT NULL DEFAULT (UUID()),
    Nome VARCHAR(120) NOT NULL,
    CONSTRAINT PK_Categoria PRIMARY KEY (Id)
);

-- ===========================================
-- TABELA Produto
-- ===========================================
CREATE TABLE Produto (
    Id CHAR(36) NOT NULL DEFAULT (UUID()),
    Nome VARCHAR(150) NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    CategoriaId CHAR(36) NOT NULL,
    CONSTRAINT PK_Produto PRIMARY KEY (Id),
    CONSTRAINT CK_Produto_Preco CHECK (Preco >= 0),
    CONSTRAINT FK_Produto_Categoria
        FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)
);

-- ===========================================
-- DADOS DE EXEMPLO (estilo marketplace)
-- Os IDs são fixos para os exemplos do docs/API.md funcionarem em qualquer máquina.
-- ===========================================

-- Tipos de usuário
INSERT INTO TipoUsuario (Id, Nome) VALUES
('11111111-1111-1111-1111-111111111111', 'Administrador'),
('22222222-2222-2222-2222-222222222222', 'Vendedor'),
('33333333-3333-3333-3333-333333333333', 'Comprador');

-- Usuários. Senhas para login (somente para desenvolvimento):
--   admin@marketplace.com  adminpass
--   vendedor@loja.com      sellerpass
--   cliente@exemplo.com    buyerpass
INSERT INTO Usuario (Id, Nome, Email, Senha, TipoUsuarioId) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Admin Plataforma', 'admin@marketplace.com', '$2b$10$SGZKjrnnred./N8lJ1DYCuaNLDKL9wX5HjPRfSvn8l9mhf9YgE/wC', '11111111-1111-1111-1111-111111111111'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Loja Exemplo (Vendedor)', 'vendedor@loja.com', '$2b$10$pLAODWPsYnCfYGsOAh7TwOJna.OsYASyv5N06Ix3LUDrta8M222zu', '22222222-2222-2222-2222-222222222222'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Cliente Teste', 'cliente@exemplo.com', '$2b$10$y7lHj7VKZLuG2NYqTjTUoucBZa8kyraJnzlmVknGc6A2xPk89L7T.', '33333333-3333-3333-3333-333333333333');

-- Categorias
INSERT INTO Categoria (Id, Nome) VALUES
('aaaaaaaa-0000-0000-0000-000000000001', 'Celulares e Smartphones'),
('aaaaaaaa-0000-0000-0000-000000000002', 'Informática'),
('aaaaaaaa-0000-0000-0000-000000000003', 'Eletrodomésticos'),
('aaaaaaaa-0000-0000-0000-000000000004', 'Moda'),
('aaaaaaaa-0000-0000-0000-000000000005', 'Casa e Decoração'),
('aaaaaaaa-0000-0000-0000-000000000006', 'Brinquedos');

-- Produtos (cada produto aponta para a categoria correspondente)
INSERT INTO Produto (Id, Nome, Preco, CategoriaId) VALUES
('bbbbbbbb-0000-0000-0000-000000000001', 'Smartphone X Pro 128GB', 2499.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
('bbbbbbbb-0000-0000-0000-000000000002', 'Capa Silicone para Smartphone X', 59.90, 'aaaaaaaa-0000-0000-0000-000000000001'),
('bbbbbbbb-0000-0000-0000-000000000003', 'Notebook Gamer 16GB / RTX 3060', 5999.00, 'aaaaaaaa-0000-0000-0000-000000000002'),
('bbbbbbbb-0000-0000-0000-000000000004', 'SSD NVMe 1TB', 429.90, 'aaaaaaaa-0000-0000-0000-000000000002'),
('bbbbbbbb-0000-0000-0000-000000000005', 'Geladeira Inox 375L', 2899.00, 'aaaaaaaa-0000-0000-0000-000000000003'),
('bbbbbbbb-0000-0000-0000-000000000006', 'Micro-ondas 25L', 499.90, 'aaaaaaaa-0000-0000-0000-000000000003'),
('bbbbbbbb-0000-0000-0000-000000000007', 'Tênis Esportivo Modelo Z', 199.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
('bbbbbbbb-0000-0000-0000-000000000008', 'Vestido Casual Floral', 129.90, 'aaaaaaaa-0000-0000-0000-000000000004'),
('bbbbbbbb-0000-0000-0000-000000000009', 'Conjunto de Cama King 4 peças', 249.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
('bbbbbbbb-0000-0000-0000-00000000000a', 'Conjunto de Panelas Antiaderente 5 peças', 179.90, 'aaaaaaaa-0000-0000-0000-000000000005'),
('bbbbbbbb-0000-0000-0000-00000000000b', 'Carrinho de Controle Remoto', 149.90, 'aaaaaaaa-0000-0000-0000-000000000006'),
('bbbbbbbb-0000-0000-0000-00000000000c', 'Boneca Interativa', 89.90, 'aaaaaaaa-0000-0000-0000-000000000006');

-- RESULTADOS (verificação rápida)
SELECT 'Banco criado com sucesso!' AS Mensagem;
SELECT * FROM TipoUsuario;
SELECT Id, Nome, Email, TipoUsuarioId FROM Usuario;
SELECT * FROM Categoria;
SELECT Id, Nome, Preco, CategoriaId FROM Produto;
