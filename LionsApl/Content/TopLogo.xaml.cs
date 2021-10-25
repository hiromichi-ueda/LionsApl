using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopLogo : ContentPage
    {

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public TopLogo()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();


            // SQLiteファイル存在チェック
            if (_sqlite.CheckFileDB3() == _sqlite.SQLITE_NOFILE)
            {
                // ファイルがない場合

                // SQLiteファイル＆ALLテーブル作成
                _sqlite.CreateTable_ALL();

            }

            //var image = new Image { 
            //    Source = ImageSource.FromResource("ImageSample.Resources.LCI_emblem_white.png"),
            //};
            //ImageStack.VerticalOptions = LayoutOptions.Center;
            //ImageStack.Children.Add(image);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await LogoProgress.ProgressTo(1.0, 1000, Easing.Linear);

            try
            {
                // TOP情報取得
                //_ = GetTopInfo();
                GetTopInfo();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"TOP情報取得エラー(OnAppearing) : {ex.Message}", "OK");
            }

            // MainPage起動
            Application.Current.MainPage = new MainPage();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Top情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //private async Task GetTopInfo()
        private void GetTopInfo()
        {
            //Device.StartTimer(System.TimeSpan.FromMilliseconds(200), () =>
            //{
            //    LogoProgress.Progress += 0.01;
            //    return LogoProgress.Progress != 1;
            //});

            //await LogoProgress.ProgressTo(0.8, 5000, Easing.Linear);

            // DB情報取得処理
            try
            {
                // TOP情報取得
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_TOP());
            }
            catch
            {
                throw;
            }
        }
    }
}