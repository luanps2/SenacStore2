# 🏬 SenacStore  
Aplicação Windows Forms com arquitetura em camadas, CRUD completo e carregamento dinâmico de UserControls.  
Projeto educacional: o aplicativo é um **cliente de uma API REST**. Ele não acessa banco de dados; todos os dados passam por requisições HTTP.

## 📚 Sobre o Projeto
**SenacStore** é um sistema desktop construído para fins educacionais, com o objetivo de demonstrar:
- Arquitetura em 4 camadas
- Consumo de API REST com HttpClient e async/await
- Autenticação com token JWT
- CRUD completo
- Navegação moderna com UserControls
- IoC manual
- Princípios SOLID

## 🚀 Como Executar

Requisitos: .NET 8 SDK (ou Visual Studio 2022) e a API SenacStore em execução, local ou no Azure.

1. **Banco de dados:** rode `database/script.sql` no SQL Server. O script apaga e recria as tabelas.
2. **API:** inicie a API do projeto [SenacStore.API](SenacStore.API/README.md), que implementa o contrato de [docs/API.md](docs/API.md).
3. **URL da API:** confira o arquivo [SenacStore.UI/appsettings.json](SenacStore.UI/appsettings.json).
4. **Aplicativo:** abra `SenacStore.sln` no Visual Studio e execute `SenacStore.UI`, ou use:
   ```
   dotnet run --project SenacStore.UI
   ```

Login de teste: `admin@marketplace.com`, senha `adminpass`.

## ⚙️ Configurando a URL da API

Altere `BaseUrl` em `SenacStore.UI/appsettings.json`:

```json
{
  "Api": {
    "BaseUrl": "http://localhost:3000/",
    "TimeoutSegundos": 30
  }
}
```

| Onde a API está | Exemplo de BaseUrl |
|---|---|
| Na mesma máquina | `http://localhost:3000/` |
| No Azure | `https://seu-dominio-padrao.azurewebsites.net/` |

O arquivo é copiado para a pasta do executável ao compilar. Para trocar a URL sem recompilar, edite a cópia que fica ao lado do `SenacStore.UI.exe` e abra o aplicativo de novo.

Nos planos gratuitos do Azure, a primeira requisição pode demorar enquanto o servidor e o banco acordam. Se aparecer a mensagem de tempo esgotado, aumente `TimeoutSegundos`.

## 🧱 Arquitetura do Projeto
```
SenacStore.Domain          Entidades
SenacStore.Application     Interfaces dos serviços e DTOs (formato do JSON)
SenacStore.Infrastructure  Services: chamadas HTTP à API
SenacStore.UI              Telas Windows Forms
SenacStore.API             API REST em Node.js, independente dos projetos C#
database/script.sql        Criação do banco e dados de exemplo
docs/API.md                Contrato da API
```

## 🗃️ Estrutura da Base de Dados
Relações:
- Usuario → TipoUsuario (N:1)
- Produto → Categoria (N:1)

## 🧭 Navegação
Fluxo usando ICrudNavigator:
frmMenu → ucCrudPadrao(handler) → ucForm → Voltar → Refresh

## 🧩 Padrões Utilizados
- Service Layer
- DTO
- Dependency Injection manual
- UserControls dinâmicos
- Clean Architecture-inspired

## 🖼 Finalidade
Projeto educacional para turmas SENAC.

## 📄 Licença
Uso educacional liberado.
