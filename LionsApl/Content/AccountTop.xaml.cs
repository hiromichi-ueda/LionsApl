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
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// アカウントTOP画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountTop : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 変数
        private string wkDistrictName;          //地区役員
        private string wkExecutiveName;         //クラブ執行部
        private string wkCommitteeName;         //クラブ委員会
        private string wkCommitteeFlg;          //委員長・副委員長
        private string wkClubDistrictName;      //クラブ役員

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public AccountTop()
        {
            InitializeComponent();

            // font-size
            this.lbl_Region.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Region.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Zone.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Zone.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_ClubName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.ClubName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MemberNo.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MemberNo.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MemberName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_TypeName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.TypeName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_JoinDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.JoinDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Sex.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Sex.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_cabnet.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Cabinet.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // アカウント情報データ取得
            GetAccount();

        }

        //---------------------------------------
        // アカウント情報を画面セット
        //---------------------------------------
        private void GetAccount()
        {

            // 変数宣言
            string wkMemberNo = string.Empty;              //会員番号
            string wkMemberName = string.Empty;            //会員名設定
            
            //Table.TableUtil Util = new Table.TableUtil();

            // 編集ボタン無効化
            EditButton.IsVisible = false;

            // 会員番号、会員名設定
            wkMemberNo = _sqlite.Db_A_Account.MemberCode;
            wkMemberName = _sqlite.Db_A_Account.MemberFirstName + " " + _sqlite.Db_A_Account.MemberLastName;

            // アカウント情報設定
            Region.Text = _sqlite.Db_A_Account.Region + "R";          //リジョン
            Zone.Text = _sqlite.Db_A_Account.Zone + "Z";              //ゾーン
            ClubName.Text = _sqlite.Db_A_Account.ClubName;          //クラブ名
            MemberNo.Text = _sqlite.Db_A_Account.MemberCode;        //会員№
            
            //-----------------------
            // 会員情報取得
            //-----------------------
            try
            {
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER("Select * " +
                                                                    "From M_MEMBER " +
                                                                    "Where MemberCode='" + wkMemberNo + "'"))
                {
                    //入会日
                    JoinDate.Text = _utl.GetString(row.JoinDate).Substring(0, 10);

                    //性別
                    Sex.Text = _utl.StrSex(row.Sex);

                    //会員名
                    MemberName.Text = wkMemberName;

                    // --------------------------
                    // クラブ役職・委員会取得
                    // --------------------------

                    // 初期化
                    wkExecutiveName = string.Empty;
                    wkCommitteeName = string.Empty;

                    //執行部名
                    if (_utl.GetString(row.ExecutiveCode) != string.Empty)
                    {
                        wkExecutiveName = _utl.GetString(row.ExecutiveName);
                    }

                    //執行部名(兼務)
                    if (_utl.GetString(row.ExecutiveCode1) != string.Empty)
                    {
                        wkExecutiveName = wkExecutiveName + Environment.NewLine + _utl.GetString(row.ExecutiveName1);
                    }
                    
                    //委員会(主)
                    if (_utl.GetString(row.CommitteeCode) != string.Empty)
                    {
                        wkCommitteeFlg = string.Empty;
                        if (_utl.GetString(row.CommitteeFlg) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName = _utl.GetString(row.CommitteeName) + wkCommitteeFlg;
                    }

                    //委員会(兼務1)
                    if (_utl.GetString(row.CommitteeCode1) != string.Empty)
                    {
                        wkCommitteeFlg = string.Empty;
                        if (_utl.GetString(row.CommitteeFlg1) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg1) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += Environment.NewLine + _utl.GetString(row.CommitteeName1) + wkCommitteeFlg;
                    }

                    //委員会(兼務2)
                    if (_utl.GetString(row.CommitteeCode2) != string.Empty)
                    {
                        wkCommitteeFlg = string.Empty;
                        if (_utl.GetString(row.CommitteeFlg2) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg2) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += Environment.NewLine + _utl.GetString(row.CommitteeName2) + wkCommitteeFlg;
                    }

                    //委員会(兼務3)
                    if (_utl.GetString(row.CommitteeCode3) != string.Empty)
                    {
                        wkCommitteeFlg = string.Empty;
                        if (_utl.GetString(row.CommitteeFlg3) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg3) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += Environment.NewLine + _utl.GetString(row.CommitteeName3) + wkCommitteeFlg;
                    }
                    
                    // 執行部・委員会生成
                    if (wkExecutiveName != string.Empty)
                    {
                        wkClubDistrictName = wkExecutiveName + Environment.NewLine + wkCommitteeName;
                    }
                    else
                    {
                        wkClubDistrictName = wkCommitteeName;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_MEMBER) : {ex.Message}", "OK");
            }

            //-----------------------
            // 地区役職取得
            //-----------------------
            try
            {
                foreach (Table.M_CABINET row in _sqlite.Get_M_CABINET("Select * " +
                                                                    "From M_CABINET " +
                                                                    "Where MemberCode='" + wkMemberNo + "'"))
                {
                    //地区役職設定
                    wkDistrictName = string.Empty;
                    if (_utl.GetString(row.DistrictName) != string.Empty)
                    {
                        wkDistrictName = _utl.GetString(row.DistrictName);                                           //地区役員名
                    }
                    if (_utl.GetString(row.DistrictName1) != string.Empty)
                    {
                        wkDistrictName = wkDistrictName + Environment.NewLine + _utl.GetString(row.DistrictName1);   //地区役員名(兼務1)
                    }
                    if (_utl.GetString(row.DistrictName2) != string.Empty)
                    {
                        wkDistrictName = wkDistrictName + Environment.NewLine + _utl.GetString(row.DistrictName2);   //地区役員名(兼務2)
                    }
                    if (_utl.GetString(row.DistrictName3) != string.Empty)
                    {
                        wkDistrictName = wkDistrictName + Environment.NewLine + _utl.GetString(row.DistrictName3);   //地区役員名(兼務3)
                    }
                    if (_utl.GetString(row.DistrictName4) != string.Empty)
                    {
                        wkDistrictName = wkDistrictName + Environment.NewLine + _utl.GetString(row.DistrictName4);   //地区役員名(兼務4)
                    }
                    if (_utl.GetString(row.DistrictName5) != string.Empty)
                    {
                        wkDistrictName = wkDistrictName + Environment.NewLine + _utl.GetString(row.DistrictName5);   //地区役員名(兼務5)
                    }
                }

                // 画面表示
                if (wkDistrictName != string.Empty)
                {
                    // 地区役員 + クラブ役員表示
                    Cabinet.Text = wkDistrictName + Environment.NewLine + wkClubDistrictName;
                }
                else
                {
                    // クラブ役員表示
                    Cabinet.Text = wkClubDistrictName;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_CABINET) : {ex.Message}", "OK");
            }

        }

        //---------------------------------------
        // アカウント設定画面(編集)
        //---------------------------------------
        void Button_Edit_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new AccountSetting();
        }

    }
}