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
    public partial class ClubSchedulePage : ContentPage
    {
        public ClubSchedulePage(string title, int dataNo)
        {
            InitializeComponent();
        }
    }
}