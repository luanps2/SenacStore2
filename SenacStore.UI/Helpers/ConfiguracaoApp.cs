// Arquivo: SenacStore.UI\Helpers\ConfiguracaoApp.cs
// Propósito: lê o appsettings.json que fica ao lado do executável.
// Assim a URL da API muda sem precisar recompilar o projeto.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SenacStore.UI.Helpers
{
    public class ConfiguracaoApp
    {
        private const int TimeoutPadraoSegundos = 30;

        public string BaseUrl { get; }
        public int TimeoutSegundos { get; }

        private ConfiguracaoApp(string baseUrl, int timeoutSegundos)
        {
            BaseUrl = baseUrl;
            TimeoutSegundos = timeoutSegundos;
        }

        // Lança InvalidOperationException, com mensagem amigável, se o arquivo estiver errado.
        public static ConfiguracaoApp Carregar()
        {
            var caminho = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            if (!File.Exists(caminho))
                throw new InvalidOperationException($"Arquivo de configuração não encontrado:\n{caminho}");

            string? baseUrl;
            int timeout;

            try
            {
                using var documento = JsonDocument.Parse(File.ReadAllText(caminho));
                var api = documento.RootElement.GetProperty("Api");

                baseUrl = api.GetProperty("BaseUrl").GetString();

                // TimeoutSegundos é opcional no arquivo
                timeout = api.TryGetProperty("TimeoutSegundos", out var valor)
                    ? valor.GetInt32()
                    : TimeoutPadraoSegundos;
            }
            catch (Exception ex) when (ex is JsonException or KeyNotFoundException
                                          or FormatException or InvalidOperationException)
            {
                throw new InvalidOperationException("O arquivo appsettings.json está incompleto ou com formato inválido.");
            }

            if (!UrlValida(baseUrl) || timeout <= 0)
                throw new InvalidOperationException("Verifique os valores de BaseUrl e TimeoutSegundos no appsettings.json.");

            return new ConfiguracaoApp(baseUrl!, timeout);
        }

        // Aceita apenas endereços completos, como http://localhost:3000/
        private static bool UrlValida(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
