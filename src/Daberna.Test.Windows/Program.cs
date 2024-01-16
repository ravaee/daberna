using Daberna.Common;

namespace Daberna.Test.Windows
{
    internal static class Program
    {
        public static TCPClient Client = new TCPClient(SocketSettings.ServerIP, int.Parse(SocketSettings.ServerPort)); 

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainPage());
        }
    }
}