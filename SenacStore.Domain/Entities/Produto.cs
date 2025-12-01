using System;

namespace SenacStore.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public decimal Preco { get; set; }

        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Nova propriedade: armazena apenas o caminho/URL relativo da imagem (ex: "img/produtos/{id}.jpg")
        public string FotoUrl { get; set; }
    }
}
