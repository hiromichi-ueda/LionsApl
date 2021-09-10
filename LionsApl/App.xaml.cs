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
        public string AndroidPdf;
        public string FilePath_Letter;
        public string FilePath_Evnet;
        public string FilePath_Magazine;
        public string FilePath_Infometion;
        public string FilePath_MeetingProgram;
        public string FilePath_ClubInfometion;

        public App()
        {
            InitializeComponent();
            
            // Configファイルより値を取得(URL)
            AppServer = PCLAppConfig.ConfigurationManager.AppSettings["ApplicationServer"];
            WebServiceUrl = PCLAppConfig.ConfigurationManager.AppSettings["WebServiceUrl"];
            AndroidPdf = PCLAppConfig.ConfigurationManager.AppSettings["AndroidPdfViewer"];

            // Configファイルより値を取得(FILEPATH)
            FilePath_Letter = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_LETTER"];
            FilePath_Evnet = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_EVENT"];
            FilePath_Magazine = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_MAGAZINE"];
            FilePath_Infometion = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_INFOMETION"];
            FilePath_MeetingProgram = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_MEETINGPROGRAM"];
            FilePath_ClubInfometion = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_CLUBINFOMETION"];

            // MainPage起動
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

        //-----------------------------------
        // Loading画面
        //-----------------------------------
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
