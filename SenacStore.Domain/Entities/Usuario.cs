namespace SenacStore.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Guid TipoUsuarioId { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        // Nova propriedade: armazena apenas o caminho/URL relativo da imagem (ex: "img/usuarios/usuario.jpg")
        public string FotoUrl { get; set; }
    }

}
