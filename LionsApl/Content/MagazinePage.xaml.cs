using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MagazinePage : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        // 前画面からの取得情報
        private int _dataNo;                                 // データNo.

        public MagazinePage(int InDataNo)
        {
            InitializeComponent();

            _dataNo = InDataNo;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // ツールバーに一覧ボタンを設定
            ToolbarItems.Add(new ToolbarItem { Text = "一覧", Command = new Command(Push_MagazineList) });

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(_sqlite.DATACLASS_MAGAZINE);

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // 地区誌情報設定
            GetMagazine();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMagazine()
        {
            try
            {
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("Select * " +
                                                                        "From T_MAGAZINE " +
                                                                        "Where DataNo='" + _dataNo + "'"))
                {
                    if (row.FileName != null)
                    {
                        string urlStr = "http://ap.insat.co.jp" + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/") + "/" + row.FileName;
                        string encodeUrl = System.Net.WebUtility.UrlEncode(urlStr);
                        string source = "https://docs.google.com/viewer?url=" + encodeUrl + "&embedded=true";
                        //string source = urlStr;
                        PdfFrame.Resources.Source = new Uri(source);


                        PdfLabel.Text = _sqlite.Db_A_FilePath.FilePath + "/" + row.FileName + "\r\n" + source;

                    }
                    else
                    {
                        PdfLabel.Text = "写真なし";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }

        private void Push_MagazineList()
        {
            Navigation.PushAsync(new MagazineList());
        }
    }
}