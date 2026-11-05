using SenacStore.Infrastructure.IoC;        // Importa o container simples IoC para configurar os serviços
using SenacStore.Infrastructure.Services;   // Importa ApiException (falhas de comunicação com a API)
using SenacStore.UI.Helpers;                // Importa ConfiguracaoApp (leitura do appsettings.json)
using System;                              // Importa tipos base do .NET (ex.: Exception, String)
using System.Threading;                    // Importa ThreadExceptionEventArgs
using System.Windows.Forms;                // Importa API WinForms (Application, Form, etc.)

namespace SenacStore.UI
{
    internal static class Program
    {
        [STAThread]                          // Atributo necessário para a thread principal de aplicações WinForms (uso de COM/clipboard)
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles(); // habilita estilos visuais modernos do Windows
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false); // usa rendering padrão do GDI+ para textos

            // Última proteção: um erro que nenhuma tela tratou vira mensagem, sem fechar a aplicação
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;

            // 1. Lê a URL da API do appsettings.json
            ConfiguracaoApp configuracao;
            try
            {
                configuracao = ConfiguracaoApp.Carregar();
            }
            catch (InvalidOperationException ex)
            {
                MostrarErro(ex.Message);
                return;
            }

            // 2. Configura IoC
            IoC.Configure(configuracao.BaseUrl, configuracao.TimeoutSegundos);

            // 3. Inicia interface
            System.Windows.Forms.Application.Run(new frmLogin(IoC.AuthService()));
            // cria e exibe o formulário de login como janela principal, injetando o serviço de autenticação via IoC
        }

        // Chamado quando um erro chega até aqui sem ter sido tratado por uma tela
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var mensagem = e.Exception is ApiException
                ? e.Exception.Message
                : $"Ocorreu um erro inesperado: {e.Exception.Message}";

            MostrarErro(mensagem);
        }

        // Mostra o erro com o mesmo diálogo usado nas telas do sistema
        private static void MostrarErro(string mensagem)
        {
            var dialogo = new Guna.UI2.WinForms.Guna2MessageDialog
            {
                Text = mensagem,
                Caption = "Erro",
                Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK,
                Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                Style = Guna.UI2.WinForms.MessageDialogStyle.Default
            };

            dialogo.Show();
        }
    }
}
