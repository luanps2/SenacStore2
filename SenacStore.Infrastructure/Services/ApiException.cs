// Arquivo: SenacStore.Infrastructure\Services\ApiException.cs
// Propósito: representa qualquer falha ao falar com a API.
// A propriedade Message já traz um texto amigável, pronto para mostrar ao usuário.

namespace SenacStore.Infrastructure.Services
{
    public class ApiException : Exception
    {
        // Código HTTP da resposta (404, 409...). Fica null quando nem houve resposta (API offline).
        public int? StatusCode { get; }

        public ApiException(string mensagem, int? statusCode = null) : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }
}
