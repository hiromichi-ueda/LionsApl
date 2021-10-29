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
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// TOPメニュー画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopMenu : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public TopMenu()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // アプリケーションバージョン設定
            AppVersion.Text = "Ver " + ((App)Application.Current).AppVersion;

            // 開発用ボタンを非表示にする
            //Develop.IsVisible = false;

            // ボタンコントロール初期値
            account.IsEnabled = false;
            home.IsEnabled = false;
            update.IsVisible = false;
            message.IsVisible = false;

            // ボタンコントロール
            ControlButtonEnable();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 画面制御

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ボタンコントロール
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void ControlButtonEnable()
        {

            try
            {
                // SQLiteファイル存在チェック
                if (_sqlite.CheckFileDB3() == _sqlite.SQLITE_NOFILE)
                {
                    // ファイルがない場合
                    DisplayAlert("Alert", $"エラーが発生しました。{Environment.NewLine}" +
                                           "アプリを終了して再起動してください。", "OK");

                    // SQLiteファイル＆ALLテーブル作成
                    //_sqlite.CreateTable_ALL();

                }
                else
                {
                    // ファイルがある場合
                    // 設定ファイル情報（A_SETTING）取得
                    _sqlite.GetSetting();
                    // 設定ファイル情報存在チェック
                    if (_sqlite.Db_A_Setting != null)
                    {
                        // アカウントボタンON
                        this.account.IsEnabled = true;

                        // 設定ファイル情報がある場合
                        // アカウント情報（A_ACCOUNT）取得
                        _sqlite.GetAccount();
                        // アカウント情報存在チェック
                        if (_sqlite.Db_A_Account != null)
                        {
                            // アカウント情報がある場合
                            // ホームボタン有効
                            this.home.IsEnabled = true;
                        }
                        // ボタンコントロール（アップデートボタン）
                        ControlUpdBtnEnable();
                    }
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"ボタン制御エラー : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ボタンコントロール（アップデートボタン）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void ControlUpdBtnEnable()
        {
            double dbVer = 0.0;
            double appVer = 0.0;

            try
            {
                // DB設定アプリバージョン取得
                dbVer = double.Parse(_sqlite.Db_A_Setting.VersionNo);
                // ローカルアプリバージョン取得
                appVer = double.Parse(((App)Application.Current).AppVersion);
                // バージョンチェック
                if (dbVer > appVer)
                {
                    // DB設定アプリバージョンの方が新しい場合
                    // ダウンロードボタン表示
                    update.Text = "アップデート  Ver " + _sqlite.Db_A_Setting.VersionNo;
                    update.IsVisible = true;
                    // メッセージ表示
                    message.IsVisible = true;
                }
            }
            catch
            {
                throw;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// ボタン処理

        #region 各ボタン処理

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ホームボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Home_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            // DB情報取得処理
            try
            {
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_HOME());

                // HOME画面表示
                Application.Current.MainPage = new Content.MainTabPage();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"ホームボタン制御エラー : {ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Account_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            try
            {
                // SQLiteファイル削除
                //_sqlite.DelFileDB3();

                // 空DB作成処理
                //_sqlite.CreateTable_ALL();

                // アカウント情報取得
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent());

                // アカウント設定画面表示
                Application.Current.MainPage = new Content.AccountSetting();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"アカウントボタン制御エラー : {ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アップデートボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //async void Button_Update_Clicked(object sender, System.EventArgs e)
        [Obsolete]
        void Button_Update_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            //            await ((App)Application.Current).DispLoadingDialog();
            Device.OpenUri(new Uri("http://ap.insat.co.jp/LionsApl/DownLoad.html"));

        }


        private void OnLogoTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        #endregion


    }
}