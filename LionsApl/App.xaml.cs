using System;
using System.Configuration;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace LionsApl
{
    public partial class App : Application
    {
        public string AppServer;

        public App()
        {
            InitializeComponent();
            
            // Configファイルより値を取得
            AppServer = PCLAppConfig.ConfigurationManager.AppSettings["ApplicationServer"];
            
            //MainPage = new NavigationPage(new MainPage());
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
