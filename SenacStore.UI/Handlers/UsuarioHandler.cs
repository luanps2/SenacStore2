// Arquivo: SenacStore.UI\Handlers\UsuarioHandler.cs
// Propósito: adaptar operações do repositório de Usuario à interface ICrudHandler,
// fornecendo dados para o ucCrudPadrao e abrindo os UCs de criação/edição.

using System;                           // Tipos básicos (.NET)
using System.Linq;                      // LINQ (Select, ToDictionary, ToList)
using SenacStore.Application;           // Interfaces de repositório/imports do projeto (IUsuarioRepository etc.)
using SenacStore.UI.Navigation;         // ICrudNavigator: usado para abrir UCs no container principal
using SenacStore.UI.UserControls;       // ucUsuarios: UserControl para criar/edição usuário

namespace SenacStore.UI.Handlers
{
    // Handler que implementa ICrudHandler para a entidade Usuario
    public class UsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;            // navigator para abrir UCs dentro do frmMenu
        private readonly IUsuarioRepository _usuarioRepo; // repositório para operações com Usuario
        private readonly ITipoUsuarioRepository _tipoRepo; // repositório para obter tipos de usuário

        // Propriedade que fornece o título exibido no ucCrudPadrao
        public string Titulo => "Usuários";

        // Construtor recebe as dependências (injeção manual via IoC)
        public UsuarioHandler(ICrudNavigator nav, IUsuarioRepository usuarioRepo, ITipoUsuarioRepository tipoRepo)
        {
            _nav = nav;                   // armazena navigator fornecido
            _usuarioRepo = usuarioRepo;   // armazena repositório de usuários
            _tipoRepo = tipoRepo;         // armazena repositório de tipos de usuário
        }

        // Retorna os dados usados pelo DataGridView no ucCrudPadrao.
        // Projeta usuários para uma lista anônima contendo campos necessários.
        public object ObterTodos()
        {
            // Carrega todos os tipos e cria um dicionário Id -> Nome para mapear rapidamente
            var tipos = _tipoRepo.ObterTodos().ToDictionary(t => t.Id, t => t.Nome);

            // Recupera todos os usuários do repositório e projeta cada um em um objeto anônimo:
            // - Id, Nome, Email, FotoUrl (para exibir imagem no grid) e Tipo (nome legível)
            return _usuarioRepo.ObterTodos()
                .Select(u => new {
                    u.Id,
                    u.Nome,
                    u.Email,
                    FotoUrl = u.FotoUrl, // caminho relativo da foto (pode ser null)
                    Tipo = tipos.ContainsKey(u.TipoUsuarioId) ? tipos[u.TipoUsuarioId] : "Desconhecido"
                })
                .ToList(); // materializa a consulta em lista para binding
        }

        // Abre o UC para criação de um novo usuário
        public void Criar() =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioRepo, _tipoRepo));

        // Abre o UC para edição do usuário identificado por id
        public void Editar(Guid id) =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioRepo, _tipoRepo, id));

        // Deleta o usuário pelo id delegando ao repositório
        public void Deletar(Guid id) =>
            _usuarioRepo.Deletar(id);

        public object BuscarPorNome(string termo)
        {
            var tipos = _tipoRepo.ObterTodos().ToDictionary(t => t.Id, t => t.Nome);
            return _usuarioRepo.BuscarPorNome(termo)
                .Select(u => new {
                    u.Id, u.Nome, u.Email, FotoUrl = u.FotoUrl,
                    Tipo = tipos.TryGetValue(u.TipoUsuarioId, out var nome) ? nome : "Desconhecido"
                }).ToList();
        }
    }
}
