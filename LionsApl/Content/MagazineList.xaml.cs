using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 地区誌一覧画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MagazineList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 情報通信マネージャクラス
        private IComManager _icom;

        // T_MAGAZINEBUYテーブルクラス
        private Table.T_MAGAZINEBUY _magazineBuy;

        // T_MAGAZINEBUY登録クラス
        private CMAGAZINE _cmagazine;

        // リストビュー設定内容
        //public List<MagazineListRow> Items { get; set; }
        public ObservableCollection<MagazineListRow> Items;

        // Magazineピッカークラス
        public ObservableCollection<CMagazinePicker> _magazinePk = new ObservableCollection<CMagazinePicker>();

        // 文字列
        private string ST_MSGTITLE = "地区誌";
        private string ST_PURCHASED = "購入済";

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MagazineList()
        {
            InitializeComponent();

            // font-size
            this.magazine.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));             //地区誌購入
            this.MagazinePicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));      //地区誌名選択
            this.BuyNumberPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));     //冊子数選択
            this.count.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));               //冊

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // 情報通信マネージャー生成
            _icom = IComManager.GetInstance(_sqlite.dbFile);

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 地区誌購入クラス設定
            string emp = string.Empty;
            string membername = string.Empty;
            membername = _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;
            _cmagazine = new CMAGAZINE(0, emp, emp, 0, 0, 0,
                                       _sqlite.Db_A_Account.Region,
                                       _sqlite.Db_A_Account.Zone,
                                       _sqlite.Db_A_Account.ClubCode,
                                       _sqlite.Db_A_Account.ClubName,
                                       _sqlite.Db_A_Account.MemberCode,
                                       membername, membername, emp);

            // T_MAGAZINEBUYテーブルクラス設定
            _magazineBuy = new Table.T_MAGAZINEBUY();

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
            int WorkDataNo = 0;
            string WorkMagazine = string.Empty;
            int WorkMagazineDataNo;
            string WorkBuy = string.Empty;
            //Items = new List<MagazineListRow>();
            Items = new ObservableCollection<MagazineListRow>();

            try
            {
                foreach (Table.MAGAZINE_LIST row in _sqlite.Get_MAGAZINE_LIST("SELECT TM.*, " +
                                                                              "TMB.MagazineDataNo " +
                                                                              "FROM T_MAGAZINE TM " +
                                                                              "LEFT OUTER JOIN T_MAGAZINEBUY TMB " +
                                                                              "ON TM.DataNo = TMB.MagazineDataNo " +
                                                                              "GROUP BY TM.DataNo " +
                                                                              "ORDER BY TM.SortNo DESC "
                                                                              ))
                {
                    WorkDataNo = row.DataNo;
                    WorkMagazine = _utl.GetString(row.Magazine);
                    WorkMagazineDataNo = row.MagazineDataNo;
                    // 購入済チェック
                    if(WorkMagazineDataNo != 0)
                    {
                        // 文字列設定
                        WorkBuy = ST_PURCHASED;
                    }
                    Items.Add(new MagazineListRow(WorkDataNo, WorkMagazine, WorkBuy));
                }
                MagazineListView.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(MAGAZINE_LIST) : {ex.Message}", "OK");
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
            int wkDataNo = 0;
            int wkPrice = 0;
            string wkMagazine = string.Empty;
            string wkName = string.Empty;

            // データ取得
            try
            {
                // 選択項目クリア
                _cmagazine.Magazine = string.Empty;
                _cmagazine.BuyNumber = 0;
                _cmagazine.MagazinePrice = 0;

                // 初期化
                _magazinePk.Clear();

                // 購入可の地区誌を取得
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("SELECT * " +
                                                                        "FROM T_MAGAZINE " +
                                                                        "WHERE MagazineClass = '1' " +
                                                                        "ORDER BY DataNo" ))
                {
                    wkDataNo = row.DataNo;
                    wkPrice = row.MagazinePrice;
                    wkMagazine = _utl.GetString(row.Magazine);
                    wkName = wkMagazine + " " + wkPrice.ToString() + "(円)";
                    _magazinePk.Add(new CMagazinePicker(wkDataNo, wkName, wkMagazine, wkPrice));
                }

                // MagazinePickerにCMagazinePickerクラスを設定する
                MagazinePicker.ItemsSource = _magazinePk;


                // 購入可能な地区誌がない場合は地区誌購入欄を非表示にする
                if (MagazinePicker.ItemsSource.Count == 0)
                {
                    MagazineBuy.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MAGAZINE) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リストタップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            MagazineListRow item = e.Item as MagazineListRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo.Equals(0))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 地区誌画面へ
            if (Device.RuntimePlatform == Device.iOS)
            {
                Navigation.PushAsync(new MagazinePage(item.DataNo));
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                Navigation.PushAsync(new MagazinePageAndroid(item.DataNo));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void MagazinePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = MagazinePicker.SelectedItem as CMagazinePicker;
            if (item != null)
            {
                // 購入情報設定
                _cmagazine.MagazineDataNo = item.DataNo;
                _cmagazine.Magazine = item.Magazine;
                _cmagazine.MagazinePrice = item.Price;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 冊数選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void BuyNumberPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = BuyNumberPicker.SelectedItem;
            if (item != null)
            {
                // 購入情報設定
                _cmagazine.BuyNumber = int.Parse(item.ToString());
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 購入ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Buy_Button_Clicked(object sender, System.EventArgs e)
        {
            bool answer = false;

            var item1 = MagazinePicker.SelectedItem as CMagazinePicker;
            if (item1 != null)
            {
                // 購入情報設定
                _cmagazine.MagazineDataNo = item1.DataNo;
                _cmagazine.Magazine = item1.Magazine;
                _cmagazine.MagazinePrice = item1.Price;
            }
            var item2 = BuyNumberPicker.SelectedItem as CMagazinePicker;
            if (item2 != null)
            {
                _cmagazine.BuyNumber = int.Parse(item2.ToString());
            }

            // 選択していない場合はエラーメッセージを表示
            if (_cmagazine.Magazine.Equals(string.Empty))
            {
                await DisplayAlert(ST_MSGTITLE, "購入する地区誌を選択してください。", "OK");
                return;
            }

            // 冊数を入力していない場合はエラーメッセージを表示
            if (_cmagazine.BuyNumber.Equals(0))
            {
                await DisplayAlert(ST_MSGTITLE, "冊数を入力して下さい。", "OK");
                return;
            }

            _cmagazine.MoneyTotal = _cmagazine.MagazinePrice * _cmagazine.BuyNumber;

            // 購入確認メッセージ
            answer = await DisplayAlert(ST_MSGTITLE, $"{_cmagazine.Magazine}を{Environment.NewLine}" +
                                                     $"{_cmagazine.BuyNumber}冊購入しますか？{Environment.NewLine}" +
                                                     $"合計は{_cmagazine.MoneyTotal}円です。", 
                                                     "はい", "いいえ");
            // メッセージ結果判定
            if (answer)
            {
                // 地区誌購入情報登録（SQLServer＆SQLite）
                RegMagazine();
                // メッセージ表示
                await DisplayAlert(ST_MSGTITLE, $"{_cmagazine.Magazine}を購入しました", "OK");
                UpdateMagazineList();
            }
            else
            {
                await DisplayAlert(ST_MSGTITLE, $"キャンセルしました", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌購入情報登録（SQLServer＆SQLite）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void RegMagazine()
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            // 処理日設定
            _cmagazine.BuyDate = nowDt.ToString();
            _cmagazine.EditDate = nowDt.ToString();

            // 出欠情報をコンテンツに設定
            _icom.SetContentToMAGAZINE(_cmagazine);
            try
            {
                // SQLServerへ登録
                Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLServer 地区誌購入情報登録エラー : {ex.Message}", "OK");
                throw;
            }

            try
            {
                // SQLiteへ登録
                SetMagazineSQlite();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite 地区誌購入情報登録エラー : {ex.Message}", "OK");
                throw;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌購入情報登録（SQLite）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetMagazineSQlite()
        {
            _magazineBuy.DataNo = 0;
            _magazineBuy.MagazineDataNo = _cmagazine.MagazineDataNo;
            _magazineBuy.Magazine = _cmagazine.Magazine;
            _magazineBuy.BuyDate = _cmagazine.BuyDate;
            _magazineBuy.BuyNumber = _cmagazine.BuyNumber;
            _magazineBuy.MagazinePrice = _cmagazine.MagazinePrice;
            _magazineBuy.MoneyTotal = _cmagazine.MoneyTotal;
            _magazineBuy.Region = _cmagazine.Region;
            _magazineBuy.Zone = _cmagazine.Zone;
            _magazineBuy.ClubCode = _cmagazine.ClubCode;
            _magazineBuy.ClubNameShort = _cmagazine.ClubNameShort;
            _magazineBuy.MemberCode = _cmagazine.MemberCode;
            _magazineBuy.MemberName = _cmagazine.MemberName;
            _magazineBuy.ShippingDate = string.Empty;
            _magazineBuy.PaymentDate = string.Empty;
            _magazineBuy.Payment = 0;
            _magazineBuy.DelFlg = string.Empty;

            _sqlite.Set_T_MAGAZINEBUY(_magazineBuy);
        }


        private void UpdateMagazineList()
        {
            foreach(var item in Items)
            {
                if(item.DataNo.Equals(_cmagazine.MagazineDataNo))
                {
                    item.MagazineBuy = ST_PURCHASED;
                }
            }
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 地区誌 行クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class MagazineListRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _dataNo = 0;
        private string _magazine = string.Empty;
        private string _magazineBuy = string.Empty;

        public MagazineListRow(int dataNo, string magazine, string magazineBuy)
        {
            DataNo = dataNo;
            Magazine = magazine;
            MagazineBuy = magazineBuy;
        }

        public int DataNo
        {
            get { return _dataNo; }
            set
            {
                if (_dataNo != value)
                {
                    _dataNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(DataNo)));
                    }
                }
            }
        }
        public string Magazine
        {
            get { return _magazine; }
            set
            {
                if (_magazine != value)
                {
                    _magazine = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Magazine)));
                    }
                }
            }
        }
        public string MagazineBuy
        {
            get { return _magazineBuy; }
            set
            {
                if (_magazineBuy != value)
                {
                    _magazineBuy = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(MagazineBuy)));

                    }
                }
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Magazineピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CMagazinePicker
    {
        public CMagazinePicker(int dataNo, string name, string magazine, int price)
        {
            DataNo = dataNo;
            Name = name;
            Magazine = magazine;
            Price = price;
        }
        public int DataNo { get; set; }
        public string Name { get; set; }
        public string Magazine { get; set; }
        public int Price { get; set; }
        
    }

}