using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.IO;

namespace LionsApl
{
    public partial class MainPage : ContentPage
    {
        // SQLiteのパス
        public static string TDbPath { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SQLiteDataBase.db");

        public string startTime;
        public string endTime;

        private SQLiteManager sqliteManager;

        public MainPage()
        {
            InitializeComponent();

            sqliteManager = SQLiteManager.GetInstance();
        }


        // ===============================
        // ボタン押下処理
        // ===============================
        #region 各ボタン処理

            //---------------------------------------
            // HOME画面
            //---------------------------------------
            void Button_Home_Clicked(object sender, System.EventArgs e)
            {

                // DB情報取得処理
                try
                {
                    Task<HttpResponseMessage> response = sqliteManager.AsyncPostFileForWebAPI(MessageText, sqliteManager.GetSendFileContent_HOME());
                    ResultText.Text += "Send SQLite file Finish : " + response.Result.StatusCode.ToString();
                }
                catch (Exception ex)
                {
                    ResultText.Text += "Send SQLite file Error : " + ex.Message;

                }

                // HOME画面表示
                Application.Current.MainPage = new Content.MainTabPage();
            }

            //---------------------------------------
            // アカウント設定
            //---------------------------------------
            void Button_Account_Clicked(object sender, System.EventArgs e)
            {

                DateTime stDt = DateTime.Now;
                StartText.Text = stDt.ToString();

                ResultText.Text = "";

                try
                {
                    // 空DB作成処理
                    sqliteManager.CreateTable_ALL();

                    // アカウント情報取得
                    Task<HttpResponseMessage> response = sqliteManager.AsyncPostFileForWebAPI(MessageText, sqliteManager.GetSendFileContent());
                    ResultText.Text = "Create Table Finish : \r\n" + sqliteManager.DbPath;
                }
                catch (Exception ex)
                {
                    ResultText.Text = "アカウント設定 : \r\n" + ex.Message;
                }
                ResultText.Text = "アカウント設定 OK";

                /// 終了時間表示
                DateTime edDt = DateTime.Now;
                EndText.Text = edDt.ToString();

                // アカウント設定画面表示
                Application.Current.MainPage = new Content.AccountSetting();

            }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 各画面遷移関連
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Account Setting)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_AccountSetting_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.AccountSetting());
            Application.Current.MainPage = new Content.AccountSetting();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Main Tab Page)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_MainTabPage_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.AccountSetting());
            Application.Current.MainPage = new Content.MainTabPage();
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Home Top)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_HomeTop_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushAsync(new Content.HomeTop());
            Application.Current.MainPage = new Content.HomeTop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Club Top)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_ClubTop_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.ClubTop());
            Application.Current.MainPage = new Content.ClubTop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(SQLite Check)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_SQLiteCheck_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new Content.SQLiteChk();
        }
        


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// SQLite関連（ファイル操作）
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create Sqliteファイル（DB3）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_CreateTable_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            ResultText.Text = "";

            // データベース初期設定
            try
            {
                sqliteManager.CreateTable_ALL();
                ResultText.Text = "Create Table Finish : \r\n" + sqliteManager.DbPath;
            }
            catch (Exception ex)
            {
                ResultText.Text = "Create Table Error : \r\n" + ex.Message;

            }
            ResultText.Text = "Create Table OK";

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報を取得する（Send SQLite File）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_SendSQLite_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Send SQLite File ACCOUNT";
            MessageText.Text = "";
            ResultText.Text = "";

            MessageText.Text = "Click Send SQLite File ACCOUNT\r\n";
            try
            {
                //HttpResponseMessage response = sqliteTest.AsyncPostFileForWebAPI();
                Task<HttpResponseMessage> response = sqliteManager.AsyncPostFileForWebAPI(MessageText, sqliteManager.GetSendFileContent());

                ResultText.Text += "Send SQLite file Finish : " + response.Result.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                ResultText.Text += "Send SQLite file Error : " + ex.Message;

            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報を元にDB情報を取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_SendSQLite_HOME_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Send SQLite File HOME";
            MessageText.Text = "";
            ResultText.Text = "";

            MessageText.Text = "Click Send SQLite File HOME\r\n";
            try
            {
                //HttpResponseMessage response = sqliteTest.AsyncPostFileForWebAPI();
                Task<HttpResponseMessage> response = sqliteManager.AsyncPostFileForWebAPI(MessageText, sqliteManager.GetSendFileContent_HOME());

                ResultText.Text += "Send SQLite file Finish : " + response.Result.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                ResultText.Text += "Send SQLite file Error : " + ex.Message;

            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get File Names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_GetFileName_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Get File Names";
            MessageText.Text = "";
            ResultText.Text = "";
            FileText.Text = "";

            //sqliteManager = SQLiteManager.GetInstance();
            string[] names = sqliteManager.GetFileName();
            foreach (string name in names)
            {
                var lwTime = System.IO.File.GetLastWriteTime(name);
                FileText.Text += name + " " + lwTime + "\r\n";
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get DB Table Names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_GetDBTableName_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Get DB Table Names";
            MessageText.Text = "";
            ResultText.Text = "";
            FileText.Text = "";

            //sqliteManager = SQLiteManager.GetInstance();

            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {

                    const string cmdText = "SELECT name FROM sqlite_master WHERE type='table'";
                    var cmd = db.CreateCommand(cmdText);
                    //ResultText.Text = cmd.ExecuteScalar<string>();
                    var datas = cmd.ExecuteQueryScalars<string>().ToList();
                    foreach (string data in datas)
                    {
                        ResultText.Text += data + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Get DB Table Names Error : " + ex.Message;
            }


            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// SQLite関連（データ操作）
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_A_SETTING_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_A_SETTING_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Select A_SETTING";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.A_SETTING>("Select * From A_SETTING"))
                    {
                        ResultText.Text += $"A_SETTING:\r\n" +
                                           $"{row.Id}, {row.DistrictCode}, {row.DistrictName}, {row.CabinetName},\r\n" + 
                                           $"{row.PeriodStart}, {row.PeriodEnd}, {row.DistrictID}, {row.MagazineMoney}, {row.EventDataDay}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select A_SETTING Error : " + ex.Message;
            }
            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_M_MEMBER_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_M_MEMBER_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Select M_MEMBER";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    // foreach (var row in db.Table<M_MEMBER>())
                    foreach (var row in db.Query<Table.M_MEMBER>("Select * From M_MEMBER"))
                    {
                        ResultText.Text += $"M_MEMBER: \r\b" +
                                           $"{row.Id}, {row.Region}, {row.Zone}, {row.ClubCode}, {row.ClubNameShort}, \r\n" +
                                           $"{row.MemberCode}, {row.MemberFirstName}, {row.MemberLastName}, {row.MemberNameKana}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select M_MEMBER Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_A_ACCOUNT_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_A_ACCOUNT_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Select A_ACCOUNT";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.A_ACCOUNT>("Select * From A_ACCOUNT"))
                    {
                        ResultText.Text += $"A_ACCOUNT: \r\n" +
                                           $"{row.Id}, {row.Region}, {row.Zone}, {row.ClubCode}, {row.ClubName},\r\n {row.MemberCode}, {row.MemberFirstName}, {row.MemberLastName}, {row.AccountDate}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select A_ACCOUNT Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_T_SLOGAN_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_T_SLOGAN_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Select T_SLOGAN";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_SLOGAN>("Select * From T_SLOGAN"))
                    {
                        ResultText.Text += $"T_SLOGAN:\r\n" +
                                           $" {row.Id}, {row.DataNo}, {row.FiscalStart}, {row.FiscalEnd},\r\n" +
                                           $"{row.Slogan},\r\n" +
                                           $" {row.DistrictGovernor}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select T_SLOGAN Error : " + ex.Message;
            }
            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_T_LETTER_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_T_LETTER_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select T_LETTER";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_LETTER>("Select * From T_LETTER"))
                    {
                        ResultText.Text += $"T_LETTER:\r\n" + 
                                           $"{row.Id}, {row.DataNo}, {row.EventDate}, {row.EventTime}, {row.Title},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select T_LETTER Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_T_EVENTRET_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_T_EVENTRET_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select EVENTRET";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_EVENTRET>("Select * From T_EVENTRET"))
                    {
                        ResultText.Text += $"T_EVENTRET:\r\n " +
                                           $"{row.Id}, {row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.MemberName}, {row.Answer} \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select T_EVENTRET Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_T_EVENT_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_T_EVENT_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select EVENT";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_EVENT>("Select * From T_EVENT"))
                    {
                        ResultText.Text += $"T_EVENT:\r\n " +
                                           $"{row.Id}, {row.DataNo}, {row.EventDate}, {row.Title} \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select T_EVENT Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button_Select_T_MEETINGSCHEDULE_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_T_MEETINGSCHEDULE_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select MEETINGSCHEDULE";
            MessageText.Text = "";
            ResultText.Text = "";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_MEETINGSCHEDULE>("Select * From T_MEETINGSCHEDULE"))
                    {
                        ResultText.Text += $"T_MEETINGSCHEDULE:\r\n " +
                                           $"{row.Id}, {row.DataNo}, {row.MeetingDate}, {row.MeetingName} \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select T_MEETINGSCHEDULE Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button4_Select_JOIN_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_JOIN_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select JOIN";
            MessageText.Text = "";
            ResultText.Text = "";

            sqliteManager.SetAccount();

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (Table.HOME_EVENT row in db.Query<Table.HOME_EVENT>(
                                        "SELECT " +
                                            "t1.DataNo, " +
                                            "t1.EventClass, " +
                                            "t1.EventDataNo, " +
                                            "t1.EventDate, " +
                                            "t1.ClubCode, " +
                                            "t1.MemberCode, " +
                                            "t1.Answer, " +
                                            "t1.CancelFlg, " +
                                            "t2.Title, " +
                                            "t3.MeetingName " +
                                        "FROM " +
                                            "T_EVENTRET t1 " +
                                        "LEFT OUTER JOIN " +
                                            "T_EVENT t2 " +
                                        "ON " +
                                            "t1.EventClass = '1' and " +
                                            "t1.EventDataNo = t2.DataNo " +
                                        "LEFT OUTER JOIN " +
                                            "T_MEETINGSCHEDULE t3 " +
                                        "ON " +
                                            "t1.EventClass = '2' and " +
                                            "t1.EventDataNo = t3.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + sqliteManager.Db_A_Account.MemberCode + "'"))
                    {
                        if (row.EventClass.Equals("1"))
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.Title}\r\n";
                        }
                        else
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.MeetingName}\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select JOIN Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button4_Select_JOINT_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_JOINT_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select JOINT";
            MessageText.Text = "";
            ResultText.Text = "";

            sqliteManager.SetAccount();

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (Table.HOME_EVENT row in db.Query<Table.HOME_EVENT>(
                                        "SELECT " +
                                            "t1.DataNo, " +
                                            "t1.EventClass, " +
                                            "t1.EventDataNo, " +
                                            "t1.EventDate, " +
                                            "t1.ClubCode, " +
                                            "t1.MemberCode, " +
                                            "t1.Answer, " +
                                            "t1.CancelFlg, " +
                                            "t2.Title, " +
                                            "t3.MeetingName " +
                                        "FROM " +
                                            "T_EVENTRET t1 " +
                                        "LEFT OUTER JOIN " +
                                            "T_EVENT t2 " +
                                        "ON " +
                                            "t1.EventClass = '1' and " +
                                            "t1.EventDataNo = t2.DataNo " +
                                        "LEFT OUTER JOIN " +
                                            "T_MEETINGSCHEDULE t3 " +
                                        "ON " +
                                            "t1.EventClass = '2' and " +
                                            "t1.EventDataNo = t3.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + sqliteManager.Db_A_Account.MemberCode + "'"))
                    {
                        if (row.EventClass.Equals("1"))
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.Title}\r\n";
                        }
                        else
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.MeetingName}\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select JOINT Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Delete DB3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_DeleteDB3_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Delete DB3";
            MessageText.Text = "";
            ResultText.Text = "";

            try
            {
                File.Delete(sqliteManager.DbPath);
                ResultText.Text += "Delete finish : \r\n" + sqliteManager.DbPath + "\r\n";
                //File.Delete(TDbPath);
                //ResultText.Text += "Delete finish : " + TDbPath;
            }
            catch (IOException deleteError)
            {
                ResultText.Text += "Delete Error : " + deleteError.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }



    }
}
