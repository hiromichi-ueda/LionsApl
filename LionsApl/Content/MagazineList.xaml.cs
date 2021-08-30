using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MagazineList : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

        public MagazineList()
        {
            InitializeComponent();

            //Items = new ObservableCollection<string>
            //{
            //    "Item 1",
            //    "Item 2",
            //    "Item 3",
            //    "Item 4",
            //    "Item 5"
            //};

            //MyListView.ItemsSource = Items;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            GetMagazine();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            LetterRow item = e.Item as LetterRow;

            Navigation.PushAsync(new LetterPage(item.Title, item.DataNo));

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMagazine()
        {
            int WorkDataNo;
            string WorkMagazine;
            int WorkMagazineDataNo;
            string WorkBuy = string.Empty;
            List<MagazineListRow> items = new List<MagazineListRow>();

            try
            {
                foreach (Table.MAGAZINE_LIST row in _sqlite.Get_MAGAZINE_LIST("SELECT TM.*, " +
                                                                                     "IFNULL(TMB.MagazineDataNo, 0) " +
                                                                              "FROM T_MAGAZINE TM " +
                                                                              "LEFT OUTER JOIN T_MAGAZINEBUY TMB " +
                                                                              "ON TM.DataNo = TMB.MagazineDataNo " +
                                                                              "ORDER BY TM.SortNo DESC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkMagazine = row.Magazine;
                    WorkMagazineDataNo = row.MagazineDataNo;
                    if(WorkMagazineDataNo != 0)
                    {
                        WorkBuy = "購入済み";
                    }
                    items.Add(new MagazineListRow(WorkDataNo, WorkMagazine, WorkBuy));
                }
                LetterListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(MAGAZINE_LIST) : &{ex.Message}", "OK");
            }
        }
    }

    public sealed class MagazineListRow
    {
        public MagazineListRow(int dataNo, string magazine, string magazineBuy)
        {
            DataNo = dataNo;
            Magazine = magazine;
            MagazineBuy = magazineBuy;
        }
        public int DataNo { get; set; }
        public string Magazine { get; set; }
        public string MagazineBuy { get; set; }
    }

}