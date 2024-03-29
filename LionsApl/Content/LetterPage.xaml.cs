﻿using System;
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
        public static String FilePath_Letter = ((App)Application.Current).FilePath_Letter;          //キャビネットレター

        // 前画面からの取得情報-
        private int _dataNo;        // データNo.

        public LetterPage(int dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Body.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 一覧から取得(データ№)
            _dataNo = dataNo;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_Letter);

            // キャビネットレター情報設定
            GetLetter();

            // ツールバーに一覧ボタンを設定
            ToolbarItems.Add(new ToolbarItem { Text = "一覧", Command = new Command(Push_LetterList) });

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 処理中表示制御
            if (Image1.IsLoading)
            {
                stack1.IsVisible = true;
            }
            if (Image2.IsLoading) 
            {
                stack2.IsVisible = true;
            }

            while (Image1.IsLoading || Image2.IsLoading)
            {
                await Task.Delay(10);
                if (!Image1.IsLoading)
                {
                    stack1.IsVisible = false;
                }
                if (!Image2.IsLoading)
                {
                    stack2.IsVisible = false;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetLetter()
        {

            // 変数
            string wkDataNo = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "Where DataNo='" + _dataNo + "'"))
                {
                    wkDataNo = row.DataNo.ToString();
                    if(Util.GetString(row.EventTime).Substring(0,5) == "00:00")
                    {
                        EventDate.Text = Util.GetString(row.EventDate).Substring(0, 10);
                    }
                    else
                    {
                        EventDate.Text = Util.GetString(row.EventDate).Substring(0, 10) + " " + Util.GetString(row.EventTime);
                    }
                    Subject.Text = Util.GetString(row.Title);
                    Body.Text = Util.GetString(row.Body);

                    // FILEPATH生成
                    var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");

                    //画像ファイル①
                    if (Util.GetString(row.Image1FileName) != "")
                    {
                        string uriStr = AppServer + filepath + "/" + wkDataNo + "/" + Util.GetString(row.Image1FileName);
                        Image1.Source = ImageSource.FromUri(new Uri(uriStr));
                    }

                    //画像ファイル②
                    if (Util.GetString(row.Image2FileName) != "")
                    {
                        string uriStr = AppServer + filepath + "/" + wkDataNo + "/" + Util.GetString(row.Image2FileName);
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