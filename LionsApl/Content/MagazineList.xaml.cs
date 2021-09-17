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
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<MagazineListRow> Items { get; set; }

        // Magazineピッカークラス
        public ObservableCollection<CMagazinePicker> _magazinePk = new ObservableCollection<CMagazinePicker>();

        public MagazineList()
        {
            InitializeComponent();

            // font-size
            this.magazine.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));             //地区誌購入
            this.MagazinePicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));      //地区誌名選択
            this.BuyNumberPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));     //冊子数選択
            this.count.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));               //冊

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 地区誌一覧取得
            GetMagazine();

            // 地区誌購入欄設定
            SetMagazineInfo();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMagazine()
        {
            string WorkDataNo = string.Empty;
            string WorkMagazine = string.Empty;
            int WorkMagazineDataNo;
            string WorkBuy = string.Empty;
            Items = new List<MagazineListRow>();

            Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.MAGAZINE_LIST row in _sqlite.Get_MAGAZINE_LIST("SELECT TM.*, " +
                                                                              "IFNULL(TMB.MagazineDataNo, 0) " +
                                                                              "FROM T_MAGAZINE TM " +
                                                                              "LEFT OUTER JOIN T_MAGAZINEBUY TMB " +
                                                                              "ON TM.DataNo = TMB.MagazineDataNo " +
                                                                              "ORDER BY TM.SortNo DESC"))
                {
                    WorkDataNo = row.DataNo.ToString();
                    WorkMagazine = Util.GetString(row.Magazine);
                    WorkMagazineDataNo = row.MagazineDataNo;
                    if(WorkMagazineDataNo != 0)
                    {
                        WorkBuy = "購入済み";
                    }
                    Items.Add(new MagazineListRow(WorkDataNo, WorkMagazine, WorkBuy));
                }
                MagazineListView.ItemsSource = Items;
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
            // 変数
            string wkDataNo = string.Empty;
            string wkMagazine = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            // データ取得
            try
            {
               
                // 初期化
                _magazinePk.Clear();

                // 購入可の地区誌を取得
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("SELECT * " +
                                                                        "FROM T_MAGAZINE " +
                                                                        "WHERE MagazineClass = '1' " +
                                                                        "ORDER BY DataNo" ))
                {
                    wkDataNo = row.DataNo.ToString();
                    wkMagazine = Util.GetString(row.Magazine) + " " + Util.GetInt(row.MagazinePrice).ToString() + "(円)";
                    _magazinePk.Add(new CMagazinePicker(wkDataNo, wkMagazine));
                }

                // MagazinePickerにCMagazinePickerクラスを設定する
                MagazinePicker.ItemsSource = _magazinePk;

                // 購入可能な地区誌がない場合は地区誌購入欄を非表示にする
                if(MagazinePicker.ItemsSource.Count == 0)
                {
                    MagazineBuy.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MAGAZINE) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            MagazineListRow item = e.Item as MagazineListRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.DataNo))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 地区誌画面へ
            Navigation.PushAsync(new MagazinePage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 地区誌 行クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class MagazineListRow
    {
        public MagazineListRow(string dataNo, string magazine, string magazineBuy)
        {
            DataNo = dataNo;
            Magazine = magazine;
            MagazineBuy = magazineBuy;
        }
        public string DataNo { get; set; }
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
        public CMagazinePicker(string dataNo, string name)
        {
            DataNo = dataNo;
            Name = name;
        }
        public string DataNo { get; set; }
        public string Name { get; set; }
        
    }

}