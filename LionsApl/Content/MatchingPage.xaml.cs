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
    public partial class MatchingPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        public event EventHandler<ModalPoppingEventArgs> ModalPoping;

        public MatchingPage(string strHP)
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // 選択URL設定
            SelectHPWebView.Source = strHP;

            ModalPoping += HandleModalPoping;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ×ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Clicked(object sender, System.EventArgs e)
        {

            await Navigation.PopModalAsync();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面を離れる処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void HandleModalPoping(object sender, ModalPoppingEventArgs e)
        {
            //明示的にこのActivityを削除
            Navigation.RemovePage(Navigation.NavigationStack[0]);

        }
    }
}