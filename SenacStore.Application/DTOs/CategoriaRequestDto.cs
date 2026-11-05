namespace SenacStore.Application.DTOs
{
    // JSON enviado à API para criar ou atualizar uma categoria (o id é gerado pelo servidor).
    public class CategoriaRequestDto
    {
        public string Nome { get; set; } = string.Empty;
    }
}
