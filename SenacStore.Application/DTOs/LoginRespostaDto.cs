namespace SenacStore.Application.DTOs
{
    // JSON devolvido por POST /auth/login: o token JWT e os dados do usuário.
    public class LoginRespostaDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto? Usuario { get; set; }
    }
}
