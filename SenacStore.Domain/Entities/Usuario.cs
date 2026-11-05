namespace SenacStore.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string Email { get; set; }

        // A API nunca devolve a senha: este campo só é preenchido para enviar uma senha nova.
        public string? Senha { get; set; }

        public Guid TipoUsuarioId { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }

}
