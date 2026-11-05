namespace SenacStore.Application.DTOs
{
    // JSON que a API devolve para um tipo de usuário.
    public class TipoUsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
