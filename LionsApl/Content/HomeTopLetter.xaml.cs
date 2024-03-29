﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTopLetter : ContentView
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
                                    typeof(HomeTopLetter),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タイトルプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty TitlePropaty =
            BindableProperty.Create("Title",
                                    typeof(string),
                                    typeof(HomeTopLetter),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty LabelFontSizePropaty =
            BindableProperty.Create("LabelFontSize",
                                    typeof(double),
                                    typeof(HomeTopLetter),
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
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTopLetter(int dataNo, string eventDate, string title, double labelFontSizse)
        {
            InitializeComponent();

            DataNo = dataNo;
            EventDate =eventDate;
            Title = title;
            LabelFontSize = labelFontSizse;

            ControlTemplate = Resources["LetterTemplate"] as ControlTemplate;
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
            get => (string)GetValue(HomeTopLetter.EventDatePropaty);
            set => SetValue(HomeTopLetter.EventDatePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タイトル
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string Title
        {
            get => (string)GetValue(HomeTopLetter.TitlePropaty);
            set => SetValue(HomeTopLetter.TitlePropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public double LabelFontSize
        {
            get => (double)GetValue(HomeTopLetter.LabelFontSizePropaty);
            set => SetValue(HomeTopLetter.LabelFontSizePropaty, value);
        }

    }
}