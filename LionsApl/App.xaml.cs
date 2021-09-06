using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace LionsApl
{
    public partial class App : Application
    {
        public string AppServer;
        public string WebServiceUrl;

        public App()
        {
            InitializeComponent();
            
            // Configファイルより値を取得
            AppServer = PCLAppConfig.ConfigurationManager.AppSettings["ApplicationServer"];
            WebServiceUrl = PCLAppConfig.ConfigurationManager.AppSettings["WebServiceUrl"];

            MainPage = new MainPage();

            //起動画面(確認用)
            //MainPage = new Content.MagazineList();

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

        public async Task DispLoadingDialog()
        {
            // ダイアログの初期設定
            Configurations.LoadingConfig = new LoadingConfig
            {
                IndicatorColor = Color.White,
                OverlayColor = Color.Black,
                Opacity = 0.6,
                FontSize = 18,
                DefaultMessage = "Now Loading.....",
                ProgressMessageFormat = "",
            };

            // ダイアログ表示
            await Loading.Instance.StartAsync(async progress =>
            {
                // タスク待機(ダイアログ表示のために2回以上必要)
                for (var i = 0; i < 2; i++)
                {
                    await Task.Delay(10);
                }
            });
        }
    }
}
