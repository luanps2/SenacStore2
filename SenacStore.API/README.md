# SenacStore.API

API REST do SenacStore, feita com Node.js, Express 5, Prisma 7 e SQL Server. Ela implementa o contrato de [docs/API.md](../docs/API.md).

A API é independente do aplicativo desktop: não usa nenhum projeto C# da solução, e o aplicativo só conhece a URL dela. Esta pasta pode ser publicada sozinha.

## Estrutura

```
index.js             Monta o Express e as rotas
server.js            Confere a configuração e inicia o servidor
prisma/schema.prisma Mapeamento das tabelas do banco
src/config           Conexão com o SQL Server
src/routes           Rotas de cada recurso
src/controllers      Validações e respostas HTTP
src/models           Consultas ao banco
src/middlewares      Token, ID da rota e tratamento de erros
src/utils            Funções de validação
client.http          Requisições de teste
```

Além das rotas do contrato, `GET /` responde que a API está no ar. Use essa rota para conferir a publicação pelo navegador.

## Rodar na máquina local

Requisitos: Node.js 20.19 ou mais novo e um SQL Server com **TCP/IP habilitado** e login por usuário e senha. A LocalDB não serve, porque não aceita conexões TCP.

1. Rode `database/script.sql` no SQL Server.
2. Nesta pasta, instale as dependências:
   ```
   npm install
   ```
3. Copie `.env.example` para `.env` e preencha os valores.
4. Inicie a API:
   ```
   npm run dev
   ```
5. Abra http://localhost:3000 no navegador. A resposta deve ser `{"mensagem":"API SenacStore no ar."}`.

Para iniciar pelo Visual Studio, clique com o botão direito em `SenacStore.API` e escolha **Depurar > Iniciar Nova Instância**.

## Publicar no Azure

Você vai criar um banco no **Azure SQL** e publicar a API no **App Service**. Crie os dois na mesma região.

### 1. Banco de dados

1. No portal do Azure, crie um **Banco de dados SQL**. Se a oferta gratuita aparecer, aplique-a.
2. Escolha **Autenticação SQL** e anote o servidor, o usuário e a senha.
3. Em **Rede** do servidor, marque **Permitir que serviços e recursos do Azure acessem este servidor** e adicione o IP da sua máquina.
4. Conecte-se ao banco pelo SSMS ou pela extensão SQL Server do VS Code e rode `database/script.sql` **a partir da seção `DROP TABLES`**. O Azure SQL não aceita o comando `USE`.

### 2. App Service

1. Crie um **Aplicativo Web** com publicação em **Código**, sistema **Linux** e pilha **Node** na versão LTS mais recente.
2. Em **Configurações > Variáveis de ambiente**, cadastre:

   | Nome | Valor |
   |---|---|
   | `JWT_SECRET` | Texto longo e aleatório (veja como gerar em `.env.example`) |
   | `DATABASE_HOST` | `nome-do-servidor.database.windows.net` |
   | `DATABASE_PORT` | `1433` |
   | `DATABASE_DB` | Nome do banco |
   | `DATABASE_USER` | Usuário do servidor SQL |
   | `DATABASE_PASS` | Senha do servidor SQL, **sem aspas** |
   | `DATABASE_TRUST_CERT` | `false` |
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
| `Login failed for user` no log | Usuário ou senha errados. No `.env`, senha com `#` precisa de aspas |
| `Failed to connect` no log | TCP/IP desligado no SQL Server local, ou firewall do Azure SQL bloqueando |
| A primeira requisição do dia dá erro ou demora | O Azure SQL gratuito pausa após 15 minutos parado e leva cerca de um minuto para voltar. Tente de novo |
| `Erro interno no servidor.` no aplicativo | Veja o detalhe em **Fluxo de logs** no App Service |
