using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SQLiteChk : ContentPage
    {
        public ObservableCollection<CTablePicker> _tablePk = new ObservableCollection<CTablePicker>();

        private SQLiteManager sqliteManager;

        private string _selTable = null;
        //private string _selTableId = null;

        public SQLiteChk()
        {
            InitializeComponent();

            sqliteManager = SQLiteManager.GetInstance();

            _tablePk.Add(new CTablePicker("0", "A_APLLOG"));
            _tablePk.Add(new CTablePicker("1", "A_SETTING"));
            _tablePk.Add(new CTablePicker("2", "A_ACCOUNT"));
            _tablePk.Add(new CTablePicker("3", "A_FILEPATH"));
            _tablePk.Add(new CTablePicker("4", "T_SLOGAN"));
            _tablePk.Add(new CTablePicker("5", "T_LETTER"));
            _tablePk.Add(new CTablePicker("6", "T_EVENTRET"));
            _tablePk.Add(new CTablePicker("7", "T_EVENT"));
            _tablePk.Add(new CTablePicker("8", "T_INFOMATION_CABI"));
            _tablePk.Add(new CTablePicker("9", "T_MAGAZINE"));
            _tablePk.Add(new CTablePicker("10", "T_MAGAZINEBUY"));
            //_tablePk.Add(new CTablePicker("11", "M_DISTRICTOFFICER"));
            _tablePk.Add(new CTablePicker("12", "M_CABINET"));
            _tablePk.Add(new CTablePicker("13", "M_CLUB"));
            _tablePk.Add(new CTablePicker("14", "T_CLUBSLOGAN"));
            _tablePk.Add(new CTablePicker("15", "T_MEETINGSCHEDULE"));
            _tablePk.Add(new CTablePicker("16", "T_DIRECTOR"));
            _tablePk.Add(new CTablePicker("17", "T_MEETINGPROGRAM"));
            _tablePk.Add(new CTablePicker("18", "T_INFOMATION_CLUB"));
            _tablePk.Add(new CTablePicker("19", "M_MEMBER"));
            TablePicker.ItemsSource = _tablePk;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 戻るボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Back_Button_Clicked(object sender, System.EventArgs e)
        {
            BackButton.BackgroundColor = Color.Gray;
            Application.Current.MainPage = new MainPage();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 対象テーブル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void TablePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = TablePicker.SelectedItem as CTablePicker;
            if (item != null)
            {
                // 選択リジョン情報設定
                //_selTableId = item.Id.ToString();
                _selTable = item.TableName.ToString();

                switch (_selTable)
                {
                    case "A_APLLOG":
                        Sel_A_APLLOG();
                        break;
                    case "A_SETTING":
                        Sel_A_SETTING();
                        break;
                    case "A_ACCOUNT":
                        Sel_A_ACCOUNT();
                        break;
                    case "A_FILEPATH":
                        Sel_A_FILEPATH();
                        break;
                    case "T_SLOGAN":
                        Sel_T_SLOGAN();
                        break;
                    case "T_LETTER":
                        Sel_T_LETTER();
                        break;
                    case "T_EVENTRET":
                        Sel_T_EVENTRET();
                        break;
                    case "T_EVENT":
                        Sel_T_EVENT();
                        break;
                    case "T_MAGAZINE":
                        Sel_T_MAGAZINE();
                        break;
                    case "T_INFOMATION_CABI":
                        Sel_T_INFOMATION_CABI();
                        break;
                    case "T_MAGAZINEBUY":
                        Sel_T_MAGAZINEBUY();
                        break;
                    //case "M_DISTRICTOFFICER":
                    //    Sel_M_DISTRICTOFFICER();
                    //    break;
                    case "M_CABINET":
                        Sel_M_CABINET();
                        break;
                    case "M_CLUB":
                        Sel_M_CLUB();
                        break;
                    case "T_CLUBSLOGAN":
                        Sel_T_CLUBSLOGAN();
                        break;
                    case "T_MEETINGSCHEDULE":
                        Sel_T_MEETINGSCHEDULE();
                        break;
                    case "T_DIRECTOR":
                        Sel_T_DIRECTOR();
                        break;
                    case "T_MEETINGPROGRAM":
                        Sel_T_MEETINGPROGRAM();
                        break;
                    case "T_INFOMATION_CLUB":
                        Sel_T_INFOMATION_CLUB();
                        break;
                    case "M_MEMBER":
                        Sel_M_MEMBER();
                        break;
                    default:
                        break;

                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 選択したテーブルの内容を表示する。
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// キャビネット
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(A_APLLOG)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_A_APLLOG()
        {
            /// label clear
            ResultLabel.Text = "A_APLLOG:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.A_APLLOG>("Select * From A_APLLOG"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.LogDate}, {row.LogClass}, {row.MachineClass}, {row.ClubCode}, \r\n" +
                                            $"{row.ClubName}, {row.MembwrCode}, {row.MemberFirstName}, {row.MemberLastName}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select A_APLLOG Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(A_SETTING)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_A_SETTING()
        {
            /// label clear
            ResultLabel.Text = "A_SETTING:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.A_SETTING>("Select * From A_SETTING"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DistrictCode}, {row.DistrictName}, {row.CabinetName},\r\n" +
                                            $"{row.PeriodStart}, {row.PeriodEnd}, {row.DistrictID}, {row.MagazineMoney}, {row.EventDataDay}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select A_SETTING Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(A_ACCOUNT)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_A_ACCOUNT()
        {
            /// label clear
            ResultLabel.Text = "A_ACCOUNT:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.A_ACCOUNT>("Select * From A_ACCOUNT"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.Region}, {row.Zone}, {row.ClubCode}, {row.ClubName},\r\n" +
                                            $"{row.MemberCode}, {row.MemberFirstName}, {row.MemberLastName}, {row.AccountDate}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select A_ACCOUNT Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(A_FILEPATH)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_A_FILEPATH()
        {
            /// label clear
            ResultLabel.Text = "A_FILEPATH:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.A_FILEPATH>("Select * From A_FILEPATH"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataClass}, {row.FilePath}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select A_FILEPATH Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_SLOGAN)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_SLOGAN()
        {
            /// label clear
            ResultLabel.Text = "T_SLOGAN:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_SLOGAN>("Select * From T_SLOGAN"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.FiscalStart}, {row.FiscalEnd},\r\n" +
                                            $"{row.Slogan}\r\n" +
                                            $"{ row.DistrictGovernor},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_SLOGAN Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_LETTER)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_LETTER()
        {
            /// label clear
            ResultLabel.Text = "T_LETTER:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_LETTER>("Select * From T_LETTER"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" +
                                            $"{row.Id}, {row.DataNo}, {row.EventDate}, {row.EventTime},\r\n" +
                                            $"{row.Title}, {row.Body}\r\n" +
                                            $"{ row.Image1FileName}, { row.Image2FileName}, { row.NoticeFlg}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_LETTER Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_EVENTRET)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_EVENTRET()
        {
            /// label clear
            ResultLabel.Text = "T_EVENTRET:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_EVENTRET>("Select * From T_EVENTRET"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.EventClass}, {row.EventClass}, {row.EventDataNo}, {row.EventDate},\r\n" +
                                            $"{row.ClubCode}, {row.ClubNameShort}, {row.MemberCode}, {row.MemberName},\r\n" +
                                            $"{row.Answer}, {row.AnswerLate}, {row.AnswerEarly}, {row.Option1}, {row.Option2}, {row.Option3}, {row.Option4}, {row.Option5}, {row.OtherCount}, {row.TargetFlg}, {row.CancelFlg}, \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_EVENTRET Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_EVENT)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_EVENT()
        {
            /// label clear
            ResultLabel.Text = "T_EVENT:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_EVENT>("Select * From T_EVENT"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.EventDate}, {row.EventTimeStart}, {row.EventTimeEnd}, {row.ReceptionTime},\r\n" +
                                            $"{row.EventPlace},\r\n" +
                                            $"{row.Title},\r\n" +
                                            $"{row.Body},\r\n" +
                                            $"{row.OptionName1}, {row.OptionRadio1}, {row.OptionName2}, {row.OptionRadio2}, {row.OptionName3}, {row.OptionRadio3}, {row.OptionName4}, {row.OptionRadio4}, {row.OptionName5}, {row.OptionRadio5},\r\n" +
                                            $"{row.Sake}, {row.Meeting}, {row.MeetingUrl}, {row.MeetingID}, {row.MeetingPW}, {row.MeetingOther}, {row.AnswerDate}, {row.FileName},\r\n" +
                                            $"{row.CabinetUser}, {row.ClubUser}, {row.NoticeFlg},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_EVENTT Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_INFOMATION_CABI)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_INFOMATION_CABI()
        {
            /// label clear
            ResultLabel.Text = "T_INFOMATION_CABI:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_INFOMATION_CABI>("Select * From T_INFOMATION_CABI"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.AddDate},\r\n" +
                                            $"{row.Subject},\r\n" +
                                            $"{row.Detail},\r\n" +
                                            $"{row.FileName},\r\n" +
                                            $"{row.InfoFlg},\r\n" +
                                            $"{row.InfoUser},\r\n" +
                                            $"{row.NoticeFlg}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_INFOMATION_CABI Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_MAGAZINE)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_MAGAZINE()
        {
            /// label clear
            ResultLabel.Text = "T_MAGAZINE:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.T_MAGAZINE>("Select * From T_MAGAZINE"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" +
                                            $"{row.Id}, {row.DataNo}, {row.SortNo}, {row.Magazine}, \r\n {row.FilePath},\r\n" +
                                            $"{row.FileName}, {row.MagazineClass}, {row.MagazinePrice}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_MAGAZINE Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_MAGAZINEBUY)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_MAGAZINEBUY()
        {
            /// label clear
            ResultLabel.Text = "T_MAGAZINEBUY:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.T_MAGAZINEBUY>("Select * From T_MAGAZINEBUY"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.MagazineDataNo}, {row.Magazine},\r\n" +
                                            $"{row.BuyDate}, {row.BuyNumber}, {row.MagazinePrice}, {row.MoneyTotal},\r\n" +
                                            $"{row.Region}, {row.Zone}, {row.ClubCode}, {row.ClubNameShort}, {row.MemberCode}, {row.MemberName},\r\n" +
                                            $"{row.ShippingDate}, {row.PaymentDate}, {row.Payment}, {row.DelFlg}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_MAGAZINEBUY Error : " + ex.Message;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// 選択したテーブルの内容を表示する。(M_DISTRICTOFFICER)
        ///// </summary>
        /////////////////////////////////////////////////////////////////////////////////////////////
        //private void Sel_M_DISTRICTOFFICER()
        //{
        //    /// label clear
        //    ResultLabel.Text = "M_DISTRICTOFFICER:\r\n";

        //    // データ取得
        //    try
        //    {
        //        using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
        //        {
        //            foreach (var row in db.Query<Table.M_DISTRICTOFFICER>("Select * From M_DISTRICTOFFICER"))
        //            {
        //                ResultLabel.Text += $"{row.Id}, {row.DistrictClass}, {row.DistrictCode}, {row.DistrictName}\r\n";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ResultLabel.Text = "Select M_DISTRICTOFFICER Error : " + ex.Message;
        //    }
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(M_CABINET)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_M_CABINET()
        {
            /// label clear
            ResultLabel.Text = "M_CABINET:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.M_CABINET>("Select * From M_CABINET"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.FiscalStart}, {row.FiscalEnd}, {row.MemberCode}, {row.MemberName},\r\n" + 
                                            $"{row.ClubCode}, {row.ClubNameShort}, {row.DistrictClass}, {row.DistrictCode}, {row.DistrictName}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select M_CABINET Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(M_CLUB)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_M_CLUB()
        {
            /// label clear
            ResultLabel.Text = "M_CLUB:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.M_CLUB>("Select * From M_CLUB"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.Region}, {row.Zone}, {row.Sort}, {row.ClubCode}, {row.ClubName}, {row.ClubNameShort},\r\n" +
                                            $"{row.PassWord}, {row.FormationDate}, {row.CharterNight}, {row.MeetingDate}, {row.MeetingTime}, {row.MeetingPlace}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select M_CLUB Error : " + ex.Message;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// クラブ
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_CLUBSLOGAN)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_CLUBSLOGAN()
        {
            /// label clear
            ResultLabel.Text = "T_CLUBSLOGAN:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.T_CLUBSLOGAN>("Select * From T_CLUBSLOGAN"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" +
                                            $"{row.Id}, {row.DataNo}, {row.ClubCode}, {row.ClubNameShort}, {row.FiscalStart}, {row.FiscalEnd},\r\n" +
                                            $"{row.ClubSlogan},\r\n" +
                                            $"{row.ExecutiveName}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_CLUBSLOGAN Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_MEETINGSCHEDULE)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_MEETINGSCHEDULE()
        {
            /// label clear
            ResultLabel.Text = "T_MEETINGSCHEDULE:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_MEETINGSCHEDULE>("Select * From T_MEETINGSCHEDULE"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.DataNo}, {row.ClubCode}, {row.ClubNameShort}, {row.Fiscal}, {row.MeetingDate}, {row.MeetingTime}, {row.MeetingCount},\r\n" +
                                            $"{row.MeetingName}, {row.Online}, {row.AnswerDate}, {row.AnswerTime},\r\n" +
                                            $"{row.RemarksItems}, {row.RemarksCheck}, {row.Remarks},\r\n" +
                                            $"{row.OptionName1}, {row.OptionRadio1}, {row.OptionName2}, {row.OptionRadio2}, {row.OptionName3}, {row.OptionRadio3},\r\n" +
                                            $"{row.OptionName4}, {row.OptionRadio4}, {row.OptionName5}, {row.OptionRadio5}, {row.Sake}, {row.OtherUser}, {row.CancelFlg},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_MEETINGSCHEDULE Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_DIRECTOR)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_DIRECTOR()
        {
            /// label clear
            ResultLabel.Text = "T_DIRECTOR:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_DIRECTOR>("Select * From T_DIRECTOR"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" +
                                            $"{row.Id}, {row.DataNo}, {row.ClubCode}, {row.ClubNameShort}, {row.CommitteeCode}, {row.CommitteeName},\r\n" +
                                            $"{row.Season}, {row.EventDate}, {row.EventTime}, {row.EventPlace},\r\n" +
                                            $"{row.Subject},\r\n" +
                                            $"{row.Agenda},\r\n" +
                                            $"{row.Member},\r\n" +
                                            $"{row.MemberAdd},\r\n" +
                                            $"{row.AnswerDate},{row.CancelFlg},{row.NoticeFlg},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_DIRECTOR Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_MEETINGPROGRAM)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_MEETINGPROGRAM()
        {
            /// label clear
            ResultLabel.Text = "T_MEETINGPROGRAM:\r\n";

            // データ取得
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {   // Select
                    foreach (var row in db.Query<Table.T_MEETINGPROGRAM>("Select * From T_MEETINGPROGRAM"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" +
                                            $"{row.Id}, {row.DataNo}, {row.ScheduleDataNo}, {row.ClubCode}, {row.ClubNameShort}, {row.Fiscal},\r\n" +
                                            $"{row.FileName},\r\n" +
                                            $"{row.FileName1},\r\n" +
                                            $"{row.FileName2},\r\n" +
                                            $"{row.FileName3},\r\n" +
                                            $"{row.FileName4},\r\n" +
                                            $"{row.FileName5},\r\n" +
                                            $"{row.Meeting},{row.MeetingUrl},{row.MeetingID},{row.MeetingPW},{row.MeetingOther},{row.NoticeFlg},\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_MEETINGPROGRAM Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_INFOMATION_CLUB)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_INFOMATION_CLUB()
        {
            /// label clear
            ResultLabel.Text = "T_INFOMATION_CLUB:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.T_INFOMATION_CLUB>("Select * From T_INFOMATION_CLUB"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.ClubCode}, {row.ClubNameShort}, {row.AddDate},\r\n" +
                                            $"{row.Subject},\r\n" +
                                            $"{row.Detail},\r\n" +
                                            $"{row.FileName},\r\n" +
                                            $"{row.InfoFlg}, {row.TypeCode}, \r\n" +
                                            $"{row.InfoUser},\r\n" +
                                            $"{row.NoticeFlg}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select T_INFOMATION_CLUB Error : " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(M_MEMBER)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_M_MEMBER()
        {
            /// label clear
            ResultLabel.Text = "M_MEMBER:\r\n";

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(sqliteManager.DbPath))
                {
                    foreach (var row in db.Query<Table.M_MEMBER>("Select * From M_MEMBER"))
                    {
                        ResultLabel.Text += $"----------------------------------------------------------------\r\n" + 
                                            $"{row.Id}, {row.Region}, {row.Zone}, {row.ClubCode}, {row.ClubNameShort},\r\n" +
                                            $"{row.MemberCode}, {row.MemberFirstName}, {row.MemberLastName}, {row.MemberNameKana}\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Select M_MEMBER Error : " + ex.Message;
            }
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Tableピッカークラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public class CTablePicker
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public CTablePicker(string id, string tablename)
        {
            Id = id;
            TableName = tablename;
        }
    }
}