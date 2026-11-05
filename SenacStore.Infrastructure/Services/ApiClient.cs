// Arquivo: SenacStore.Infrastructure\Services\ApiClient.cs
// Propósito: único ponto do sistema que faz requisições HTTP.
// Monta as requisições, converte JSON e transforma qualquer falha em ApiException.

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SenacStore.Application.DTOs;

namespace SenacStore.Infrastructure.Services
{
    public class ApiClient
    {
        // "Web" = nomes em camelCase no JSON (Nome <-> nome) e números que chegam como texto são aceitos.
        private static readonly JsonSerializerOptions OpcoesJson = new(JsonSerializerDefaults.Web);

        // Um único HttpClient para o programa todo: criar um por requisição esgota as conexões.
        private readonly HttpClient _http;

        public ApiClient(string baseUrl, int timeoutSegundos)
        {
            _http = new HttpClient
            {
                // A barra final é obrigatória para a rota ser somada à URL base corretamente.
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"),
                Timeout = TimeSpan.FromSeconds(timeoutSegundos)
            };
        }

        // Guarda o token JWT: a partir daqui ele vai em todas as requisições. Null remove o token.
        public void DefinirToken(string? token)
        {
            _http.DefaultRequestHeaders.Authorization =
                token == null ? null : new AuthenticationHeaderValue("Bearer", token);
        }

        // GET: devolve o JSON da resposta convertido para T.
        public async Task<T?> GetAsync<T>(string rota)
        {
            var json = await EnviarAsync(HttpMethod.Get, rota);
            return LerJson<T>(json);
        }

        // GET de um único registro: 404 não é erro aqui, apenas significa "não existe".
        public async Task<T?> GetOuNuloAsync<T>(string rota) where T : class
        {
            try
            {
                return await GetAsync<T>(rota);
            }
            catch (ApiException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // POST: envia o corpo em JSON e devolve o objeto que a API respondeu.
        public async Task<T> PostAsync<T>(string rota, object corpo) where T : class
        {
            var json = await EnviarAsync(HttpMethod.Post, rota, corpo);
            return LerJson<T>(json) ?? throw RespostaInvalida();
        }

        // PUT: basta a API responder com sucesso (qualquer status 2xx).
        public async Task PutAsync(string rota, object corpo)
        {
            await EnviarAsync(HttpMethod.Put, rota, corpo);
        }

        // DELETE: basta a API responder com sucesso (qualquer status 2xx).
        public async Task DeleteAsync(string rota)
        {
            await EnviarAsync(HttpMethod.Delete, rota);
        }

        // Envia a requisição e devolve o texto da resposta. Todos os métodos acima passam por aqui.
        private async Task<string> EnviarAsync(HttpMethod metodo, string rota, object? corpo = null)
        {
            using var requisicao = new HttpRequestMessage(metodo, rota);

            if (corpo != null)
            {
                var json = JsonSerializer.Serialize(corpo, OpcoesJson);
                requisicao.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            try
            {
                using var resposta = await _http.SendAsync(requisicao);
                var texto = await resposta.Content.ReadAsStringAsync();

                if (resposta.IsSuccessStatusCode)
                    return texto;

                throw new ApiException(MensagemDeErro(resposta.StatusCode, texto), (int)resposta.StatusCode);
            }
            catch (HttpRequestException)
            {
                // Sem resposta: API desligada, URL errada ou sem internet.
                throw new ApiException("Não foi possível conectar à API. Verifique se ela está em execução.");
            }
            catch (TaskCanceledException)
            {
                // O HttpClient cancela a requisição quando o Timeout estoura.
                throw new ApiException("A API demorou demais para responder. Tente novamente.");
            }
        }

        // Converte o texto JSON para T. Resposta vazia vira null (por exemplo, 204 No Content).
        private static T? LerJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json, OpcoesJson);
            }
            catch (JsonException)
            {
                throw RespostaInvalida();
            }
        }

        private static ApiException RespostaInvalida()
        {
            return new ApiException("A API respondeu em um formato inesperado.");
        }

        // Usa a mensagem enviada pela API; se não houver, escolhe uma pelo código HTTP.
        private static string MensagemDeErro(HttpStatusCode status, string corpo)
        {
            try
            {
                var erro = JsonSerializer.Deserialize<ErroDto>(corpo, OpcoesJson);
                if (!string.IsNullOrWhiteSpace(erro?.Mensagem))
                    return erro.Mensagem;
            }
            catch (JsonException)
            {
                // O corpo não é JSON (por exemplo, página HTML de erro): usa a mensagem padrão abaixo.
            }

            return status switch
            {
                HttpStatusCode.BadRequest => "A API recusou os dados enviados. Confira os campos.",
                HttpStatusCode.Unauthorized => "Sua sessão expirou. Faça login novamente.",
                HttpStatusCode.Forbidden => "Você não tem permissão para esta operação.",
                HttpStatusCode.NotFound => "Registro não encontrado.",
                HttpStatusCode.Conflict => "A operação conflita com outros registros.",
                HttpStatusCode.RequestEntityTooLarge => "Os dados enviados são grandes demais para a API.",
                >= HttpStatusCode.InternalServerError => "A API encontrou um erro interno. Tente novamente mais tarde.",
                _ => $"A API recusou a operação (código {(int)status})."
            };
        }
    }
}
