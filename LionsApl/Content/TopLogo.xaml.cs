using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// TOPロゴ画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopLogo : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // 処理パスフラグ
        private bool pathFlg = false;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

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

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ScrCtrlAndGetTopInfo();

            if (!pathFlg)
            {
                // MainPage起動
                Application.Current.MainPage = new TopMenu();
            }

        }

        private async Task ScrCtrlAndGetTopInfo()
        {
            while (LogoProgress.Progress < 0.5)
            {
                await Task.Delay(10);
                LogoProgress.Progress += 0.01;
            }

            try
            {
                // TOP情報取得
                //_ = GetTopInfo();
                GetTopInfo();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"TOP情報取得エラー : {ex.Message}", "OK");
            }

            while (LogoProgress.Progress < 1.0)
            {
                await Task.Delay(10);
                LogoProgress.Progress += 0.01;
            }

            //Task<HttpResponseMessage> response = null;
            //try
            //{
            //    // TOP情報取得
            //    response = _sqlite.NAsyncPostFileForWebAPI(_sqlite.GetSendFileContent_TOP());
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Alert", $"TOP情報取得エラー : {ex.Message}", "OK");
            //}

            //while (LogoProgress.Progress < 1.0)
            //{
            //    await Task.Delay(10);
            //    LogoProgress.Progress += 0.001;

            //    if (response != null)
            //    {
            //    //    if (response.Result.StatusCode == HttpStatusCode.OK)
            //    //    {
            //    //        LogoProgress.Progress = 1.0;
            //    //    }
            //    }
            //}
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Top情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //private async Task GetTopInfo()
        private void GetTopInfo()
        {
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

        private void OnLogoTapped(object sender, EventArgs e)
        {
            pathFlg = true;
            Application.Current.MainPage = new TopMenu();

        }
    }
}