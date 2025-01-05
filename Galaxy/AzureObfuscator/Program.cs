using MetalMaxSystem;
using System;
using System.Windows.Forms;

namespace GalaxyObfuscator
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            MMCore.writeTell = true;//调用功能库打印方法同时开启调试
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Obfuscator.form1 = new Form1();
            Application.Run(Obfuscator.form1);
        }
    }
}
