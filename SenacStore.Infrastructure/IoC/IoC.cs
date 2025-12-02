namespace SenacStore.Infrastructure.IoC
{
    // Classe estática usada como ponto único para obter dependências (repositórios)
    public static class IoC
    {
        // Campo privado que guarda a connection string configurada pela aplicação (setada em startup)
        private static string _connectionString;

        // Método público chamado na inicialização da aplicação para fornecer a connection string
        public static void Configure(string connectionString)
        {
            // Armazena a connection string para uso posterior ao criar conexões
            _connectionString = connectionString;
        }

        // Função auxiliar privada que cria uma nova instância de Conexao usando a connection string configurada
        private static Conexao CriarConexao()
        {
            // Retorna uma nova conexão; cada repositório receberá sua própria instância
            return new Conexao(_connectionString);
        }

        // ---------- Fábricas de repositório ----------
        // Cada método retorna uma nova instância do repositório correspondente
        // e injeta uma nova Conexao criada acima.

        // Retorna uma instância de IUsuarioRepository pronta para uso
        public static IUsuarioRepository UsuarioRepository()
        {
            // Cria e retorna o repositório de usuários com a conexão configurada
            return new UsuarioRepository(CriarConexao());
        }

        // Retorna uma instância de ITipoUsuarioRepository pronta para uso
        public static ITipoUsuarioRepository TipoUsuarioRepository()
        {
            return new TipoUsuarioRepository(CriarConexao());
        }

        // Retorna uma instância de IProdutoRepository pronta para uso
        public static IProdutoRepository ProdutoRepository()
        {
            return new ProdutoRepository(CriarConexao());
        }

        // Retorna uma instância de ICategoriaRepository pronta para uso
        public static ICategoriaRepository CategoriaRepository()
        {
            return new CategoriaRepository(CriarConexao());
        }
    }
}
