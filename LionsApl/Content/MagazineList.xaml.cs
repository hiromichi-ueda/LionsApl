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

        public ObservableCollection<CMagazinePicker> _magazinePk = new ObservableCollection<CMagazinePicker>();

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

            SetMagazineInfo();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            MagazineListRow item = e.Item as MagazineListRow;

            //Navigation.PushAsync(new MagazinePage(item.Title, item.DataNo));

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌情報をSQLiteファイルから取得して画面に設定する。
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
                MagazineListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(MAGAZINE_LIST) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetMagazineInfo()
        {
            // データ取得
            try
            {
                string strMagazine;
                _magazinePk.Clear();
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("SELECT * " +
                                                                        "FROM T_MAGAZINE " +
                                                                        "ORDER BY DataNo" ))
                {
                    strMagazine = row.Magazine + " " + row.MagazinePrice.ToString() + "(円)";
                    _magazinePk.Add(new CMagazinePicker(row.DataNo, strMagazine));
                }
                // RegionPickerにCRegionPickerクラスを設定する
                MagazinePicker.ItemsSource = _magazinePk;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MAGAZINE) : &{ex.Message}", "OK");
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

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Magazineピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CMagazinePicker
    {
        public int DataNo { get; set; }
        public string Name { get; set; }
        public CMagazinePicker(int dataNo, string name)
        {
            DataNo = dataNo;
            Name = name;
        }
    }

}