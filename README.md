# 🏬 SenacStore  
Aplicação Windows Forms com arquitetura em camadas, CRUD completo e carregamento dinâmico de UserControls.  
Projeto educacional desenvolvido para ensinar boas práticas de desenvolvimento .NET sem uso de Entity Framework.

## 📚 Sobre o Projeto
**SenacStore** é um sistema desktop construído para fins educacionais, com o objetivo de demonstrar:
- Arquitetura em 4 camadas
- ADO.NET (Microsoft.Data.SqlClient)
- CRUD completo
- Navegação moderna com UserControls
- IoC manual
- Princípios SOLID

## 🧱 Arquitetura do Projeto
```
SenacStore.Domain
SenacStore.Application
SenacStore.Infrastructure
SenacStore.UI
```

## 🗃️ Estrutura da Base de Dados
Relações:
- Usuario → TipoUsuario (N:1)
- Produto → Categoria (N:1)

Banco é criado automaticamente via DatabaseInitializer.

## ⚙️ Navegação
Fluxo usando ICrudNavigator:
frmMenu → ucCrudPadrao(handler) → ucForm → Voltar → Refresh

## 🧩 Padrões Utilizados
- Repository Pattern
- Dependency Injection manual
- UserControls dinâmicos
- Clean Architecture-inspired

## 🚀 Como Executar
1. Clonar o repo
2. dotnet restore
3. Executar SenacStore.UI
4. Banco será criado automaticamente

## 📁 Estrutura Simplificada
```
Domain/
Application/
Infrastructure/
UI/
```

## 🖼 Finalidade
Projeto educacional para turmas SENAC.

## 📄 Licença
Uso educacional liberado.
