namespace SenacStore.Application.DTOs
{
    // JSON enviado à API para criar ou atualizar um tipo de usuário (o id é gerado pelo servidor).
    public class TipoUsuarioRequestDto
    {
        public string Nome { get; set; } = string.Empty;
    }
}
