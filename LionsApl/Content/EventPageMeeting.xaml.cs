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
    public partial class EventPageMeeting : ContentView
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// バインダブルプロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会日プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty DatePropaty =
            BindableProperty.Create("Date",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 中止プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty CancelPropaty =
            BindableProperty.Create("Cancel",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会場所プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty PlacePropaty =
            BindableProperty.Create("Place",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会回数プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty CountPropaty =
            BindableProperty.Create("Count",
                                    typeof(int),
                                    typeof(EventPageMeeting),
                                    0);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会名プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty NamePropaty =
            BindableProperty.Create("Name",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会議方法プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty OnlinePropaty =
            BindableProperty.Create("Online",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会議方法高さプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Rwh_OnlinePropaty =
            BindableProperty.Create("Rwh_Online",
                                    typeof(GridLength),
                                    typeof(EventPageMeeting),
                                    GridLength.Auto);


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// お酒プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty SakePropaty =
            BindableProperty.Create("Sake",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション1（項目名）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt1NamePropaty =
            BindableProperty.Create("Opt1Name",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション1（入力値）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt1ValuePropaty =
            BindableProperty.Create("Opt1Value",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション2（項目名）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt2NamePropaty =
            BindableProperty.Create("Opt2Name",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション2（入力値）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt2ValuePropaty =
            BindableProperty.Create("Opt2Value",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション3（項目名）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt3NamePropaty =
            BindableProperty.Create("Opt3Name",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション3（入力値）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt3ValuePropaty =
            BindableProperty.Create("Opt3Value",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション4（項目名）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt4NamePropaty =
            BindableProperty.Create("Opt4",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション4（入力値）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt4ValuePropaty =
            BindableProperty.Create("Opt4Value",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション5（項目名）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt5NamePropaty =
            BindableProperty.Create("Opt5Name",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション5（入力値）プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty Opt5ValuePropaty =
            BindableProperty.Create("Opt5Value",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 遅刻プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty AnsLatePropaty =
            BindableProperty.Create("AnsLate",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 早退プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty AnsEarlyPropaty =
            BindableProperty.Create("AnsEarly",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 本人以外の参加プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty OtherCountPropaty =
            BindableProperty.Create("OtherCount",
                                    typeof(int),
                                    typeof(EventPageMeeting),
                                    0);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 回答期限プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty AnsDatePropaty =
            BindableProperty.Create("AnsDate",
                                    typeof(string),
                                    typeof(EventPageMeeting),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty LabelFontSizePropaty =
            BindableProperty.Create("LabelFontSize",
                                    typeof(double),
                                    typeof(EventPageDirect),
                                    0.0);


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // Utilityクラス
        private LAUtility _utl;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データNo
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int DataNo { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベントデータNo
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int EventDataNo { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会日
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Date
        {
            get => (string)GetValue(EventPageMeeting.DatePropaty);
            set => SetValue(EventPageMeeting.DatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 中止
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Cancel
        {
            get => (string)GetValue(EventPageMeeting.CancelPropaty);
            set => SetValue(EventPageMeeting.CancelPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会場所
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Place
        {
            get => (string)GetValue(EventPageMeeting.PlacePropaty);
            set => SetValue(EventPageMeeting.PlacePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会回数
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int Count
        {
            get => (int)GetValue(EventPageMeeting.CountPropaty);
            set => SetValue(EventPageMeeting.CountPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会名
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Name
        {
            get => (string)GetValue(EventPageMeeting.NamePropaty);
            set => SetValue(EventPageMeeting.NamePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会議方法
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Online
        {
            get => (string)GetValue(EventPageMeeting.OnlinePropaty);
            set => SetValue(EventPageMeeting.OnlinePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会議方法（高さ）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public GridLength Rdh_Online
        {
            get => (GridLength)GetValue(EventPageMeeting.Rwh_OnlinePropaty);
            set => SetValue(EventPageMeeting.Rwh_OnlinePropaty, value);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// お酒
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Sake
        {
            get => (string)GetValue(EventPageMeeting.SakePropaty);
            set => SetValue(EventPageMeeting.SakePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション1（項目名）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Opt1Name
        {
            get => (string)GetValue(EventPageMeeting.Opt1NamePropaty);
            set => SetValue(EventPageMeeting.Opt1NamePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション1（入力値）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Opt1Value
        {
            get => (string)GetValue(EventPageMeeting.Opt1ValuePropaty);
            set => SetValue(EventPageMeeting.Opt1ValuePropaty, value);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 回答期限
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string AnsDate
        {
            get => (string)GetValue(EventPageMeeting.AnsDatePropaty);
            set => SetValue(EventPageMeeting.AnsDatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public double LabelFontSize
        {
            get => (double)GetValue(EventPageMeeting.LabelFontSizePropaty);
            set => SetValue(EventPageMeeting.LabelFontSizePropaty, value);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventret"></param>
        /// <param name="meetingschedule"></param>
        /// <param name="labelFontSizse"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventPageMeeting(ref Table.T_EVENTRET eventret, 
                                ref Table.T_MEETINGSCHEDULE meetingschedule, 
                                double labelFontSizse)
        {
            InitializeComponent();

            // Content Utilクラス生成
            _utl = new LAUtility();

            SetMeeting(ref eventret, ref meetingschedule, labelFontSizse);

            ControlTemplate = Resources["MeetingTemplate"] as ControlTemplate;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 各項目の設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetMeeting(ref Table.T_EVENTRET eventret,
                                ref Table.T_MEETINGSCHEDULE meetingschedule,
                                double labelFontSizse)
        {
            string wkDate = string.Empty;
            string wkCancel = string.Empty;
            int wkCount = 0;
            string wkPlace = string.Empty;
            string wkName = string.Empty;
            string wkOnline = string.Empty;
            string wkSake = string.Empty;
            string wkOpt1Name = string.Empty;
            string wkOpt1Value = string.Empty;
            string wkAnsDate = string.Empty;


            // データNo.
            DataNo = eventret.DataNo;

            // イベントデータNo.
            EventDataNo = eventret.EventDataNo;

            // 例会日
            wkDate = _utl.GetString(meetingschedule.MeetingDate).Substring(0, 10) + " " +
                     _utl.GetString(meetingschedule.MeetingTime).Substring(0, 5) + "～";

            // 中止
            wkCancel = _utl.StrCancel(meetingschedule.CancelFlg);

            // 例会場所
            wkPlace = _utl.GetString(meetingschedule.MeetingPlace);

            // 区分
            wkCount = meetingschedule.MeetingCount;

            // 例会名
            wkName = _utl.GetString(meetingschedule.MeetingName);

            // 会議方法
            wkOnline = _utl.StrOnline(meetingschedule.Online);
            if (wkOnline == string.Empty)
            {
                //Rdh_Online = GridLength(0.0);
            }
            else
            {
                //Rdh_Online = GridLength.Auto;
            }
            
            // お酒
            wkSake = _utl.StrOnOff(meetingschedule.Sake);

            // オプション1
            if (_utl.GetString(meetingschedule.OptionRadio1).Equals(_utl.ONFLG))
            {
                // オプション1（項目名）
                wkOpt1Name = _utl.GetString(meetingschedule.OptionName1);

                // オプション1（入力値）
                wkOpt1Value = _utl.GetString(eventret.Option1);
            }
            else
            {

            }

            // 回答期限
            wkAnsDate = _utl.GetString(meetingschedule.AnswerDate).Substring(0, 10);


            Date = wkDate;
            Cancel = wkCancel;
            Place = wkPlace;
            Count = wkCount;
            Name = wkName;
            Online = wkOnline;
            Sake = wkSake;
            AnsDate = wkAnsDate;
            Opt1Name = wkOpt1Name;
            Opt1Value = wkOpt1Value;

        }


    }
}