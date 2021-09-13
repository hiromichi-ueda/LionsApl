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
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                      //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                    //PdfViewer
        public static String FilePath_Letter = ((App)Application.Current).FilePath_Letter;          //連絡事項(CLUB)

        // 前画面からの取得情報-
        //private string _titleName;      // タイトル 
        private string _DataNo;        // データNo.

        public LetterPage(string dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Body.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));


            //_titleName = title;
            _DataNo = dataNo;

            //TitleLabel.Text = _titleName;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_Letter);

            // キャビネットレター情報設定
            GetLetter();

            // ツールバーに一覧ボタンを設定
            ToolbarItems.Add(new ToolbarItem { Text = "一覧", Command = new Command(Push_LetterList) });

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetLetter()
        {
            try
            {
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "Where DataNo='" + _DataNo + "'"))
                {
                    EventDate.Text = row.EventDate.Substring(0, 10) + " " + row.EventTime;
                    Subject.Text = row.Title;
                    Body.Text = row.Body;

                    //画像ファイル①
                    if (row.Image1FileName != null)
                    {
                        string uriStr = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/") +
                                        "/" + row.DataNo.ToString() + "/" + row.Image1FileName;
                        Image1.Source = ImageSource.FromUri(new Uri(uriStr));
                    }

                    //画像ファイル②
                    if (row.Image2FileName != null)
                    {
                        string uriStr = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/") +
                                        "/" + row.DataNo.ToString() + "/" + row.Image2FileName;
                        Image2.Source = ImageSource.FromUri(new Uri(uriStr));
                    }
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