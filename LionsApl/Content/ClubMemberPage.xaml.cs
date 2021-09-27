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
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private string _MemberCode;             // 会員番号

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
        /// <param name="memberno">会員番号</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
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

            // Content Utilクラス生成
            _utl = new LAUtility();

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
            string wkSex;    //性別

            //Table.TableUtil Util = new Table.TableUtil();

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
                    TypeName.Text = _utl.GetString(row.TypeName);

                    //会員名
                    MemberName.Text = _utl.GetString(row.MemberFirstName) + " " + _utl.GetString(row.MemberLastName);

                    //性別
                    if (_utl.GetString(row.Sex) == "1")
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
                    JoinDate.Text = _utl.GetString(row.JoinDate).Substring(0, 10);

                    //電話番号
                    Tel.Text = _utl.GetString(row.Tel);

                    // --------------------------
                    // クラブ役職・委員会取得
                    // --------------------------

                    // 初期化
                    wkExecutiveName = "";
                    wkCommitteeName = "";

                    //執行部名
                    if (_utl.GetString(row.ExecutiveCode) != "")
                    {
                        wkExecutiveName = _utl.GetString(row.ExecutiveName);
                    }

                    //執行部名(兼務)
                    if (_utl.GetString(row.ExecutiveCode1) != "")
                    {
                        wkExecutiveName = wkExecutiveName + "\r\n" + _utl.GetString(row.ExecutiveName1);
                    }
                    //DisplayAlert("Alert", $"{wkExecutiveName}", "OK");

                    //委員会(主)
                    if (_utl.GetString(row.CommitteeCode) != "")
                    {
                        if (_utl.GetString(row.CommitteeFlg) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg == "1")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName = _utl.GetString(row.CommitteeName) + wkCommitteeFlg;
                    }

                    //委員会(兼務1)
                    if (_utl.GetString(row.CommitteeCode1) != "")
                    {
                        if (_utl.GetString(row.CommitteeFlg1) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (row.CommitteeFlg1 == "1")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + _utl.GetString(row.CommitteeName1) + wkCommitteeFlg;
                    }

                    //委員会(兼務2)
                    if (_utl.GetString(row.CommitteeCode2) != "")
                    {
                        if (_utl.GetString(row.CommitteeFlg2) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg2) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + _utl.GetString(row.CommitteeName2) + wkCommitteeFlg;
                    }

                    //委員会(兼務3)
                    if (_utl.GetString(row.CommitteeCode3) != "")
                    {
                        if (_utl.GetString(row.CommitteeFlg3) == "1")
                        {
                            wkCommitteeFlg = "（委員長）";
                        }
                        else if (_utl.GetString(row.CommitteeFlg3) == "2")
                        {
                            wkCommitteeFlg = "（副委員長）";
                        }
                        wkCommitteeName += "\r\n" + _utl.GetString(row.CommitteeName3) + wkCommitteeFlg;
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
                    if (_utl.GetString(row.DistrictName) != "")
                    {
                        wkDistrictName = _utl.GetString(row.DistrictName);             //地区役員名
                    }
                    if (_utl.GetString(row.DistrictName1) != "")
                    {
                        wkDistrictName += "\r\n" + _utl.GetString(row.DistrictName1);   //地区役員名(兼務1)
                    }
                    if (_utl.GetString(row.DistrictName2) != "")
                    {
                        wkDistrictName += "\r\n" + _utl.GetString(row.DistrictName2);   //地区役員名(兼務2)
                    }
                    if (_utl.GetString(row.DistrictName3) != "")
                    {
                        wkDistrictName += "\r\n" + _utl.GetString(row.DistrictName3);   //地区役員名(兼務3)
                    }
                    if (_utl.GetString(row.DistrictName4) != "")
                    {
                        wkDistrictName += "\r\n" + _utl.GetString(row.DistrictName4);   //地区役員名(兼務4)
                    }
                    if (_utl.GetString(row.DistrictName5) != "")
                    {
                        wkDistrictName += "\r\n" + _utl.GetString(row.DistrictName5);   //地区役員名(兼務5)
                    }
                    //DisplayAlert("Alert", $"{wkDistrictName}", "OK");
                }

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
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_CABINET) : &{ex.Message}", "OK");
            }



        }

    }
}