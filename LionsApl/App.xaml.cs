using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace LionsApl
{
    public partial class App : Application
    {
        public string AppVersion;
        public string AppServer;
        public string WebServiceUrl;
        public string AppDownloadUrl;
        public string AndroidPdf;
        public string SQLiteFileName;
        public string SQLiteFileExte;
        public string FilePath_Letter;
        public string FilePath_Evnet;
        public string FilePath_Magazine;
        public string FilePath_Infometion;
        public string FilePath_Matching;
        public string FilePath_MeetingProgram;
        public string FilePath_ClubInfometion;
        public TabbedPage TabPage; 


        public System.Diagnostics.Stopwatch sw;     // ストップウォッチ
        public TimeSpan ElapsedTime;                // 経過時間
        public double RestartTime;                  // アプリのリスタート時間
        public int AndroidAlarmInterval;            // アラーム動作のインターバル(Android用)
        public int iOSNotFetchFromTime;          // フェッチの停止時刻From(iOS用)
        public int iOSNotFetchToTime;            // フェッチの停止時刻To(iOS用)

        public App()
        {
            InitializeComponent();

            // Configファイルより値を取得(Version)
            AppVersion = PCLAppConfig.ConfigurationManager.AppSettings["ApplicationVersion"];

            // Configファイルより値を取得(URL)
            AppServer = PCLAppConfig.ConfigurationManager.AppSettings["ApplicationServer"];
            WebServiceUrl = PCLAppConfig.ConfigurationManager.AppSettings["WebServiceUrl"];
            AppDownloadUrl = PCLAppConfig.ConfigurationManager.AppSettings["AppDownloadUrl"];
            AndroidPdf = PCLAppConfig.ConfigurationManager.AppSettings["AndroidPdfViewer"];

            // Configファイルより値を取得(SQLite)
            SQLiteFileName = PCLAppConfig.ConfigurationManager.AppSettings["SQLiteFileName"];
            SQLiteFileExte = PCLAppConfig.ConfigurationManager.AppSettings["SQLiteFileExtension"];

            // Configファイルより値を取得(FILEPATH)
            FilePath_Letter = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_LETTER"];
            FilePath_Evnet = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_EVENT"];
            FilePath_Magazine = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_MAGAZINE"];
            FilePath_Infometion = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_INFOMETION"];
            FilePath_Matching = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_MATCHING"];
            FilePath_MeetingProgram = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_MEETINGPROGRAM"];
            FilePath_ClubInfometion = PCLAppConfig.ConfigurationManager.AppSettings["FILEPATH_CLUBINFOMETION"];

            // Configファイルより値を取得(リスタート時間)
            RestartTime = double.Parse(PCLAppConfig.ConfigurationManager.AppSettings["RestartMinutes"]);

            // Configファイルより値を取得(アラーム動作インターバル)
            AndroidAlarmInterval = int.Parse(PCLAppConfig.ConfigurationManager.AppSettings["AndroidAlarmInterval"]);

            // Configファイルより値を取得(フェッチ停止時刻)
            iOSNotFetchFromTime = int.Parse(PCLAppConfig.ConfigurationManager.AppSettings["iOSNotFetchFromTime"]);
            iOSNotFetchToTime = int.Parse(PCLAppConfig.ConfigurationManager.AppSettings["iOSNotFetchToTime"]);

            // StopWatch生成
            sw = new System.Diagnostics.Stopwatch();

            // MainPage起動
            //MainPage = new MainPage();
            MainPage = new Content.TopLogo();
        }

        protected override void OnStart()
        {
            //// 計測開始
            //sw.Start();
        }

        protected override void OnSleep()
        {
            //// 計測停止
            //sw.Stop();
        }

        protected override void OnResume()
        {

            // TOP画面に遷移する
            MainPage = new Content.TopMenu();

            //MainPage = new MainPage();
            //// 経過時間設定
            //ElapsedTime = sw.Elapsed;
            //// アプリ起動時間がリスタート時間を超えている場合
            //if (ElapsedTime.TotalMinutes > RestartTime)
            //{
            //    // 経過時間リセット後に計測開始
            //    sw.Restart();
            //    // 開始ページに遷移
            //    MainPage = new MainPage();
            //}
            //else
            //{
            //    // 計測再開
            //    sw.Start();
            //}
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

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int GetBadgeCount()
        {
            // 変数
            SQLiteManager _sqlite;
            int badgeCount = 0;

            // データ取得
            try
            {
                // SQLite マネージャークラス生成
                _sqlite = SQLiteManager.GetInstance();

                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_BADGEUPD());

                // 未読情報を取得
                foreach (Table.T_BADGE row in _sqlite.Get_T_BADGE("SELECT * " +
                                                                    "FROM T_BADGE "))
                {
                    badgeCount += 1;
                }
                return badgeCount;
                
            }
            catch
            {
                return 0;
            }
        }
    }
}
