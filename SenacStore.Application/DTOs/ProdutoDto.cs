namespace SenacStore.Application.DTOs
{
    // JSON que a API devolve para um produto.
    public class ProdutoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
