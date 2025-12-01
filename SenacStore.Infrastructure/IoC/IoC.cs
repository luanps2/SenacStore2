namespace SenacStore.Infrastructure.IoC
{
    public static class IoC
    {
        private static string _connectionString;

        public static void Configure(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static Conexao CriarConexao()
        {
            return new Conexao(_connectionString);
        }

        // Repositórios

        public static IUsuarioRepository UsuarioRepository()
        {
            return new UsuarioRepository(CriarConexao());
        }

        public static ITipoUsuarioRepository TipoUsuarioRepository()
        {
            return new TipoUsuarioRepository(CriarConexao());
        }

        public static IProdutoRepository ProdutoRepository()
        {
            return new ProdutoRepository(CriarConexao());
        }

        public static ICategoriaRepository CategoriaRepository()
        {
            return new CategoriaRepository(CriarConexao());
        }
    }
}
