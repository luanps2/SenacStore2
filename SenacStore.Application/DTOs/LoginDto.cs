namespace SenacStore.Application.DTOs
{
    // JSON enviado para POST /auth/login.
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
