// Arquivo: SenacStore.UI\Handlers\UsuarioHandler.cs
// Propósito: adaptar operações do serviço de Usuario à interface ICrudHandler,
// fornecendo dados para o ucCrudPadrao e abrindo os UCs de criação/edição.

using System;                           // Tipos básicos (.NET)
using System.Linq;                      // LINQ (Select, ToDictionary, ToList)
using System.Threading.Tasks;           // Task: operações assíncronas
using SenacStore.UI.Navigation;         // ICrudNavigator: usado para abrir UCs no container principal
using SenacStore.UI.UserControls;       // ucUsuarios: UserControl para criar/edição usuário

namespace SenacStore.UI.Handlers
{
    // Handler que implementa ICrudHandler para a entidade Usuario
    public class UsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;            // navigator para abrir UCs dentro do frmMenu
        private readonly IUsuarioService _usuarioService; // serviço para operações com Usuario
        private readonly ITipoUsuarioService _tipoService; // serviço para obter tipos de usuário

        // Propriedade que fornece o título exibido no ucCrudPadrao
        public string Titulo => "Usuários";

        // Construtor recebe as dependências (injeção manual via IoC)
        public UsuarioHandler(ICrudNavigator nav, IUsuarioService usuarioService, ITipoUsuarioService tipoService)
        {
            _nav = nav;                       // armazena navigator fornecido
            _usuarioService = usuarioService; // armazena serviço de usuários
            _tipoService = tipoService;       // armazena serviço de tipos de usuário
        }

        // Retorna os dados usados pelo DataGridView no ucCrudPadrao.
        // Projeta usuários para uma lista anônima contendo campos necessários.
        public async Task<object> ObterTodosAsync()
        {
            // Carrega todos os tipos e cria um dicionário Id -> Nome para mapear rapidamente
            var tipos = (await _tipoService.ObterTodosAsync()).ToDictionary(t => t.Id, t => t.Nome);
            var usuarios = await _usuarioService.ObterTodosAsync();

            // Projeta cada usuário em um objeto anônimo:
            // - Id, Nome, Email e Tipo (nome legível)
            return usuarios
                .Select(u => new {
                    u.Id,
                    u.Nome,
                    u.Email,
                    Tipo = tipos.ContainsKey(u.TipoUsuarioId) ? tipos[u.TipoUsuarioId] : "Desconhecido"
                })
                .ToList(); // materializa a consulta em lista para binding
        }

        // Abre o UC para criação de um novo usuário
        public void Criar() =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioService, _tipoService));

        // Abre o UC para edição do usuário identificado por id
        public void Editar(Guid id) =>
            _nav.Abrir(new ucUsuarios(_nav, _usuarioService, _tipoService, id));

        // Deleta o usuário pelo id delegando ao serviço
        public async Task DeletarAsync(Guid id) =>
            await _usuarioService.DeletarAsync(id);

        public async Task<object> BuscarPorNomeAsync(string termo)
        {
            var tipos = (await _tipoService.ObterTodosAsync()).ToDictionary(t => t.Id, t => t.Nome);
            var usuarios = await _usuarioService.BuscarPorNomeAsync(termo);

            return usuarios
                .Select(u => new {
                    u.Id, u.Nome, u.Email,
                    Tipo = tipos.TryGetValue(u.TipoUsuarioId, out var nome) ? nome : "Desconhecido"
                }).ToList();
        }
    }
}
