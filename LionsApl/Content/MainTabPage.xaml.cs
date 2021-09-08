using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabPage : TabbedPage
    {
        public MainTabPage()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            var navigationPage = CurrentPage as NavigationPage;
            
            //タイトルバー背景・文字色
            navigationPage.BarBackgroundColor = Color.FromRgb(33, 150, 243);
            navigationPage.BarTextColor = Color.White;

            // ナビゲーションのルートに戻る
            navigationPage.PopToRootAsync();

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
    }
}