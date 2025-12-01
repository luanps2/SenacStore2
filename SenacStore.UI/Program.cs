using SenacStore.Infrastructure.Database;
using SenacStore.Infrastructure.IoC;
using System;
using System.Windows.Forms;

namespace SenacStore.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string masterConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
            string appConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SenacStore;Integrated Security=True;";

            // 1. Cria banco + tabelas + seed se não existir
            DatabaseInitializer.Initialize(masterConnection);

            // 2. Configura IoC
            IoC.Configure(appConnection);

            // 3. Inicia interface
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new frmLogin(IoC.UsuarioRepository()));
        }
    }
}
