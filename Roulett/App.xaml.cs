using System;
using System.Windows;

namespace Roulett
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Elsőként a bejelentkező ablakot nyitjuk meg
            _3_as_projekt loginWindow = new _3_as_projekt();
            loginWindow.Show();
        }
    }
}