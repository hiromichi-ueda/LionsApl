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
    public partial class HomeTopLetter : ContentView
    {
        public static readonly BindableProperty EventDatePropaty =
            BindableProperty.Create("EventDate",
                                    typeof(string),
                                    typeof(HomeTopLetter),
                                    string.Empty);

        public static readonly BindableProperty TitlePropaty =
            BindableProperty.Create("Title",
                                    typeof(string),
                                    typeof(HomeTopLetter),
                                    string.Empty);

        public HomeTopLetter(int dataNo, string eventDate, string title)
        {
            InitializeComponent();

            DataNo = dataNo;
            EventDate =eventDate;
            Title = title;
            ControlTemplate = Resources["LetterTemplate"] as ControlTemplate;
        }

        public int DataNo { get; set; }

        public string EventDate
        {
            get => (string)GetValue(HomeTopLetter.EventDatePropaty);
            set => SetValue(HomeTopLetter.EventDatePropaty, value);
        }

        public string Title
        {
            get => (string)GetValue(HomeTopLetter.TitlePropaty);
            set => SetValue(HomeTopLetter.TitlePropaty, value);
        }

    }
}