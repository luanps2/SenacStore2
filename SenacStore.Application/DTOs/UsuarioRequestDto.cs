using System.Text.Json.Serialization;

namespace SenacStore.Application.DTOs
{
    // JSON enviado à API para criar ou atualizar um usuário (o id é gerado pelo servidor).
    public class UsuarioRequestDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Sem senha, o campo nem vai no JSON: na edição, a API mantém a senha atual.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Senha { get; set; }

        public Guid TipoUsuarioId { get; set; }
    }
}
