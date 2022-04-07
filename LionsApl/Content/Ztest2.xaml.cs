using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZTest2 : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;


        public ZTest2()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();


            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;
        }

    }
}