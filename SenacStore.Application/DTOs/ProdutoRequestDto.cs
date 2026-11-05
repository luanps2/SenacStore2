namespace SenacStore.Application.DTOs
{
    // JSON enviado à API para criar ou atualizar um produto (o id é gerado pelo servidor).
    public class ProdutoRequestDto
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
