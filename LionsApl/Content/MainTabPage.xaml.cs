using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabPage : TabbedPage
    {
        // Utilityクラス
        private LAUtility _utl;

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // 出欠確認初回表示フラグ
        private bool isFirstDispEvent = true;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MainTabPage()
        {
            InitializeComponent();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            ((App)Application.Current).TabPage = this;

            // 未読バッジの設定
            SetBadgeInfo();
        }

        protected override void OnCurrentPageChanged()
        {
            // イベント内の処理をメソッド呼び出しにする(処理中カーソル表示のため)
            CurrentPageChangeEvent();
            //base.OnCurrentPageChanged();

            //var navigationPage = CurrentPage as NavigationPage;

            ////タイトルバー背景・文字色
            //navigationPage.BarBackgroundColor = Color.FromRgb(33, 150, 243);
            //navigationPage.BarTextColor = Color.White;

            //// 出欠確認の場合
            //if (navigationPage.Title.Equals(EventNavi.Title))
            //{
            //    // 未読情報取得
            //    GetBadgeInfo(navigationPage);
            //}
            //else
            //{
            //    // ナビゲーションのルートに戻る
            //    navigationPage.PopToRootAsync();
            //}

            //DisplayAlert("OnCurrentPageChanged", "カレントページチェンジ", "OK");

            //var currentPage = navigationPage.CurrentPage;
            //if (currentPage.GetType() == typeof(HomeTop))
            //{
            //    DisplayAlert("CurrentPageChanged works correctly", "HomeTop", "ok");
            //}
            //else if (currentPage.GetType() == typeof(ClubTop))
            //{
            //    DisplayAlert("CurrentPageChanged works correctly", "ClubTop", "ok");
            //}
            //else if (currentPage.GetType() == typeof(MatchingTop))
            //{
            //    DisplayAlert("CurrentPageChanged works correctly", "MatchingTop", "ok");
            //}
            //else if (currentPage.GetType() == typeof(AccountTop))
            //{
            //    DisplayAlert("CurrentPageChanged works correctly", "AccountTop", "ok");
            //}

        }

        //protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    base.OnPagesChanged(e);
        //}

        private async void CurrentPageChangeEvent()
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            base.OnCurrentPageChanged();

            var navigationPage = CurrentPage as NavigationPage;

            //タイトルバー背景・文字色
            navigationPage.BarBackgroundColor = Color.FromRgb(33, 150, 243);
            navigationPage.BarTextColor = Color.White;

            // 出欠確認の場合
            if (navigationPage.Title.Equals(EventNavi.Title))
            {
                // 未読情報取得
                GetBadgeInfo(navigationPage);
            }
            else
            {
                // ナビゲーションのルートに戻る
                await navigationPage.PopToRootAsync();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void GetBadgeInfo(NavigationPage page)
        {
            // DB情報取得処理
            try
            {
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_BADGEUPD());

                // タブページのバッジ更新
                SetBadgeInfo();

                // ナビゲーションのルートに戻る
                await page.PopToRootAsync();

                if (isFirstDispEvent)
                {
                    // 初回表示時は、OnAppearingイベントが呼ばれないため、直接更新メソッドを呼び出す
                    var rootPage = page.RootPage as EventList;
                    rootPage.UpdEventData();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"出欠確認一覧表示エラー : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void SetBadgeInfo()
        {
            // 変数
            int wkDataNo = 0;
            string wkDataClass = string.Empty;
            int clubBadgeCount = 0;
            int eventBadgeCount = 0;

            // データ取得
            try
            {
                // 未読情報を取得
                foreach (Table.T_BADGE row in _sqlite.Get_T_BADGE("SELECT * " +
                                                                    "FROM T_BADGE "))
                {
                    wkDataNo = row.DataNo;
                    wkDataClass = _utl.GetString(row.DataClass);

                    if (wkDataClass == "1")
                    {
                        eventBadgeCount += 1;
                    }
                    else if (wkDataClass == "2" || wkDataClass == "3")
                    {
                        clubBadgeCount += 1;
                    }
                }

                // バッジを設定
                if (eventBadgeCount > 0)
                {
                    EventNavi.BindingContext = new { EventCount = "" + eventBadgeCount };
                }
                else
                {
                    EventNavi.BindingContext = new { EventCount = "" };
                }

                if (clubBadgeCount > 0)
                {
                    ClubNavi.BindingContext = new { ClubCount = "" + clubBadgeCount };
                }
                else
                {
                    ClubNavi.BindingContext = new { ClubCount = "" };
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_BADGE) : {ex.Message}", "OK");
            }
        }
    }
}