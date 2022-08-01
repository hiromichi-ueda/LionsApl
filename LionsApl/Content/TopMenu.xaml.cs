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

            // Content Utilクラス生成
            _utl = new LAUtility();

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

            // 未読情報データ取得
            GetBadgeData();

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
        /// <summary>
        /// 未読情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetBadgeData()
        {
            int intEventCount = 0;
            int intDirectorCount = 0;
            int intInfomationCount = 0;
            Label lblTitle1;
            Label lblCount1;
            Label lblTitle2;
            Label lblCount2;
            Label lblTitle3;
            Label lblCount3;

            string strTitle1 = string.Empty;
            string strCount1 = string.Empty;
            string strTitle2 = string.Empty;
            string strCount2 = string.Empty;
            string strTitle3 = string.Empty;
            string strCount3 = string.Empty;

            int rowCount = 0;

            try
            {
                foreach (Table.T_BADGE row in _sqlite.Get_T_BADGE("SELECT * " +
                                                                  "FROM T_BADGE " +
                                                                  "ORDER BY DataClass"))
                {
                    // データ種別ごとの件数を取得
                    switch (row.DataClass)
                    {
                        case LADef.DATACLASS_EV:
                            intEventCount += 1;
                            break;
                        case LADef.DATACLASS_DI:
                            intDirectorCount += 1;
                            break;
                        case LADef.DATACLASS_IN:
                            intInfomationCount += 1;
                            break;
                        default:
                            break;
                    }
                }

                if (intEventCount > 0)
                {
                    // 未読情報(出欠確認)
                    // 項目名
                    strTitle1 = "出欠確認";
                    lblTitle1 = _utl.CreateLabel_Style(strTitle1,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 0, 1);
                    grdBadge.Children.Add(lblTitle1);

                    // 項目値
                    strCount1 = intEventCount.ToString() + "件";
                    lblCount1 = _utl.CreateLabel_Style(strCount1,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 1, 1);
                    grdBadge.Children.Add(lblCount1);

                    // 出力行カウント
                    rowCount++;
                }
                if (intDirectorCount > 0)
                {
                    // 未読情報(理事・委員会・その他)
                    // 項目名
                    strTitle2 = "理事・委員会・その他";
                    lblTitle2 = _utl.CreateLabel_Style(strTitle2,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 0, 1);
                    grdBadge.Children.Add(lblTitle2);

                    // 項目値
                    strCount2 = intDirectorCount.ToString() + "件";
                    lblCount2 = _utl.CreateLabel_Style(strCount2,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 1, 1);
                    grdBadge.Children.Add(lblCount2);

                    // 出力行カウント
                    rowCount++;
                }
                if (intInfomationCount > 0)
                {
                    // 未読情報(連絡事項)
                    // 項目名
                    strTitle3 = "連絡事項";
                    lblTitle3 = _utl.CreateLabel_Style(strTitle3,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 0, 1);
                    grdBadge.Children.Add(lblTitle3);

                    // 項目値
                    strCount3 = intInfomationCount.ToString() + "件";
                    lblCount3 = _utl.CreateLabel_Style(strCount3,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base",
                                                       rowCount, 1, 1);
                    grdBadge.Children.Add(lblCount3);

                    // 出力行カウント
                    rowCount++;
                }

                if(intEventCount == 0 && intDirectorCount == 0 && intInfomationCount == 0)
                {
                    // 未読情報(0件)
                    // 項目名
                    strTitle1 = "新着情報はありません";
                    lblTitle1 = _utl.CreateLabel_Style(strTitle1,
                                                       NamedSize.Large,
                                                       LayoutOptions.Center,
                                                       "Page_Base_Center",
                                                       rowCount, 0, 2);
                    grdBadge.Children.Add(lblTitle1);

                    BadgeInfo.HeightRequest = 100;
                }
                else
                {
                    BadgeInfo.HeightRequest = 100 + 40 * (rowCount - 1);
                }
                

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_BADGE) : {ex.Message}", "OK");
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


                // アカウント情報存在チェック
                if (_sqlite.Db_A_Account != null)
                {
                    // アカウント情報がある場合
                    // アカウント情報の再取得が必要なため最終更新日クリア
                    _sqlite.Db_A_Account.LastUpdDate = LADef.NOSTR;
                    // アカウント情報追加（更新）
                    _sqlite.Set_A_ACCOUNT(_sqlite.Db_A_Account);
                }

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
            //Device.OpenUri(new Uri("http://ap.insat.co.jp/LionsApl/DownLoad.html"));
            Device.OpenUri(new Uri(((App)Application.Current).AppDownloadUrl));

        }


        private void OnLogoTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        #endregion

        public sealed class BadgeRow
        {
            public BadgeRow(string title, string badgeCount)
            {
                Title = title;
                BadgeCount = badgeCount;
            }
            public string Title { get; set; }
            public string BadgeCount { get; set; }
        }

    }
}