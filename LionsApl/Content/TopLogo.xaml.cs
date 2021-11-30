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
            else
            {
                // ファイルがある場合

                // 設定ファイル情報（A_SETTING）取得
                _sqlite.GetSetting();
                // 設定ファイル情報存在チェック
                if (_sqlite.Db_A_Setting != null)
                {
                    // 設定ファイル情報がある場合
                    // アカウント情報（A_ACCOUNT）取得
                    _sqlite.GetAccount();
                    // アカウント情報存在チェック
                    if (_sqlite.Db_A_Account != null)
                    {
                        // アカウント情報がある場合
                        // アプリケーションバージョン取得
                        string appVer = ((App)Application.Current).AppVersion;
                        // アカウント情報のバージョンNo.があるか確認する
                        if (_sqlite.Db_A_Account.VersionNo != null)
//                        if (_sqlite.Db_A_Account.VersionNo != LADef.NOSTR)
                        {
                            // アカウント情報のバージョンNo.がある場合

                            // アカウント情報のバージョンとアプリケーションのバージョンを比較する
                            if (double.Parse(_sqlite.Db_A_Account.VersionNo) < double.Parse(appVer))
                            {
                                // アカウント情報のバージョンNo.が古い場合

                                // アカウント情報の更新、およびそれ以外のテーブルのクリア
                                UpdAccountAndClrExTbl();

                            }
                        }
                        else 
                        {
                            // アカウント情報のバージョンNo.がない場合

                            // アカウント情報の更新、およびそれ以外のテーブルのクリア
                            UpdAccountAndClrExTbl();
                        }
                    }
                }

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

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報の更新とそれ以外のテーブルのクリア
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void UpdAccountAndClrExTbl()
        {
            // アプリケーションバージョン取得
            string appVer = ((App)Application.Current).AppVersion;

            // アカウント情報のバージョンNo.が古い場合
            // アカウント情報のバージョンNo.の更新、及び最終更新日のクリア
            //foreach (Table.A_ACCOUNT row in _sqlite.Upd_A_ACCOUNT("UPDATE A_ACCOUNT SET" +
            //                                                      " LastUpdDate = NULL, " +
            //                                                      " VersionNo = '" + appVer + "'"))
            //{
            //    _sqlite.Db_A_Account = row;
            //}
            // バージョンNo.の更新
            _sqlite.Db_A_Account.VersionNo = appVer;
            // 最終更新日のクリア
            _sqlite.Db_A_Account.LastUpdDate = LADef.NOSTR;
            // アカウント情報の追加
            _sqlite.Set_A_ACCOUNT(_sqlite.Db_A_Account);



            // アカウント情報以外のテーブルクリア
            _sqlite.DropTable_ExAccount();
            _sqlite.CreateTable_ExAccount();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面操作、及びTop情報取得
        /// </summary>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
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
                // TOP対象テーブルのクリア
                _sqlite.DropTable_Top();
                _sqlite.CreateTable_Top();

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