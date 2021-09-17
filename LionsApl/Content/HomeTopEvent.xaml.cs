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
    public partial class HomeTopEvent : ContentView
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// バインダブルプロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 開催日プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////

        public static readonly BindableProperty EventDatePropaty =
            BindableProperty.Create("EventDate",
                            typeof(string),
                            typeof(HomeTopEvent),
                            string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タイトルプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////

        public static readonly BindableProperty TitlePropaty =
            BindableProperty.Create("Title",
                                    typeof(string),
                                    typeof(HomeTopEvent),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 日数プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty CountDtPropaty =
            BindableProperty.Create("CountDt",
                            typeof(string),
                            typeof(HomeTopEvent),
                            string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 中止プロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty CancelPropaty =
            BindableProperty.Create("Cancel",
                            typeof(string),
                            typeof(HomeTopEvent),
                            string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty LabelFontSizePropaty =
            BindableProperty.Create("LabelFontSize",
                                    typeof(double),
                                    typeof(HomeTopEvent),
                                    0.0);


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo"></param>
        /// <param name="eventDate"></param>
        /// <param name="title"></param>
        /// <param name="countDt"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTopEvent(int dataNo, string eventDate, string title, string countDt, string cancel, double labelFontSizse)
        {
            InitializeComponent();

            DataNo = dataNo;
            EventDate = eventDate;
            Title = title;
            CountDt = countDt;
            Cancel = cancel;
            LabelFontSize = labelFontSizse;

            ControlTemplate = Resources["EventTemplate"] as ControlTemplate;
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
        public string EventDate
        {
            get => (string)GetValue(HomeTopEvent.EventDatePropaty);
            set => SetValue(HomeTopEvent.EventDatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タイトル
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Title
        {
            get => (string)GetValue(HomeTopEvent.TitlePropaty);
            set => SetValue(HomeTopEvent.TitlePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 日数
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string CountDt
        {
            get => (string)GetValue(HomeTopEvent.CountDtPropaty);
            set => SetValue(HomeTopEvent.CountDtPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャンセル
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Cancel
        {
            get => (string)GetValue(HomeTopEvent.CancelPropaty);
            set => SetValue(HomeTopEvent.CancelPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public double LabelFontSize
        {
            get => (double)GetValue(HomeTopEvent.LabelFontSizePropaty);
            set => SetValue(HomeTopEvent.LabelFontSizePropaty, value);
        }

    }
}