namespace Obfuscator
{
    internal static class Program
    {
        /// <summary>
        /// Ӧ�ó��������ڵ�
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}