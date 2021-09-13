using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMemberPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // 前画面からの取得情報
        private string _MemberCode;      // タイトル

        // 変数
        private string wkDistrictName;          //地区役員
        private string wkExecutiveName;         //クラブ執行部
        private string wkCommitteeName;         //クラブ委員会
        private string wkCommitteeFlg;          //委員長・副委員長
        private string wkClubDistrictName;      //クラブ役員

        public ClubMemberPage(string memberno)
        {
            InitializeComponent();

            // font-size
            this.lbl_MemberCode.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MemberCode.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_TypeName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.TypeName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MemberName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MemberName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Sex.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Sex.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_JoinDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.JoinDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Tel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Tel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Obligation.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Obligation.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));


            // 一覧から取得（会員番号）
            _MemberCode = memberno;

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

            // 会員情報設定
            GetMemberInfo();

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMemberInfo()
        {

            // 変数宣言
            string wkSex;                   //性別

            // 会員情報取得
            try
            {
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER("Select * " +
                                                                    "From M_MEMBER " +
                                                                    "Where MemberCode='" + _MemberCode + "'"))
                {

                    //会員番号
                    MemberCode.Text = _MemberCode;

                    //会員種別
                    TypeName.Text = row.TypeName;

                    //会員名
                    MemberName.Text = row.MemberFirstName + " " + row.MemberLastName;

                    //性別
                    if (row.Sex == "1")
                    {
                        wkSex = "男性";
                    }
                    else if (row.Sex == "2")
                    {
                        wkSex = "女性";
                    }
                    else
                    {
                        wkSex = "その他";
                    }
                    Sex.Text = wkSex;

                    //入会日
                    JoinDate.Text = row.JoinDate.Substring(0, 10);

                    //電話番号
                    Tel.Text = row.Tel;

                    // --------------------------
                    // クラブ役職・委員会取得
                    // --------------------------

                    // 初期化
                    wkExecutiveName = "";
                    wkCommitteeName = "";

                    //執行部名
                    if (row.ExecutiveCode != null)
                    {
                        wkExecutiveName = row.ExecutiveName;
                    }

                    //執行部名(兼務)
                    if (row.ExecutiveCode1 != null)
                    {
                        wkExecutiveName = wkExecutiveName + "\r\n" + row.ExecutiveName1;
                    }
                    //DisplayAlert("Alert", $"{wkExecutiveName}", "OK");

                    //委員会(主)
                    if (row.CommitteeCode != null)
                    {
                        if (row.CommitteeFlg == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg == "1")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName = row.CommitteeName + wkCommitteeFlg;
                    }

                    //委員会(兼務1)
                    if (row.CommitteeCode1 != null)
                    {
                        if (row.CommitteeFlg1 == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg1 == "1")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + row.CommitteeName1 + wkCommitteeFlg;
                    }

                    //委員会(兼務2)
                    if (row.CommitteeCode2 != null)
                    {
                        if (row.CommitteeFlg2 == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg2 == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + row.CommitteeName2 + wkCommitteeFlg;
                    }

                    //委員会(兼務3)
                    if (row.CommitteeCode3 != null)
                    {
                        if (row.CommitteeFlg3 == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg3 == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + row.CommitteeName3 + wkCommitteeFlg;
                    }
                    //DisplayAlert("Alert", $"{wkCommitteeName}", "OK");

                    // 執行部・委員会生成
                    if (wkExecutiveName != "")
                    {
                        wkClubDistrictName = wkExecutiveName + "\r\n" + wkCommitteeName;
                    }
                    else
                    {
                        wkClubDistrictName = wkCommitteeName;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_MEMBER) : &{ex.Message}", "OK");
            }

            // 地区役職取得
            try
            {
                foreach (Table.M_CABINET row in _sqlite.Get_M_CABINET("Select * " +
                                                                    "From M_CABINET " +
                                                                    "Where MemberCode='" + _MemberCode + "'"))
                {

                    //地区役職設定
                    wkDistrictName = "";
                    if (row.DistrictName != null)
                    {
                        wkDistrictName = row.DistrictName;             //地区役員名
                    }
                    if (row.DistrictName1 != null)
                    {
                        wkDistrictName += "\r\n" + row.DistrictName1;   //地区役員名(兼務1)
                    }
                    if (row.DistrictName2 != null)
                    {
                        wkDistrictName += "\r\n" + row.DistrictName2;   //地区役員名(兼務2)
                    }
                    if (row.DistrictName3 != null)
                    {
                        wkDistrictName += "\r\n" + row.DistrictName3;   //地区役員名(兼務3)
                    }
                    if (row.DistrictName4 != null)
                    {
                        wkDistrictName += "\r\n" + row.DistrictName4;   //地区役員名(兼務4)
                    }
                    if (row.DistrictName5 != null)
                    {
                        wkDistrictName += "\r\n" + row.DistrictName5;   //地区役員名(兼務5)
                    }
                    //DisplayAlert("Alert", $"{wkDistrictName}", "OK");

                    // 画面表示
                    if (wkDistrictName != "")
                    {
                        // 地区役員 + クラブ役員表示
                        Obligation.Text = wkDistrictName + "\r\n" + wkClubDistrictName;
                    }
                    else
                    {
                        // クラブ役員表示
                        Obligation.Text = wkClubDistrictName;
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_CABINET) : &{ex.Message}", "OK");
            }



        }

    }
}