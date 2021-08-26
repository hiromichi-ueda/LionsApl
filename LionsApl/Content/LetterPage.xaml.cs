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
    public partial class LetterPage : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        // 前画面からの取得情報
        private string _titleName;                           // タイトル
        private int _dataNo;                                 // データNo.

        public LetterPage(string InTitle, int InDataNo)
        {
            InitializeComponent();

            _titleName = InTitle;
            _dataNo = InDataNo;

            TitleLabel.Text = _titleName;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // ツールバーに一覧ボタンを設定
            ToolbarItems.Add(new ToolbarItem { Text = "一覧", Command = new Command(Push_LetterList) });

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // キャビネットレター情報設定
            SetLetter();

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetLetter()
        {
            try
            {
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "Where DataNo='" + _dataNo + "'"))
                {
                    DateLabel.Text = row.EventDate + " " + row.EventTime;
                    BodyLabel.Text = row.Body;
                    if (row.Image1FileName == null)
                    {
                        DisplayAlert("Alert", "Image1はnull 1", "OK");
                    }
                    if (row.Image1FileName.Equals(null))
                    {
                        DisplayAlert("Alert", "Image1はnull 2", "OK");
                    }
                    if (row.Image1FileName == string.Empty)
                    {
                        DisplayAlert("Alert", "Image1は空 1", "OK");
                    }
                    if (row.Image1FileName.Equals(string.Empty))
                    {
                        DisplayAlert("Alert", "Image1は空 2", "OK");
                    }
                    //Image1.Source = ImageSource.FromUri(new Uri());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }

        private void Push_LetterList()
        {
            Navigation.PushAsync(new LetterList());
        }
    }
}