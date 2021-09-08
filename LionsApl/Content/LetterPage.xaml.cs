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

        // url取得
        public static String AppServer = ((App)Application.Current).AppServer;

        // 前画面からの取得情報
        private string _titleName;  // タイトル
        private int _dataNo;        // データNo.


        public LetterPage(string title, int dataNo)
        {
            InitializeComponent();

            // font-size
            this.LoginInfo.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));      //Login
            this.title.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));        //Title
            this.lbl_EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.DateLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Title.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.TitleLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.BodyLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));


            _titleName = title;
            _dataNo = dataNo;

            TitleLabel.Text = _titleName;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(_sqlite.DATACLASS_LETTER);

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
                                                                    "Where DataNo='" + _dataNo + "'"))
                {
                    DateLabel.Text = row.EventDate.Substring(0, 10) + " " + row.EventTime;
                    BodyLabel.Text = row.Body;
                    if (row.Image1FileName != null)
                    {
                        string uriStr = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/") + "/" + row.Image1FileName;
                        Image1.Source = ImageSource.FromUri(new Uri(uriStr));
                    }
                    if (row.Image2FileName != null)
                    {
                        string uriStr = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/") + "/" + row.Image2FileName;
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