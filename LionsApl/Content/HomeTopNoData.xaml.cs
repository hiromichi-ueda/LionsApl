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
    public partial class HomeTopNoData : ContentView
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// バインダブルプロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルテキストプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty LabelTextPropaty =
            BindableProperty.Create("LabelText",
                                    typeof(string),
                                    typeof(HomeTopNoData),
                                    string.Empty);

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズプロパティ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static readonly BindableProperty LabelFontSizePropaty =
            BindableProperty.Create("LabelFontSize",
                                    typeof(double),
                                    typeof(HomeTopNoData),
                                    0.0);


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="labelText"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTopNoData(string labelText, double labelFontSizse)
        {
            InitializeComponent();

            LabelText = labelText;
            LabelFontSize = labelFontSizse;

            ControlTemplate = Resources["NoDataTemplate"] as ControlTemplate;

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルテキスト
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string LabelText
        {
            get => (string)GetValue(HomeTopNoData.LabelTextPropaty);
            set => SetValue(HomeTopNoData.LabelTextPropaty, value);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルフォントサイズ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public double LabelFontSize
        {
            get => (double)GetValue(HomeTopNoData.LabelFontSizePropaty);
            set => SetValue(HomeTopNoData.LabelFontSizePropaty, value);
        }

    }
}