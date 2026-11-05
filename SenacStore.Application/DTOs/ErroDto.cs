namespace SenacStore.Application.DTOs
{
    // JSON que a API devolve em caso de erro: { "mensagem": "..." }.
    public class ErroDto
    {
        public string? Mensagem { get; set; }
    }
}
