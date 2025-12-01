using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SenacStore.Domain.Entities;

namespace SenacStore.UI.Helpers
{
    public static class UIHelpers
    {
        public static void CarregarCategorias(ComboBox combo, List<Categoria> categorias)
        {
            combo.DataSource = categorias;
            combo.DisplayMember = "Nome";
            combo.ValueMember = "Id";
        }

        public static void CarregarTiposUsuario(ComboBox combo, List<TipoUsuario> tipos)
        {
            combo.DataSource = tipos;
            combo.DisplayMember = "Nome";
            combo.ValueMember = "Id";
        }

        public static void CarregarProdutosGrid(DataGridView grid, List<Produto> produtos)
        {
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
