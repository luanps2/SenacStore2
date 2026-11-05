// Arquivo: SenacStore.UI\Handlers\CategoriaHandler.cs
// Propósito: adaptar operações do serviço de Categoria à interface ICrudHandler,
// fornecendo dados para o ucCrudPadrao e abrindo os UserControls de criação/edição.

using System;                          // Tipos base (.NET)
using System.Linq;                     // LINQ (Select, ToList)
using System.Threading.Tasks;          // Task: operações assíncronas
using SenacStore.UI.Navigation;        // ICrudNavigator: usado para abrir UserControls no container principal
using SenacStore.UI.UserControls;      // ucCategorias: UserControl para criar/editar categoria

namespace SenacStore.UI.Handlers
{
    // Implementa ICrudHandler para permitir que ucCrudPadrao opere sobre Categorias
    public class CategoriaHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;      // Navegador para abrir UserControls no frmMenu
        private readonly ICategoriaService _service; // Serviço responsável por operações de Categoria (CRUD)

        // Título exibido pelo ucCrudPadrao quando este handler é usado
        public string Titulo => "Categorias";

        // Construtor recebe o navigator e o serviço (injeção simples)
        public CategoriaHandler(ICrudNavigator nav, ICategoriaService service)
        {
            _nav = nav;         // armazena navigator para abrir UCs
            _service = service; // armazena serviço para operações na API
        }

        // Retorna os dados que serão ligados ao DataGridView no ucCrudPadrao.
        // Projeta apenas os campos relevantes (Id e Nome).
        public async Task<object> ObterTodosAsync()
        {
            var categorias = await _service.ObterTodosAsync();

            return categorias
                .Select(c => new { c.Id, c.Nome }) // projeta para objeto anônimo com só o que a view precisa
                .ToList();                         // materializa a lista para binding
        }

        // Abre o UserControl de criação de categoria dentro do container (frmMenu)
        public void Criar()
        {
            _nav.Abrir(new ucCategorias(_nav, _service));
        }

        // Abre o UserControl de edição de categoria, passando o id para carregar os dados
        public void Editar(Guid id)
        {
            _nav.Abrir(new ucCategorias(_nav, _service, id));
        }

        // Deleta a categoria através do serviço
        public async Task DeletarAsync(Guid id)
        {
            await _service.DeletarAsync(id);
        }

        public async Task<object> BuscarPorNomeAsync(string termo)
        {
            var categorias = await _service.BuscarPorNomeAsync(termo);

            return categorias
                .Select(c => new { c.Id, c.Nome }).ToList();
        }
    }
}
