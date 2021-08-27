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

    public partial class ClubTop : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ClubTop()
        {
            InitializeComponent();

            // キャビネット名
            //Title = _sqlite.Db_A_Setting.CabinetName;

        }

    }

}