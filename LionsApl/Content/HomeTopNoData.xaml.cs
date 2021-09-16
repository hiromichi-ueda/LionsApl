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
        public static readonly BindableProperty LabelTextPropaty =
            BindableProperty.Create("LabelText",
                                    typeof(string),
                                    typeof(HomeTopNoData),
                                    string.Empty);

        public HomeTopNoData(string labelText)
        {
            InitializeComponent();

            LabelText = labelText;

            ControlTemplate = Resources["NoDataTemplate"] as ControlTemplate;

        }
        public string LabelText
        {
            get => (string)GetValue(HomeTopNoData.LabelTextPropaty);
            set => SetValue(HomeTopNoData.LabelTextPropaty, value);
        }
    }
}