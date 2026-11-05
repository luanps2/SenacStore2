namespace SenacStore.Application.DTOs
{
    // JSON que a API devolve para uma categoria.
    public class CategoriaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
