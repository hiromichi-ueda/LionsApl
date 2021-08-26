using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// https://anderson02.com/cs/xamarin1/xamarin-21/

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubTop : ContentPage
    {
        public ClubTop()
        {
            InitializeComponent();


            List<ClubTopSlogan> sloganItems = new List<ClubTopSlogan>
            {
                new ClubTopSlogan("「温かき心を以って たがいに手を繋ごう」",
                                      "会長 L.山田 太郎")
            };
            ClubTopListViewSlogan.ItemsSource = sloganItems;
            //ClubTopTableViewSlogan.BindingContext = sloganItems;
            ClubTopLabelClubSlogun.Text = "「温かき心を以って たがいに手を繋ごう」";
            ClubTopLabelExecutiveName.Text = "会長 L.山田 太郎";

            List<ClubTopMenu> menuItems = new List<ClubTopMenu>
            {
                new ClubTopMenu("年間例会スケジュール"),
                new ClubTopMenu("理事・委員会"),
                new ClubTopMenu("例会プログラム"),
                new ClubTopMenu("連絡事項"),
                new ClubTopMenu("会員情報")
            };
            ClubTopListViewMenu.ItemsSource = menuItems;
        }

        private void ClubTopViewSlogan_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) { return; }

            this.ClubTopListViewSlogan.SelectedItem = null;

        }
        

        private void ClubTopViewMenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ClubTopMenu;
            if (item.Title == "年間例会スケジュール")
            {
                Navigation.PushAsync(new HomeTop());
            }
        }

        private void OnLabelClickMenu1(object sender, EventArgs args)
        {
            //Navigation.PushAsync(new AccountTop());
        }
    }

    public sealed class ClubTopSlogan
    { 
        public ClubTopSlogan(string clubSlogan, string executiveName)
        {
            ClubSlogan = clubSlogan;
            ExecutiveName = executiveName;
        }
        public string ClubSlogan { get; set; }
        public string ExecutiveName { get; set; }
    }

    public sealed class ClubTopMenu
    {
        public ClubTopMenu(string title)
        {
            Title = title;
        }
        public string Title { get; set; }
    }
}