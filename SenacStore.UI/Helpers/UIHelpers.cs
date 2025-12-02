// Arquivo: SenacStore.UI\Helpers\UIHelpers.cs
// Propósito: coleção de métodos utilitários para preencher controles de UI (ComboBox, DataGridView).
// Cada método aplica data binding projetando apenas os campos necessários para a exibição.

using System.Collections.Generic;    // Tipos genéricos (List<T>) usados nas assinaturas dos métodos
using System.Linq;                   // LINQ (Select) usado para projetar coleções antes de atribuir ao DataGridView
using System.Windows.Forms;          // Controles WinForms (ComboBox, DataGridView)
using SenacStore.Domain.Entities;    // Entidades do domínio (Categoria, TipoUsuario, Produto)

namespace SenacStore.UI.Helpers
{
    // Classe estática que agrupa helpers de UI reutilizáveis
    public static class UIHelpers
    {
        // Preenche um ComboBox com uma lista de Categorias.
        // combo: controle ComboBox a ser preenchido.
        // categorias: lista de entidades Categoria obtida do repositório.
        public static void CarregarCategorias(ComboBox combo, List<Categoria> categorias)
        {
            combo.DataSource = categorias;   // Define a fonte de dados do ComboBox
            combo.DisplayMember = "Nome";    // Propriedade a ser exibida para cada item
            combo.ValueMember = "Id";        // Propriedade usada como valor do item (usada ao salvar)
        }

        // Preenche um ComboBox com uma lista de Tipos de Usuário.
        // Mesma ideia de CarregarCategorias, apenas para TipoUsuario.
        public static void CarregarTiposUsuario(ComboBox combo, List<TipoUsuario> tipos)
        {
            combo.DataSource = tipos;        // Fonte de dados: lista de TipoUsuario
            combo.DisplayMember = "Nome";    // Exibe a propriedade Nome
            combo.ValueMember = "Id";        // Usa Id como valor associado a cada item
        }

        // Preenche um DataGridView com uma lista de produtos projetada para os campos necessários.
        // Em vez de ligar a lista de entidades diretamente, projeta objetos anônimos com só os campos desejados.
        // Isso evita expor propriedades desnecessárias e facilita a exibição.
        public static void CarregarProdutosGrid(DataGridView grid, List<Produto> produtos)
        {
            // Usa LINQ para projetar cada Produto em um objeto anônimo contendo só Id, Nome, Preco e CategoriaId,
            // então materializa em lista e atribui ao DataSource do grid.
            grid.DataSource = produtos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Preco,
                p.CategoriaId
            }).ToList();
        }
    }
}
