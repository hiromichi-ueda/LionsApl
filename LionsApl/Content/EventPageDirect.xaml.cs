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
    public partial class EventPageDirect : ContentView
    {


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// バインダブルプロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 開催日プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty DatePropaty =
            BindableProperty.Create("Date",
                                    typeof(string),
                                    typeof(EventPageDirect),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 中止プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty CancelPropaty =
            BindableProperty.Create("Cancel",
                                    typeof(string),
                                    typeof(EventPageDirect),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 区分プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty SeasonPropaty =
            BindableProperty.Create("Season",
                                    typeof(string),
                                    typeof(EventPageDirect),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 開催場所プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty PlacePropaty =
            BindableProperty.Create("Place",
                                    typeof(string),
                                    typeof(EventPageDirect),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 議題・内容プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty AgendaPropaty =
            BindableProperty.Create("Agenda",
                                    typeof(string),
                                    typeof(EventPageDirect),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 回答期限プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty AnsDatePropaty =
            BindableProperty.Create("AnsDate",
                                    typeof(string),
                                    typeof(EventPageDirect),
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
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo"></param>
        /// <param name="date"></param>
        /// <param name="cancel"></param>
        /// <param name="season"></param>
        /// <param name="place"></param>
        /// <param name="agenda"></param>
        /// <param name="ansdate"></param>
        /// <param name="labelFontSizse"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventPageDirect(int dataNo, 
                               string date, 
                               string cancel,
                               string season,
                               string place,
                               string agenda,
                               string ansdate,
                               double labelFontSizse)
        {
            InitializeComponent();

            DataNo = dataNo;
            Date = date;
            Cancel = cancel;
            Season = season;
            Place = place;
            Agenda = agenda;
            AnsDate = ansdate;
            LabelFontSize = labelFontSizse;

            ControlTemplate = Resources["DirectTemplate"] as ControlTemplate;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データNo
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int DataNo { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 開催日
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Date
        {
            get => (string)GetValue(EventPageDirect.DatePropaty);
            set => SetValue(EventPageDirect.DatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 中止
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Cancel
        {
            get => (string)GetValue(EventPageDirect.CancelPropaty);
            set => SetValue(EventPageDirect.CancelPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 区分
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Season
        {
            get => (string)GetValue(EventPageDirect.SeasonPropaty);
            set => SetValue(EventPageDirect.SeasonPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 開催場所
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Place
        {
            get => (string)GetValue(EventPageDirect.PlacePropaty);
            set => SetValue(EventPageDirect.PlacePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 議題・内容
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Agenda
        {
            get => (string)GetValue(EventPageDirect.AgendaPropaty);
            set => SetValue(EventPageDirect.AgendaPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 回答期限
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string AnsDate
        {
            get => (string)GetValue(EventPageDirect.AnsDatePropaty);
            set => SetValue(EventPageDirect.AnsDatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public double LabelFontSize
        {
            get => (double)GetValue(EventPageDirect.LabelFontSizePropaty);
            set => SetValue(EventPageDirect.LabelFontSizePropaty, value);
        }

    }
}