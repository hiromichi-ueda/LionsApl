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
    public partial class AccountSetting : ContentPage
    {
        public ObservableCollection<CRegionPicker> _regionPk = new ObservableCollection<CRegionPicker>();
        public ObservableCollection<CZonePicker> _zonePk = new ObservableCollection<CZonePicker>();
        public ObservableCollection<CClubPicker> _clubPk = new ObservableCollection<CClubPicker>();
        public ObservableCollection<CMemberPicker> _memberPk = new ObservableCollection<CMemberPicker>();

        private bool _pickerSelect = false;
        private string _selRegion = null;
        private string _selZone = null;
        private string _selClub = null;
        private string _selMember = null;
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス
        private Table.A_ACCOUNT _account;                   // A_ACCOUNTテーブルクラス

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public AccountSetting()
        {
            InitializeComponent();

            //DisplayAlert("Alert", "コンストラクタ開始", "OK");

            // ピッカーセレクト処理OFF
            _pickerSelect = false;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();
            // 選択リジョン情報クリア
            ClearSelectRegionInfo();
            // 選択ゾーン情報クリア
            ClearSelectZoneInfo();
            // 選択クラブ情報クリア
            ClearSelectClubInfo();
            // 選択会員情報クリア
            ClearSelectMemberInfo();

            // アカウント情報取得
            //DisplayAlert("Alert", "アカウント情報取得", "OK");
            GetAccountInfo();
            if (_account == null)
            {
                // アカウント情報が取得できなかった場合
                //DisplayAlert("Alert", "アカウント情報なし", "OK");
                // リジョン情報のみSQLiteから取得する。
                // リジョン情報取得
                SetRegionInfo();
                ZonePicker.ItemsSource = null;
                _account = new Table.A_ACCOUNT();

            }
            else
            {
                // アカウント情報が取得できた場合
                //DisplayAlert("Alert", "アカウント情報あり", "OK");
                // １．リジョン情報～会員情報を全て取得
                // ２．各ピッカーの値とアカウント情報の値を比較してピッカーのインデックスを取得
                // ３．ピッカーにインデックスを設定

                _selRegion = _account.Region;
                _selZone = _account.Zone;
                _selClub = _account.ClubCode;
                _selMember = _account.MemberCode;

                // リジョン情報取得
                //DisplayAlert("Alert", "リジョン情報取得", "OK");
                SetRegionInfo();
                // ゾーン情報取得
                //DisplayAlert("Alert", $"ゾーン情報取得 Region:{_selRegion}", "OK");
                SetZoneInfo(_selRegion);
                // クラブ情報取得
                //DisplayAlert("Alert", $"クラブ情報取得 Zone:{_selZone}", "OK");
                SetClubInfo(_selRegion, _selZone);
                // 会員情報取得
                //DisplayAlert("Alert", $"会員情報取得 Club:{_selClub}", "OK");
                SetMemberInfo(_selRegion, _selZone, _selClub);

                // リジョンインデックス設定
                //DisplayAlert("Alert", $"リジョンインデックス設定 Region:{_selRegion}", "OK");
                RegionPicker.SelectedIndex = GetRegionIndex(_selRegion);
                // ゾーンインデックス設定
                //DisplayAlert("Alert", $"ゾーンインデックス設定 Zone:{_selZone}", "OK");
                ZonePicker.SelectedIndex = GetZoneIndex(_selZone);
                // クラブインデックス設定
                //DisplayAlert("Alert", $"クラブインデックス設定 Club:{_selClub}", "OK");
                ClubPicker.SelectedIndex = GetClubIndex(_selClub);
                // 会員インデックス設定
                //DisplayAlert("Alert", $"会員インデックス設定 Club:{_selMember}", "OK");
                MemberPicker.SelectedIndex = GetMemberIndex(_selMember);

            }
            // ピッカーセレクト処理ON
            _pickerSelect = true;

            //DisplayAlert("Alert", "コンストラクタ終了", "OK");

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リジョン情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetRegionInfo()
        {
            // データ取得
            try
            {
                string strRegion;
                _regionPk.Clear();
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER("Select DISTINCT Region From M_MEMBER ORDER BY Region"))
                {
                    strRegion = $"{row.Region}R";
                    _regionPk.Add(new CRegionPicker(row.Region, strRegion));
                }
                // RegionPickerにCRegionPickerクラスを設定する
                RegionPicker.ItemsSource = _regionPk;

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    String strRegion;
                //    foreach (var row in db.Query<M_MEMBER>("Select DISTINCT Region From M_MEMBER ORDER BY Region"))
                //    {
                //        // CRegionPickerクラスに値を設定する
                //        strRegion = $"{row.Region}R";
                //        _regionPk.Add(new CRegionPicker(row.Region, strRegion));
                //    }
                //    // RegionPickerにCRegionPickerクラスを設定する
                //    RegionPicker.ItemsSource = _regionPk;
                //    // インデックスに0を設定する
                //    // RegionPicker.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(リジョン) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リジョン情報から対象のインデックスを取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int GetRegionIndex(string chkRegion)
        {
            int idx = 0;
            // インデックス取得
            try
            {
                foreach (CRegionPicker item in RegionPicker.ItemsSource)
                {
                    // 値をチェックする
                    if (item.Region.Equals(chkRegion))
                    {
                        break;
                    }
                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"Picker検索エラー(リジョン) : &{ex.Message}", "OK");
            }
            return idx;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リジョン選択情報クリア
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ClearSelectRegionInfo()
        {
            RegionPicker.ItemsSource = null;
            RegionPicker.SelectedIndex = -1;
            _regionPk.Clear();
            _selRegion = null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ゾーン情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetZoneInfo(string region)
        {
            // データ取得
            try
            {
                string strZone;
                _zonePk.Clear();
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER($"Select DISTINCT Zone From M_MEMBER Where Region={region} ORDER BY Zone"))
                {
                    // CZonePickerクラスに値を設定する
                    strZone = $"{row.Zone}Z";
                    _zonePk.Add(new CZonePicker(row.Zone, strZone));
                }
                // ZonePickerにCRegionPickerクラスを設定する
                ZonePicker.ItemsSource = _zonePk;

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    string strZone;
                //    ZonePicker.ItemsSource = null;
                //    _zonePk.Clear();
                //    foreach (var row in db.Query<M_MEMBER>($"Select DISTINCT Zone From M_MEMBER Where Region={region} ORDER BY Zone"))
                //    {
                //        // CZonePickerクラスに値を設定する
                //        strZone = $"{row.Zone}Z";
                //        _zonePk.Add(new CZonePicker(row.Zone, strZone));
                //    }
                //    // ZonePickerにCRegionPickerクラスを設定する
                //    ZonePicker.ItemsSource = _zonePk;
                //    // インデックスに0を設定する
                //    //ZonePicker.SelectedIndex = 0;

                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(ゾーン) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ゾーン情報から対象のインデックスを取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int GetZoneIndex(string chkZone)
        {
            int idx = 0;

            //DisplayAlert("Alert", $"chkZone:{chkZone}", "OK");
            // インデックス取得
            try
            {
                foreach (CZonePicker item in ZonePicker.ItemsSource)
                {
                    //DisplayAlert("Alert", $"idx:{idx}\r\nitemZone:{item.Zone}\r\nAccuntZone:{chkZone}", "OK");
                    // 値をチェックする
                    if (item.Zone.Equals(chkZone))
                    {
                        break;
                    }
                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"Picker検索エラー(ゾーン) : &{ex.Message}", "OK");
            }
            //DisplayAlert("Alert", $"last idx:{idx}", "OK");
            return idx;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ゾーン選択情報クリア
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ClearSelectZoneInfo()
        {
            ZonePicker.ItemsSource = null;
            ZonePicker.SelectedIndex = -1;
            _zonePk.Clear();
            _selZone = null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// クラブ情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetClubInfo(string region, string zone)
        {
            // データ取得
            try
            {
                _clubPk.Clear();
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER($"Select DISTINCT ClubCode, ClubNameShort From M_MEMBER Where Region={region} and Zone={zone} ORDER BY ClubCode"))
                {
                    // CClubPickerクラスに値を設定する
                    _clubPk.Add(new CClubPicker(row.ClubCode, row.ClubNameShort));
                }
                // ClubPickerにCRegionPickerクラスを設定する
                ClubPicker.ItemsSource = _clubPk;

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    ClubPicker.ItemsSource = null;
                //    _clubPk.Clear();
                //    foreach (var row in db.Query<M_MEMBER>($"Select DISTINCT ClubCode, ClubNameShort From M_MEMBER Where Region={region} and Zone={zone} ORDER BY ClubCode"))
                //    {
                //        // CClubPickerクラスに値を設定する
                //        _clubPk.Add(new CClubPicker(row.ClubCode, row.ClubNameShort));
                //    }
                //    // ClubPickerにCRegionPickerクラスを設定する
                //    ClubPicker.ItemsSource = _clubPk;
                //    // インデックスに0を設定する
                //    //ClubPicker.SelectedIndex = 0;

                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(クラブ) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// クラブ情報から対象のインデックスを取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int GetClubIndex(string chkClub)
        {
            int idx = 0;
            // インデックス取得
            try
            {
                foreach (CClubPicker item in ClubPicker.ItemsSource)
                {
                    // 値をチェックする
                    if (item.ClubCode.Equals(chkClub))
                    {
                        break;
                    }
                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"Picker検索エラー(クラブ) : &{ex.Message}", "OK");
            }
            return idx;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// クラブ選択情報クリア
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ClearSelectClubInfo()
        {
            ClubPicker.ItemsSource = null;
            ClubPicker.SelectedIndex = -1;
            _clubPk.Clear();
            _selClub = null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetMemberInfo(string region, string zone, string clubcode)
        {
            // データ取得
            try
            {
                string strMember;
                _memberPk.Clear();
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER($"Select DISTINCT MemberCode, MemberFirstName, MemberLastName, MemberNameKana From M_MEMBER Where Region={region} and Zone={zone} and ClubCode={clubcode}  ORDER BY MemberNameKana"))
                {
                    strMember = $"{row.MemberFirstName}{row.MemberLastName}";
                    // CClubPickerクラスに値を設定する
                    _memberPk.Add(new CMemberPicker(row.MemberCode, strMember, row.MemberFirstName, row.MemberLastName, row.MemberNameKana));
                }
                // MemberPickerにCRegionPickerクラスを設定する
                MemberPicker.ItemsSource = _memberPk;

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    MemberPicker.ItemsSource = null;
                //    _memberPk.Clear();
                //    String strMember;
                //    foreach (var row in db.Query<M_MEMBER>($"Select DISTINCT MemberCode, MemberFirstName, MemberLastName, MemberNameKana From M_MEMBER Where Region={region} and Zone={zone} and ClubCode={clubcode}  ORDER BY MemberNameKana"))
                //    {
                //        strMember = $"{row.MemberFirstName}{row.MemberLastName}";
                //        // CClubPickerクラスに値を設定する
                //        _memberPk.Add(new CMemberPicker(row.MemberCode, strMember, row.MemberFirstName, row.MemberLastName, row.MemberNameKana));
                //    }
                //    // MemberPickerにCRegionPickerクラスを設定する
                //    MemberPicker.ItemsSource = _memberPk;
                //    // インデックスに0を設定する
                //    //MemberPicker.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(会員) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員情報から対象のインデックスを取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int GetMemberIndex(string chkMember)
        {
            int idx = 0;
            // インデックス取得
            try
            {
                foreach (CMemberPicker item in MemberPicker.ItemsSource)
                {
                    // 値をチェックする
                    if (item.MemberCode.Equals(chkMember))
                    {
                        break;
                    }
                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"Picker検索エラー(会員) : &{ex.Message}", "OK");
            }
            return idx;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員選択情報クリア
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ClearSelectMemberInfo()
        {
            MemberPicker.ItemsSource = null;
            MemberPicker.SelectedIndex = -1;
            _memberPk.Clear();
            _selMember = null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リジョン選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void RegionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_pickerSelect == false)
            {
                // ピッカー選択処理がOFFの場合
                return;
            }
            //DisplayAlert("Alert", $"リジョンピッカー選択", "OK");

            var item = RegionPicker.SelectedItem as CRegionPicker;
            if (item != null)
            {
                //String disp = $"{item.Region.ToString()}:{item.Name.ToString()}";
                //DisplayAlert("Region", disp, "OK");
                // 選択リジョン情報設定
                _selRegion = item.Region.ToString();
                _account.Region = item.Region.ToString();

                // 選択ゾーン情報クリア
                ClearSelectZoneInfo();
                // 選択クラブ情報クリア
                ClearSelectClubInfo();
                // 選択会員情報クリア
                ClearSelectMemberInfo();

                // ゾーン情報設定
                SetZoneInfo(_selRegion);

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ゾーン選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ZonePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_pickerSelect == false)
            {
                // ピッカー選択処理がOFFの場合
                return;
            }
            //DisplayAlert("Alert", $"ゾーンピッカー選択", "OK");

            var item = ZonePicker.SelectedItem as CZonePicker;
            if (item != null)
            {
                // 選択ゾーン情報設定
                _selZone = item.Zone.ToString();
                _account.Zone = item.Zone.ToString();

                // 選択クラブ情報クリア
                ClearSelectClubInfo();
                // 選択会員情報クリア
                ClearSelectMemberInfo();

                // クラブ情報設定
                SetClubInfo(_selRegion, _selZone);

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// クラブ選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ClubPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_pickerSelect == false)
            {
                // ピッカー選択処理がOFFの場合
                return;
            }

            //DisplayAlert("Alert", $"クラブピッカー選択", "OK");
            var item = ClubPicker.SelectedItem as CClubPicker;
            if (item != null)
            {
                // 選択クラブ情報設定
                _selClub = item.ClubCode.ToString();
                _account.ClubCode = item.ClubCode.ToString();
                _account.ClubName = item.Name.ToString();

                // 選択会員情報クリア
                ClearSelectMemberInfo();

                // 会員情報設定
                SetMemberInfo(_selRegion, _selZone, _selClub);

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void MemberPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_pickerSelect == false)
            {
                // ピッカー選択処理がOFFの場合
                return;
            }

            //DisplayAlert("Alert", $"会員ピッカー選択", "OK");
            var item = MemberPicker.SelectedItem as CMemberPicker;
            if (item != null)
            {
                // 選択会員情報設定
                _selMember = item.MemberCode.ToString();
                _account.MemberCode = item.MemberCode.ToString();
                _account.MemberFirstName = item.MemberFirstName.ToString();
                _account.MemberLastName = item.MemberLastName.ToString();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 戻るボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Back_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報登録ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_AccountSet_Clicked(object sender, System.EventArgs e)
        {
            if (_selRegion == null ||
                _selZone == null ||
                _selClub == null ||
                _selMember == null)
            {
                DisplayAlert("アカウント設定", "選択していない項目があります。", "OK");
            }
            else
            {
                // データ登録
                SetAccountInfo();
                DisplayAlert("アカウント設定", "アカウントを登録しました。", "OK");

                Application.Current.MainPage = new MainPage();

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetAccountInfo()
        {
            _account = null;

            // データ取得
            try
            {
                foreach (Table.A_ACCOUNT row in _sqlite.Get_A_ACCOUNT("SELECT * FROM A_ACCOUNT"))
                {
                    _account = new Table.A_ACCOUNT
                    {
                        Region = row.Region,
                        Zone = row.Zone,
                        ClubCode = row.ClubCode,
                        ClubName = row.ClubName,
                        MemberCode = row.MemberCode,
                        MemberFirstName = row.MemberFirstName,
                        MemberLastName = row.MemberLastName,
                        AccountDate = row.AccountDate
                    };

                }

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    foreach (var row in db.Query<A_ACCOUNT>("SELECT * FROM A_ACCOUNT"))
                //    {
                //        //DisplayAlert("アカウント設定値", 
                //        //    $"Region:{row.Region}\r\n" + 
                //        //    $"Zone:{row.Zone}\r\n" +
                //        //    $"ClubCode:{row.ClubCode}\r\n" +
                //        //    $"ClubName:{row.ClubName}\r\n" +
                //        //    $"MemberCode:{row.MemberCode}\r\n" +
                //        //    $"MemberFirstName:{row.MemberFirstName}\r\n" +
                //        //    $"MemberLastName:{row.MemberLastName}\r\n" +
                //        //    $"AccountDate:{row.AccountDate}",
                //        //    "OK");

                //        _account = new A_ACCOUNT
                //        {
                //            Region = row.Region,
                //            Zone = row.Zone,
                //            ClubCode = row.ClubCode,
                //            ClubName = row.ClubName,
                //            MemberCode = row.MemberCode,
                //            MemberFirstName = row.MemberFirstName,
                //            MemberLastName = row.MemberLastName,
                //            AccountDate = row.AccountDate
                //        };

                //    }
                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(アカウント情報) : &{ex.Message}", "OK");
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報登録
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetAccountInfo()
        {
            // データ登録
            try
            {
                _account.AccountDate = DateTime.Now;
                _sqlite.Set_A_ACCOUNT(_account);

                //using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                //{
                //    //db.Query<A_ACCOUNT>("DELETE FROM A_ACCOUNT");
                //    db.DropTable<A_ACCOUNT>();
                //    db.CreateTable<A_ACCOUNT>();
                //    db.Insert(new A_ACCOUNT()
                //    {
                //        Region = _account.Region,
                //        Zone = _account.Zone,
                //        ClubCode = _account.ClubCode,
                //        ClubName = _account.ClubName,
                //        MemberCode = _account.MemberCode,
                //        MemberFirstName = _account.MemberFirstName,
                //        MemberLastName = _account.MemberLastName,
                //        AccountDate = now
                //    });
                //}
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite登録エラー(アカウント情報) : &{ex.Message}", "OK");
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Regionピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CRegionPicker
    {
        public string Region { get; set; }
        public string Name { get; set; }
        public CRegionPicker(string region, string name)
        {
            Region = region;
            Name = name;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Zoneピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CZonePicker
    {
        public string Zone { get; set; }
        public string Name { get; set; }
        public CZonePicker(string zone, string name)
        {
            Zone = zone;
            Name = name;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Clubピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CClubPicker
    {
        public string ClubCode { get; set; }
        public string Name { get; set; }
        public CClubPicker(string clubcode, string name)
        {
            ClubCode = clubcode;
            Name = name;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Memberピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CMemberPicker
    {
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberNameKana { get; set; }
        public CMemberPicker(string membercode, string name, string memberfirstname, string memberlastname, string membernamekana)
        {
            MemberCode = membercode;
            Name = name;
            MemberFirstName = memberfirstname;
            MemberLastName = memberlastname;
            MemberNameKana = membernamekana;
        }
    }

}