// Arquivo: SenacStore.Infrastructure.IoC.IoC.cs
// Finalidade: container IoC(Inverse of Control) muito simples (fábricas estáticas) que centraliza
// a criação do cliente HTTP e fornece instâncias dos serviços usados pela UI.

using SenacStore.Infrastructure.Services;

namespace SenacStore.Infrastructure.IoC
{
    // Classe estática usada como ponto único para obter dependências (serviços)
    public static class IoC
    {
        // Cliente HTTP compartilhado por todos os serviços (criado em Configure)
        private static ApiClient _api = null!;

        // Método público chamado na inicialização da aplicação com os dados do appsettings.json
        public static void Configure(string baseUrl, int timeoutSegundos)
        {
            _api = new ApiClient(baseUrl, timeoutSegundos);
        }

        // ---------- Fábricas de serviço ----------
        // Cada método retorna uma nova instância do serviço, usando o mesmo ApiClient
        // (é ele que guarda o token do usuário logado).

        public static IAuthService AuthService()
        {
            return new AuthService(_api);
        }

        public static IUsuarioService UsuarioService()
        {
            return new UsuarioService(_api);
        }

        public static ITipoUsuarioService TipoUsuarioService()
        {
            return new TipoUsuarioService(_api);
        }

        public static IProdutoService ProdutoService()
        {
            return new ProdutoService(_api);
        }

        public static ICategoriaService CategoriaService()
        {
            return new CategoriaService(_api);
        }
    }
}
