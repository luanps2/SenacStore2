using SenacStore.Infrastructure.Database;   // Importa a classe DatabaseInitializer responsável por criar o DB/tabelas/seeds
using SenacStore.Infrastructure.IoC;        // Importa o container simples IoC para configurar repositórios/conexões
using System;                              // Importa tipos base do .NET (ex.: Exception, String)
using System.Windows.Forms;                // Importa API WinForms (Application, Form, etc.)

namespace SenacStore.UI
{
    internal static class Program
    {
        [STAThread]                          // Atributo necessário para a thread principal de aplicações WinForms (uso de COM/clipboard)
        static void Main()
        {
            // string com conexão ao banco mestre (usada para criar o banco se necessário)
            string masterConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
            // string com conexão ao banco da aplicação (após criar/usar o DB SenacStore)
            string appConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SenacStore;Integrated Security=True;";

            // 1. Cria banco + tabelas + seed se não existir
            DatabaseInitializer.Initialize(masterConnection); // chama rotina que verifica/cria o DB e executa script de migração

            // 2. Configura IoC
            IoC.Configure(appConnection); // guarda a connection string para as factories de repositório retornarem conexões corretas

            // 3. Inicia interface
            System.Windows.Forms.Application.EnableVisualStyles(); // habilita estilos visuais modernos do Windows
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false); // usa rendering padrão do GDI+ para textos
            System.Windows.Forms.Application.Run(new frmLogin(IoC.UsuarioRepository())); 
            // cria e exibe o formulário de login como janela principal, injetando o repositório de usuário via IoC
        }
    }
}
