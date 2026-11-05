namespace SenacStore.Application.DTOs
{
    // JSON que a API devolve para um usuário. Repare que não existe campo de senha.
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid TipoUsuarioId { get; set; }
    }
}
