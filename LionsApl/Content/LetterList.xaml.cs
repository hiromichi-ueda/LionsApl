﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LetterList : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス


        public ObservableCollection<string> Items { get; set; }

        public LetterList()
        {
            InitializeComponent();

            //Items = new ObservableCollection<string>
            //{
            //    "Item 1",
            //    "Item 2",
            //    "Item 3",
            //    "Item 4",
            //    "Item 5"
            //};

            //MyListView.ItemsSource = Items;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            GetLetter();

        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            LetterRow item = e.Item as LetterRow;

            Navigation.PushAsync(new LetterPage(item.Title, item.DataNo));

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetLetter()
        {
            int WorkDataNo;
            string WorkDate;
            string WorkTitle;
            List<LetterRow> items = new List<LetterRow>();

            try
            {
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "ORDER BY EventDate DESC, EventTime DESC, DataNo DESC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkDate = row.EventDate + " " + row.EventTime;
                    WorkTitle = row.Title;
                    items.Add(new LetterRow(WorkDataNo, WorkDate, WorkTitle));
                }
                LetterListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }

    }



    public sealed class LetterRow
    {
        public LetterRow(int dataNo, string dateTime, string title)
        {
            DataNo = dataNo;
            DateTime = dateTime;
            Title = title;
        }
        public int DataNo { get; set; }
        public string DateTime { get; set; }
        public string Title { get; set; }
    }
}
