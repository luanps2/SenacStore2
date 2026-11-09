# API SenacStore: contrato

Este documento descreve a API REST que você vai construir em Node.js com MySQL. O aplicativo desktop **SenacStore** já está pronto e faz todas as chamadas descritas aqui. Se a sua API seguir este contrato, o aplicativo funciona sem nenhuma alteração.

## Sumário

1. [Como começar](#1-como-começar)
2. [Regras gerais](#2-regras-gerais)
3. [Autenticação](#3-autenticação)
4. [Tipos de usuário](#4-tipos-de-usuário)
5. [Categorias](#5-categorias)
6. [Produtos](#6-produtos)
7. [Usuários](#7-usuários)
8. [Dicas de implementação](#8-dicas-de-implementação)
9. [Checklist final](#9-checklist-final)

---

## 1. Como começar

1. Crie o banco rodando `database/script.sql` no MySQL 8. Ele cria as tabelas e os dados de exemplo.
2. Crie a API em **http://localhost:3000**. As rotas ficam na raiz, sem prefixo: `/produtos`, e não `/api/produtos`.
3. Abra o aplicativo SenacStore e entre com um dos usuários abaixo.

| E-mail | Senha | Tipo |
|---|---|---|
| admin@marketplace.com | adminpass | Administrador |
| vendedor@loja.com | sellerpass | Vendedor |
| cliente@exemplo.com | buyerpass | Comprador |

No banco, as senhas estão guardadas como hash bcrypt.

---

## 2. Regras gerais

### Formato dos dados

- Requisições e respostas usam JSON, com nomes de campos em **camelCase** (`categoriaId`, não `CategoriaId`).
- **IDs são GUIDs** em texto, como `"aaaaaaaa-0000-0000-0000-000000000001"`, e quem gera é o servidor.
- `preco` é um **número** em JSON: `429.90`, e não `"429.90"`.
- **`senha` nunca aparece em uma resposta.** Nem o texto, nem o hash.

### Erros

Todo erro responde com este formato. O aplicativo mostra o texto de `mensagem` para o usuário, então escreva frases claras em português.

```json
{ "mensagem": "Já existe um usuário com este e-mail." }
```

### Códigos de status

| Código | Quando usar |
|---|---|
| `200 OK` | Consulta ou atualização bem-sucedida |
| `201 Created` | Registro criado |
| `204 No Content` | Registro excluído (resposta sem corpo) |
| `400 Bad Request` | Campo obrigatório ausente ou valor inválido |
| `401 Unauthorized` | Token ausente, inválido ou expirado; login incorreto |
| `403 Forbidden` | Usuário sem permissão para a operação |
| `404 Not Found` | Registro não existe |
| `409 Conflict` | Conflito com outros dados (e-mail repetido, categoria em uso) |
| `500 Internal Server Error` | Erro inesperado no servidor |

### Autenticação nas rotas

Depois do login, o aplicativo envia o token em todas as requisições:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Todas as rotas exigem token, exceto estas três**, porque a tela de cadastro abre antes do login:

| Rota pública | Motivo |
|---|---|
| `POST /auth/login` | É onde o token é gerado |
| `GET /tipos-usuario` | Preenche o combo da tela de cadastro |
| `POST /usuarios` | Cadastro de novo usuário (veja a regra na [seção 7](#7-usuários)) |

Sem token válido nas demais rotas, responda `401`.

### Busca por nome

As listagens aceitam o parâmetro `nome`, que filtra os registros cujo nome **contém** o termo:

```
GET /produtos?nome=smart
```

Sem o parâmetro, a rota devolve todos os registros. Sem resultados, responda `200` com lista vazia `[]`.

---

## 3. Autenticação

### `POST /auth/login`

Rota pública. Confere o e-mail e a senha e devolve um token JWT com os dados do usuário.

**Requisição**
```json
{
  "email": "admin@marketplace.com",
  "senha": "adminpass"
}
```

**Resposta `200`**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "usuario": {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "nome": "Admin Plataforma",
    "email": "admin@marketplace.com",
    "tipoUsuarioId": "11111111-1111-1111-1111-111111111111"
  }
}
```

**Erros**

| Status | Situação | Exemplo de `mensagem` |
|---|---|---|
| `400` | E-mail ou senha ausentes | "Informe e-mail e senha." |
| `401` | E-mail não existe ou senha errada | "E-mail ou senha inválidos." |

Use **a mesma mensagem** para e-mail inexistente e senha errada. Assim a API não revela quais e-mails estão cadastrados.

Validade sugerida para o token: 8 horas.

---

## 4. Tipos de usuário

Rota base: `/tipos-usuario`. Tabela: `TipoUsuario`.

**Objeto**
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "nome": "Administrador"
}
```

| Método e rota | Corpo | Sucesso | Erros |
|---|---|---|---|
| `GET /tipos-usuario` (pública) | | `200` lista de objetos | |
| `GET /tipos-usuario?nome=termo` (pública) | | `200` lista de objetos | |
| `GET /tipos-usuario/:id` | | `200` objeto | `401` `404` |
| `POST /tipos-usuario` | `{ "nome": "Gerente" }` | `201` objeto criado | `400` `401` |
| `PUT /tipos-usuario/:id` | `{ "nome": "Gerente" }` | `200` objeto atualizado | `400` `401` `404` |
| `DELETE /tipos-usuario/:id` | | `204` sem corpo | `401` `404` `409` |

Responda `409` ao excluir um tipo que ainda tem usuários.

---

## 5. Categorias

Rota base: `/categorias`. Tabela: `Categoria`.

**Objeto**
```json
{
  "id": "aaaaaaaa-0000-0000-0000-000000000002",
  "nome": "Informática"
}
```

| Método e rota | Corpo | Sucesso | Erros |
|---|---|---|---|
| `GET /categorias` | | `200` lista de objetos | `401` |
| `GET /categorias?nome=termo` | | `200` lista de objetos | `401` |
| `GET /categorias/:id` | | `200` objeto | `401` `404` |
| `POST /categorias` | `{ "nome": "Esportes" }` | `201` objeto criado | `400` `401` |
| `PUT /categorias/:id` | `{ "nome": "Esportes" }` | `200` objeto atualizado | `400` `401` `404` |
| `DELETE /categorias/:id` | | `204` sem corpo | `401` `404` `409` |

Responda `409` ao excluir uma categoria que ainda tem produtos.

---

## 6. Produtos

Rota base: `/produtos`. Tabela: `Produto`.

**Objeto**
```json
{
  "id": "bbbbbbbb-0000-0000-0000-000000000004",
  "nome": "SSD NVMe 1TB",
  "preco": 429.90,
  "categoriaId": "aaaaaaaa-0000-0000-0000-000000000002"
}
```

**Corpo de `POST` e `PUT`** (o mesmo objeto, sem `id`)
```json
{
  "nome": "SSD NVMe 1TB",
  "preco": 429.90,
  "categoriaId": "aaaaaaaa-0000-0000-0000-000000000002"
}
```

| Método e rota | Sucesso | Erros |
|---|---|---|
| `GET /produtos` | `200` lista de objetos | `401` |
| `GET /produtos?nome=termo` | `200` lista de objetos | `401` |
| `GET /produtos/:id` | `200` objeto | `401` `404` |
| `POST /produtos` | `201` objeto criado | `400` `401` |
| `PUT /produtos/:id` | `200` objeto atualizado | `400` `401` `404` |
| `DELETE /produtos/:id` | `204` sem corpo | `401` `404` |

**Validações (`400`):** `nome` obrigatório, `preco` maior ou igual a zero, e `categoriaId` deve ser de uma categoria existente.

O `PUT` grava todos os campos recebidos. O aplicativo sempre envia o objeto completo.

---

## 7. Usuários

Rota base: `/usuarios`. Tabela: `Usuario`.

**Objeto** (sem senha)
```json
{
  "id": "cccccccc-cccc-cccc-cccc-cccccccccccc",
  "nome": "Cliente Teste",
  "email": "cliente@exemplo.com",
  "tipoUsuarioId": "33333333-3333-3333-3333-333333333333"
}
```

**Corpo de `POST` e `PUT`**
```json
{
  "nome": "Cliente Teste",
  "email": "cliente@exemplo.com",
  "senha": "buyerpass",
  "tipoUsuarioId": "33333333-3333-3333-3333-333333333333"
}
```

| Método e rota | Sucesso | Erros |
|---|---|---|
| `GET /usuarios` | `200` lista de objetos | `401` |
| `GET /usuarios?nome=termo` | `200` lista de objetos | `401` |
| `GET /usuarios/:id` | `200` objeto | `401` `404` |
| `POST /usuarios` (pública) | `201` objeto criado | `400` `403` `409` |
| `PUT /usuarios/:id` | `200` objeto atualizado | `400` `401` `404` `409` |
| `DELETE /usuarios/:id` | `204` sem corpo | `401` `404` |

**Regras**

- **Senha:** grave sempre o hash bcrypt, nunca o texto digitado.
- **`senha` no `POST`:** obrigatória.
- **`senha` no `PUT`:** opcional. Se vier ausente ou vazia, mantenha a senha atual. Se vier preenchida, gere um novo hash.
- **E-mail repetido:** responda `409`.
- **`tipoUsuarioId` inexistente:** responda `400`.

**Regra de segurança do `POST /usuarios`**

Essa rota aceita requisições sem token, porque a tela de cadastro abre antes do login. Por isso, **sem token válido, não é permitido criar um Administrador**. Responda `403`:

```json
{ "mensagem": "Somente um usuário logado pode criar um Administrador." }
```

Sem essa regra, qualquer pessoa criaria uma conta de administrador pela rota pública. O tipo Administrador tem o ID `11111111-1111-1111-1111-111111111111`.

---

## 8. Dicas de implementação

### Prisma com MySQL

Use o `provider = "mysql"` e o pacote `@prisma/adapter-mariadb` (ele atende MySQL e MariaDB). Os IDs são GUIDs guardados em `CHAR(36)`. As colunas do banco começam com letra maiúscula, então use `@map` e `@@map` para que os campos saiam em camelCase no JSON:

```prisma
model Produto {
  id          String    @id @default(uuid()) @map("Id") @db.Char(36)
  nome        String    @map("Nome") @db.VarChar(150)
  preco       Decimal   @map("Preco") @db.Decimal(10, 2)
  categoriaId String    @map("CategoriaId") @db.Char(36)
  categoria   Categoria @relation(fields: [categoriaId], references: [id])

  @@map("Produto")
}
```

Três cuidados com o Prisma:

- **`Decimal` vira texto no JSON.** Converta antes de responder: `{ ...produto, preco: Number(produto.preco) }`.
- **`mode: 'insensitive'` não existe no MySQL.** Use apenas `{ contains: termo }`. A busca já ignora maiúsculas, minúsculas e acentos por causa da collation `utf8mb4_0900_ai_ci` do banco.
- **Trate os códigos de erro do Prisma** para responder o status certo:

| Código do Prisma | Significado | Responda |
|---|---|---|
| `P2002` | Valor único repetido (e-mail) | `409` |
| `P2003` | Violação de chave estrangeira | `409` ao excluir um registro em uso; `400` ao criar ou atualizar com um ID inexistente |
| `P2025` | Registro não encontrado | `404` |

**Atenção ao usar `create(req.body)`:** em `/usuarios`, o corpo traz a senha em texto. Gere o hash antes de gravar e tire a senha do objeto antes de responder.

### Se não usar Prisma

O pacote `mysql2` devolve as colunas com o nome do banco (`Nome`, `CategoriaId`). Use apelidos no `SELECT` para obter camelCase:

```sql
SELECT Id AS id, Nome AS nome, Preco AS preco, CategoriaId AS categoriaId
FROM Produto;
```

### Senha e token

Sugestão de pacotes: `bcryptjs` e `jsonwebtoken`.

```js
const hash = await bcrypt.hash(senha, 10)           // ao cadastrar
const confere = await bcrypt.compare(senha, hash)   // ao fazer login

const token = jwt.sign({ id: usuario.id }, process.env.JWT_SECRET, { expiresIn: '8h' })
```

Guarde `JWT_SECRET` no arquivo `.env` e **não envie esse arquivo para o Git**.

Crie um middleware que lê o cabeçalho `Authorization`, valida o token com `jwt.verify` e responde `401` se ele for inválido. Aplique esse middleware nas rotas protegidas.

### Erros inesperados

No Express 5, uma exceção em um controller `async` chega ao middleware de erro. Registre um middleware no final do `index.js` para que os erros saiam no formato do contrato:

```js
app.use((err, req, res, next) => {
  console.error(err)
  res.status(500).json({ mensagem: 'Erro interno no servidor.' })
})
```

### Registro não encontrado

`findUnique` do Prisma devolve `null` quando não encontra nada. Confira o resultado e responda `404`, em vez de `200` com `null`.

---

## 9. Checklist final

Teste no `client.http` e depois pelo aplicativo:

- [ ] Login com senha certa devolve `token` e `usuario`, sem o campo `senha`.
- [ ] Login com senha errada responde `401`.
- [ ] `GET /produtos` sem token responde `401`.
- [ ] `GET /tipos-usuario` sem token responde `200`.
- [ ] `preco` chega como número, sem aspas.
- [ ] Os nomes dos campos estão em camelCase.
- [ ] `GET /produtos?nome=smart` filtra os resultados.
- [ ] `GET` com ID inexistente responde `404`.
- [ ] `POST` responde `201` com o objeto criado, incluindo o `id`.
- [ ] `DELETE` responde `204`.
- [ ] Excluir uma categoria com produtos responde `409`.
- [ ] Cadastrar um e-mail repetido responde `409`.
- [ ] Cadastrar Administrador sem token responde `403`.
- [ ] `PUT /usuarios/:id` sem `senha` mantém a senha antiga.
