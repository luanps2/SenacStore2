# SenacStore.API

API REST do SenacStore, feita com Node.js, Express 5, Prisma 7 e MySQL. Ela implementa o contrato de [docs/API.md](../docs/API.md).

A API é independente do aplicativo desktop: não usa nenhum projeto C# da solução, e o aplicativo só conhece a URL dela. Esta pasta pode ser publicada sozinha.

## Estrutura

```
index.js             Monta o Express e as rotas
server.js            Confere a configuração e inicia o servidor
prisma/schema.prisma Mapeamento das tabelas do banco
src/config           Conexão com o MySQL
src/routes           Rotas de cada recurso
src/controllers      Validações e respostas HTTP
src/models           Consultas ao banco
src/middlewares      Token, ID da rota e tratamento de erros
src/docs             Documentação Swagger (página /docs)
src/utils            Funções de validação e abertura do navegador
client.http          Requisições de teste
```

Além das rotas do contrato, `GET /` responde que a API está no ar. Use essa rota para conferir a publicação pelo navegador.

## Rodar na máquina local

Requisitos: Node.js 20.19 ou mais novo e um MySQL 8 (instalador oficial, XAMPP/MariaDB ou Docker).

1. Rode `database/script.sql` no MySQL, pelo MySQL Workbench ou pelo terminal:
   ```
   mysql -u root -p < ../database/script.sql
   ```
2. Nesta pasta, instale as dependências:
   ```
   npm install
   ```
3. Copie `.env.example` para `.env` e preencha os valores.
4. Inicie a API:
   ```
   npm run dev
   ```
5. O navegador abre sozinho no **Swagger** (http://localhost:3000/docs), onde todas as rotas podem ser testadas. O terminal também mostra os links, que abrem com Ctrl+clique.
   - Para testar as rotas protegidas: rode `POST /auth/login`, copie o `token`, clique em **Authorize** e cole o token.
   - Para não abrir o navegador, coloque `ABRIR_NAVEGADOR=false` no `.env`. No modo `npm run dev`, ele abre só na primeira vez, e não a cada arquivo salvo.

Para iniciar pelo Visual Studio, clique com o botão direito em `SenacStore.API` e escolha **Depurar > Iniciar Nova Instância**.

## Publicar no Azure

Você vai criar um banco no **Azure Database for MySQL** e publicar a API no **App Service**. Crie os dois na mesma região.

### 1. Banco de dados

1. No portal do Azure, crie um **Servidor flexível do Banco de Dados do Azure para MySQL**. Se a oferta gratuita aparecer, aplique-a.
2. Escolha **Autenticação do MySQL** e anote o servidor, o usuário administrador e a senha.
3. Em **Rede** do servidor, marque **Permitir acesso público de qualquer serviço do Azure** e adicione o IP da sua máquina.
4. Conecte-se ao servidor pelo MySQL Workbench e rode `database/script.sql`. Ele cria o banco `SenacStore` se ainda não existir.

### 2. App Service

1. Crie um **Aplicativo Web** com publicação em **Código**, sistema **Linux** e pilha **Node** na versão LTS mais recente.
2. Em **Configurações > Variáveis de ambiente**, cadastre:

   | Nome | Valor |
   |---|---|
   | `JWT_SECRET` | Texto longo e aleatório (veja como gerar em `.env.example`) |
   | `DATABASE_HOST` | `nome-do-servidor.mysql.database.azure.com` |
   | `DATABASE_PORT` | `3306` |
   | `DATABASE_DB` | Nome do banco |
   | `DATABASE_USER` | Usuário administrador do MySQL |
   | `DATABASE_PASS` | Senha do MySQL, **sem aspas** |
   | `DATABASE_SSL` | `true` |
   | `SCM_DO_BUILD_DURING_DEPLOYMENT` | `true` |

   A última variável faz o Azure rodar o `npm install` durante a publicação. A porta não é cadastrada, porque o Azure informa a dele em `PORT`.

### 3. Publicação

1. Abra **esta pasta** (`SenacStore.API`) no VS Code, e não a raiz da solução. O arquivo `.vscode/settings.json` impede que o `.env` e o `node_modules` sejam enviados.
2. Instale a extensão **Azure App Service** e entre na sua conta.
3. Na aba do Azure, clique com o botão direito no Aplicativo Web e escolha **Deploy to Web App**.
4. Copie o **Domínio padrão** na página **Visão geral** do Aplicativo Web e abra no navegador para ver a mensagem da API.

### 4. Aplicativo desktop

Coloque a URL do Azure em `SenacStore.UI/appsettings.json`:

```json
{
  "Api": {
    "BaseUrl": "https://seu-dominio-padrao.azurewebsites.net/",
    "TimeoutSegundos": 60
  }
}
```

Use `https`. Com `http`, a senha e o token viajariam sem criptografia.

### 5. Segurança

As senhas de exemplo estão no `script.sql`, que é público no repositório. Logo depois de publicar, entre pelo aplicativo e **troque a senha dos três usuários de exemplo**. Enquanto isso não for feito, qualquer pessoa com a URL entra como administrador.

## Problemas comuns

| Sintoma | Causa provável |
|---|---|
| A API para ao iniciar com "Configure estas variáveis" | Falta alguma variável no `.env` ou no App Service |
| `Access denied for user` no log | Usuário ou senha errados. No `.env`, senha com `#` precisa de aspas |
| `pool failed to retrieve a connection` ou `ECONNREFUSED` no log | O serviço do MySQL está parado, a porta está errada ou o firewall do Azure está bloqueando |
| `Unknown database 'SenacStore'` no log | O `database/script.sql` ainda não foi executado |
| Erro de SSL ou `secure transport required` no Azure | Falta `DATABASE_SSL=true` |
| `Erro interno no servidor.` no aplicativo | Veja o detalhe em **Fluxo de logs** no App Service |
