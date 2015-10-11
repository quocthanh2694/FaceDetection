using System;
using System.Windows.Forms;
using DXApplication1.gui;
namespace DXApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

  

            Application.Run(new NhanDang());
             
        }
    }
}