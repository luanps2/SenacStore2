using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenacStore.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public decimal Preco { get; set; }

        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }

}
