using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ztest1 : ContentPage
    {
        //----------------
        /// プロパティ
        //----------------

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                      //Url
        public static String FilePath_Matching = ((App)Application.Current).FilePath_Matching;          //キャビネットレター

        // リストビュー設定内容
        public ObservableCollection<MatchingARow> Items { get; set; }

        private bool _pickerSelect = false;
        private string _selArea = null;
        private string _selJob = null;
        private string _nameDefult = "選択してください";
        private string[] _areaList;
        private string[] _jobList;

        //----------------
        /// メソッド
        //----------------
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////

        public Ztest1()
        {
            InitializeComponent();

            // font-size(<ListView>はCSSが効かないのでここで設定)
            this.LoginInfo.FontSize = 16.0;
            this.title.FontSize = 16.0;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_Matching);

            // font-size
            AreaLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            AreaPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
            JobLabel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            JobPicker.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Picker));

            // ピッカーセレクト処理OFF
            _pickerSelect = false;

            // 地域選択リスト作成
            SetAreaInfo();
            // 職種選択リスト作成
            SetJobInfo();

            // マッチング情報データ取得
            GetMatchig();

            // ピッカーセレクト処理ON
            _pickerSelect = true;
        }


        private async void AreaPickerClicked(object sender, EventArgs e)
        {
            string selA = await DisplayActionSheet("地域選択", null, "キャンセル", _areaList[0], _areaList[1]);
            if(selA == "キャンセル")
            {
                return;
            }
            AreaPicker.Text = selA;
            if (selA == _nameDefult)
            {
                AreaPicker.TextColor = Color.Gray;
            }
            else
            {
                AreaPicker.TextColor = Color.Black;
            }
            _selArea = selA;

        }

        private async void JobPickerClicked(object sender, EventArgs e)
        {
            await DisplayAlert("マッチング検索", "検索が完了しました。", "OK");

            /*
            string selJ = await DisplayActionSheet("職種選択", null, "キャンセル", _jobList[0], _jobList[1]);
            if (selJ == "キャンセル")
            {
                return;
            }
            JobPicker.Text = selJ;
            if (selJ == _nameDefult)
            {
                JobPicker.TextColor = Color.Gray;
            }
            else
            {
                JobPicker.TextColor = Color.Black;
            }
            _selJob = selJ;
            */
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// マッチング情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMatchig()
        {
            string strSQL;
            long WorkDataNo = 0;
            string strCompanyName = string.Empty;
            string strComment = string.Empty;
            string menberName = string.Empty;
            string FileName = string.Empty;
            string hp = string.Empty;

            try
            {

                if (_pickerSelect == false)
                {
                    //初期表示
                    Items = new ObservableCollection<MatchingARow>();

                    // メッセージ表示のため空行を追加
                    WorkDataNo = 9999999999;
                    Items.Add(new MatchingARow(WorkDataNo, strCompanyName, strComment, menberName, FileName, hp));

                    BindingContext = this;
                    //MatchingListView.ItemsSource = Items;

                }
                else
                {
                    //現在のデータ削除
                    var befItems = Items.ToList();
                    foreach (var befItem in befItems)
                    {
                        Items.Remove(befItem);
                    }

                    strSQL = "Select * From T_MATCHING WHERE ";
                    if (_selArea != _nameDefult)
                    {
                        strSQL += "Area = '" + _selArea + "' ";
                        if (_selJob != _nameDefult)
                        {
                            strSQL += "AND ";
                        }
                    }
                    if (_selJob != _nameDefult)
                    {
                        strSQL += "JobName like '%" + _selJob + "%' ";
                    }
                    strSQL += "ORDER BY DataNo ASC";

                    foreach (Table.T_MATCHING row in _sqlite.Get_T_MATCHING(strSQL))
                    {
                        WorkDataNo = row.DataNo;
                        strCompanyName = _utl.GetString(row.CompanyName);
                        strComment = _utl.GetString(row.Comment);
                        menberName = _utl.GetString(row.ClubNameShort) + " " + _utl.GetString(row.MemberName);
                        if (_utl.GetString(row.HP) == string.Empty || _utl.GetString(row.HP).Length == 0)
                        {
                            menberName = "[HPなし] " + menberName;
                        }

                        // FILEPATH生成
                        string filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");
                        if (_utl.GetString(row.FileName) != string.Empty && _utl.GetString(row.FileName).Trim().Length > 0)
                        {
                            FileName = AppServer + filepath + "/" + row.DataNo.ToString() + "/" + _utl.GetString(row.FileName);
                        }
                        else
                        {
                            //No image画像
                            FileName = "noimage.png";
                        }

                        hp = _utl.GetString(row.HP);

                        Items.Add(new MatchingARow(WorkDataNo, strCompanyName, strComment, menberName, FileName, hp));
                    }
                    if (Items.Count == 0)
                    {
                        // メッセージ表示のため空行を追加
                        Items.Add(new MatchingARow(WorkDataNo, strCompanyName, strComment, menberName, FileName, hp));
                    }
                }

                BindingContext = this;

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MATCHING) : {ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地域情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetAreaInfo()
        {
            // データ取得
            try
            {
                _areaList = new string[1];
                _areaList[_areaList.Length-1] = _nameDefult;

                foreach (Table.M_AREA row in _sqlite.Get_M_AREA("Select AreaName From M_AREA ORDER BY SortNo"))
                {
                    Array.Resize(ref _areaList, _areaList.Length + 1);
                    _areaList[_areaList.Length-1] =row.AreaName;
                }
                // AreaPickerにCRegionPickerクラスを設定する
                _selArea = _nameDefult;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(地域) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 職種情報取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetJobInfo()
        {
            // データ取得
            try
            {
                _jobList = new string[1];
                _jobList[_jobList.Length-1] = _nameDefult;

                foreach (Table.M_JOB row in _sqlite.Get_M_JOB("Select JobName From M_JOB ORDER BY JobCode"))
                {
                    Array.Resize(ref _jobList, _jobList.Length + 1);
                    _jobList[_jobList.Length-1] = row.JobName;
                }
                // JobPickerにCRegionPickerクラスを設定する
                _selJob = _nameDefult;
                _selJob = "製造業";
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(職種) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //対象のHPへSafari起動
            if (e.Item == null)
                return;

            MatchingARow item = e.Item as MatchingARow;

            if (item.DataNo == 0 || item.DataNo == 9999999999)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }
            if (item.HP.Length == 0 || item.HP == string.Empty)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            //マッチング画面へ遷移
            //Navigation.PushAsync(new MatchingPage(item.HP));
            Navigation.PushModalAsync(new MatchingPage(item.HP));
            //Browser.OpenAsync(item.HP, BrowserLaunchMode.SystemPreferred);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;


        }

       ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Button_S_Clicked(object sender, System.EventArgs e)
        {

            if ((_selArea == null || _selArea == _nameDefult) && (_selJob == null || _selJob == _nameDefult))
            {
                await DisplayAlert("マッチング検索", "地域、または職種の条件を選択してください。", "OK");
            }
            else
            {
                // マッチング情報データ取得
                GetMatchig();

                await DisplayAlert("マッチング検索", "検索が完了しました。", "OK");
            }

        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// マッチング行情報クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class MatchingARow
    {
        public MatchingARow(long dataNo, string companyName, string comment, string memberName, string fileName, string hp)
        {
            DataNo = dataNo;
            CompanyName = companyName;
            Comment = comment;
            MemberName = memberName;
            FileName = fileName;
            HP = hp;
        }
        public long DataNo { get; set; }
        public string CompanyName { get; set; }
        public string Comment { get; set; }
        public string MemberName { get; set; }
        public string FileName { get; set; }
        public string HP { get; set; }
    }

    public class MyMatchingASelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate LoadTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (MatchingARow)item;
            if (info.DataNo == 9999999999)
            {
                return LoadTemplate;
            }
            else
            {
                if (info.DataNo > 0)
                {
                    return ExistDataTemplate;
                }
                else
                {
                    return NoDataTemplate;
                }
            }

        }
    } 
}