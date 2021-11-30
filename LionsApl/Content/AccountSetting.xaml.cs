using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// アカウント設定画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountSetting : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 情報通信マネージャクラス
        private IComManager _icom;

        // A_ACCOUNTテーブルクラス
        private Table.A_ACCOUNT _account;

        // T_MEMBER更新用クラス
        private CACCOUNTREG _caccountreg;

        // ピッカー用変数
        public ObservableCollection<CRegionPicker> _regionPk = new ObservableCollection<CRegionPicker>();
        public ObservableCollection<CZonePicker> _zonePk = new ObservableCollection<CZonePicker>();
        public ObservableCollection<CClubPicker> _clubPk = new ObservableCollection<CClubPicker>();
        public ObservableCollection<CMemberPicker> _memberPk = new ObservableCollection<CMemberPicker>();

        private bool _pickerSelect = false;
        private string _selRegion = null;
        private string _selZone = null;
        private string _selClub = null;
        private string _selMember = null;


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public AccountSetting()
        {
            InitializeComponent();

            // font-size
            this.lbl_region.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.RegionPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
            this.lbl_zone.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.ZonePicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
            this.lbl_club.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.ClubPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
            this.lbl_member.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MemberPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
            

            // ピッカーセレクト処理OFF
            _pickerSelect = false;

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // 情報通信マネージャークラス生成
            _icom = IComManager.GetInstance(_sqlite.dbFile);

            // 選択リジョン情報クリア
            ClearSelectRegionInfo();
            // 選択ゾーン情報クリア
            ClearSelectZoneInfo();
            // 選択クラブ情報クリア
            ClearSelectClubInfo();
            // 選択会員情報クリア
            ClearSelectMemberInfo();

            // アカウント情報取得
            GetAccountInfo();
            if (_account == null)
            {
                // アカウント情報が取得できなかった場合
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
                SetRegionInfo();
                // ゾーン情報取得
                SetZoneInfo(_selRegion);
                // クラブ情報取得
                SetClubInfo(_selRegion, _selZone);
                // 会員情報取得
                SetMemberInfo(_selRegion, _selZone, _selClub);

                // リジョンインデックス設定
                RegionPicker.SelectedIndex = GetRegionIndex(_selRegion);
                // ゾーンインデックス設定
                ZonePicker.SelectedIndex = GetZoneIndex(_selZone);
                // クラブインデックス設定
                ClubPicker.SelectedIndex = GetClubIndex(_selClub);
                // 会員インデックス設定
                MemberPicker.SelectedIndex = GetMemberIndex(_selMember);

            }
            // ピッカーセレクト処理ON
            _pickerSelect = true;

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

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(リジョン) : {ex.Message}", "OK");
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
                DisplayAlert("Alert", $"Picker検索エラー(リジョン) : {ex.Message}", "OK");
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

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(ゾーン) : {ex.Message}", "OK");
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

            // インデックス取得
            try
            {
                foreach (CZonePicker item in ZonePicker.ItemsSource)
                {
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
                DisplayAlert("Alert", $"Picker検索エラー(ゾーン) : {ex.Message}", "OK");
            }

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

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(クラブ) : {ex.Message}", "OK");
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
                DisplayAlert("Alert", $"Picker検索エラー(クラブ) : {ex.Message}", "OK");
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

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(会員) : {ex.Message}", "OK");
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
                DisplayAlert("Alert", $"Picker検索エラー(会員) : {ex.Message}", "OK");
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
            // HOME対象テーブル削除
            _sqlite.DropTable_Home();
            // HOME対象テーブル作成
            _sqlite.CreateTable_Home();

            Application.Current.MainPage = new TopMenu();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報登録ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Button_AccountSet_Clicked(object sender, System.EventArgs e)
        {
            if (_selRegion == null ||
                _selZone == null ||
                _selClub == null ||
                _selMember == null)
            {
                await DisplayAlert("アカウント設定", "選択していない項目があります。", "OK");
            }
            else
            {
                // アカウント情報登録
                SetAccountInfo();
                // アカウント設定日をDBへ登録する
                RegAccountDate();
                await DisplayAlert("アカウント設定", "アカウントを登録しました。", "OK");

                // TOP画面に遷移する
                Application.Current.MainPage = new TopMenu();

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
                        Region = _utl.GetString(row.Region),
                        Zone = _utl.GetString(row.Zone),
                        ClubCode = _utl.GetString(row.ClubCode),
                        ClubName = _utl.GetString(row.ClubName),
                        MemberCode = _utl.GetString(row.MemberCode),
                        MemberFirstName = _utl.GetString(row.MemberFirstName),
                        MemberLastName = _utl.GetString(row.MemberLastName),
                        AccountDate = _utl.GetString(row.AccountDate)
                    };

                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(アカウント情報) : {ex.Message}", "OK");
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
                // 処理日時取得
                DateTime nowDt = DateTime.Now;
                _account.AccountDate = nowDt.ToString("yyyy/MM/dd HH:mm:ss");
                // アプリケーションバージョン
                _account.VersionNo = ((App)Application.Current).AppVersion;
                // アカウント情報追加
                _sqlite.Set_A_ACCOUNT(_account);
                // HOME対象テーブル削除
                _sqlite.DropTable_Home();
                // HOME対象テーブル作成
                _sqlite.CreateTable_Home();

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite登録エラー(アカウント情報) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント設定日をDBへ登録する
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void RegAccountDate()
        {
            // アカウント設定登録情報クラス生成
            _caccountreg = new CACCOUNTREG(_account.AccountDate,
                                           _account.Region,
                                           _account.Zone,
                                           _account.ClubCode,
                                           _account.MemberCode);

            // アカウント設定日情報をコンテンツに設定
            _icom.SetContentToACCOUNTREG(_caccountreg);
            try
            {
                // SQLServerへ登録
                Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLServer アカウント設定登録エラー : {ex.Message}", "OK");
                throw;
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