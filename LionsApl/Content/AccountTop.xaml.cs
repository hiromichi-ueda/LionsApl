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
    public partial class AccountTop : ContentPage
    {
        public AccountTop()
        {
            InitializeComponent();
        }

        //---------------------------------------
        // アカウント設定画面(編集)
        //---------------------------------------
        void Button_Edit_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new AccountSetting();
        }

    }
}