using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LionsApl
{
    class SQLiteManager
    {

        // ログイン情報（ユーザ文字列（クラブ名＋氏名））
        public string LoginInfo;

        private static SQLiteManager _single = null;
        private HttpClient _httpClient = null;
        
        public Table.A_SETTING Db_A_Setting;                 // A_SETTINGテーブルクラス
        public Table.A_ACCOUNT Db_A_Account;                 // A_ACCOUNTテーブルクラス
        public Table.A_FILEPATH Db_A_FilePath;               // A_FILEPATHテーブルクラス
        public List<Table.T_LETTER> DbList_T_Letter = new List<Table.T_LETTER>();   // T_LETTERテーブルクラスリスト


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// テーブル作成
        ///////////////////////////////////////////////////////////////////////////////////////////

        //public string sqlliteFileName { get; set; } = "LionsAplDB.db3";
        public string DbPath { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LionsAplDB.db3");
        public static MultipartFormDataContent content;
        public static String webServiceUrl = ((App)Application.Current).WebServiceUrl;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private SQLiteManager()
        {
            _httpClient = new HttpClient();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// デストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        ~SQLiteManager()
        {
            SQLiteManager.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static SQLiteManager GetInstance()
        {
            if (_single == null)
            {
                _single = new SQLiteManager();
            }
            return _single;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リソース開放（実態）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static void Dispose()
        {
            if (_single == null)
            {
                return;
            }
            if (_single._httpClient == null)
            {
                return;
            }
            _single._httpClient.Dispose();
            _single = null;
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// テーブル作成
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_ALL()
        {
            using (SQLiteConnection db = new SQLiteConnection(DbPath))
            {
                // Create Table
                _ = db.CreateTable<Table.A_APLLOG>();
                _ = db.CreateTable<Table.A_SETTING>();
                _ = db.CreateTable<Table.A_ACCOUNT>();
                _ = db.CreateTable<Table.A_FILEPATH>();
                _ = db.CreateTable<Table.T_SLOGAN>();
                _ = db.CreateTable<Table.T_LETTER>();
                _ = db.CreateTable<Table.T_EVENTRET>();
                _ = db.CreateTable<Table.T_EVENT>();
                _ = db.CreateTable<Table.T_INFOMATION_CABI>();
                _ = db.CreateTable<Table.T_MAGAZINE>();
                _ = db.CreateTable<Table.T_MAGAZINEBUY>();
                //_ = db.CreateTable<Table.M_DISTRICTOFFICER>();
                _ = db.CreateTable<Table.M_CABINET>();
                _ = db.CreateTable<Table.M_CLUB>();
                _ = db.CreateTable<Table.T_CLUBSLOGAN>();
                _ = db.CreateTable<Table.T_MEETINGSCHEDULE>();
                _ = db.CreateTable<Table.T_DIRECTOR>();
                _ = db.CreateTable<Table.T_MEETINGPROGRAM>();
                _ = db.CreateTable<Table.T_INFOMATION_CLUB>();
                _ = db.CreateTable<Table.M_MEMBER>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（A_APLLOG）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_A_APLLOG()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.A_APLLOG>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（A_SETTING）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_A_SETTING()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.A_SETTING>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（A_ACCOUNT）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_A_ACCOUNT()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.A_ACCOUNT>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（A_FILEPATH）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_A_FILEPATH()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.A_FILEPATH>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_SLOGAN）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_SLOGAN()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_SLOGAN>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_LETTER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_LETTER()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_LETTER>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_EVENTRET）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_EVENTRET()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_EVENTRET>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_EVENT）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_EVENT()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_EVENT>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_INFOMATION_CABI）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_INFOMATION_CABI()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_INFOMATION_CABI>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_MAGAZINE）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_MAGAZINE()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_MAGAZINE>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_MAGAZINEBUY）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_MAGAZINEBUY()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_MAGAZINEBUY>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（M_DISTRICTOFFICER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //public void CreateTable_M_DISTRICTOFFICER()
        //{
        //    using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
        //    {
        //        // Create Table
        //        db.CreateTable<Table.M_DISTRICTOFFICER>();
        //    }
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（M_CABINET）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_M_CABINET()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.M_CABINET>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（M_CLUB）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_M_CLUB()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.M_CLUB>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_CLUBSLOGAN）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_CLUBSLOGAN()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_CLUBSLOGAN>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_MEETINGSCHEDULE）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_MEETINGSCHEDULE()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_MEETINGSCHEDULE>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_DIRECTOR）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_DIRECTOR()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_DIRECTOR>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_MEETINGPROGRAM）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_MEETINGPROGRAM()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_MEETINGPROGRAM>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（T_INFOMATION_CLUB）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_T_INFOMATION_CLUB()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.T_INFOMATION_CLUB>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル作成（M_MEMBER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTable_M_MEMBER()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.CreateTable<Table.M_MEMBER>();
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// テーブル削除
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Create Table
                db.DropTable<Table.A_APLLOG>();
                db.DropTable<Table.A_SETTING>();
                db.DropTable<Table.A_ACCOUNT>();
                db.DropTable<Table.A_FILEPATH>();
                db.DropTable<Table.T_SLOGAN>();
                db.DropTable<Table.T_LETTER>();
                db.DropTable<Table.T_EVENTRET>();
                db.DropTable<Table.T_EVENT>();
                db.DropTable<Table.T_INFOMATION_CABI>();
                db.DropTable<Table.T_MAGAZINE>();
                db.DropTable<Table.T_MAGAZINEBUY>();
                //db.DropTable<Table.M_DISTRICTOFFICER>();
                db.DropTable<Table.M_CABINET>();
                db.DropTable<Table.M_CLUB>();
                db.DropTable<Table.T_CLUBSLOGAN>();
                db.DropTable<Table.T_MEETINGSCHEDULE>();
                db.DropTable<Table.T_DIRECTOR>();
                db.DropTable<Table.T_MEETINGPROGRAM>();
                db.DropTable<Table.T_INFOMATION_CLUB>();
                db.DropTable<Table.M_MEMBER>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（A_APLLOG）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_A_APLLOG()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.A_APLLOG>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（A_SETTING）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_A_SETTING()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.A_SETTING>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（A_ACCOUNT）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_A_ACCOUNT()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.A_ACCOUNT>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（A_FILEPATH）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_A_FILEPATH()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.A_FILEPATH>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_SLOGAN）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_SLOGAN()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_SLOGAN>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_LETTER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_LETTER()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_LETTER>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_EVENTRET）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_EVENTRET()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_EVENTRET>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_EVENT）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_EVENT()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_EVENT>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_INFOMATION_CABI）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_INFOMATION_CABI()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_INFOMATION_CABI>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_MAGAZINE）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_MAGAZINE()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_MAGAZINE>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_MAGAZINEBUY）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_MAGAZINEBUY()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_MAGAZINEBUY>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（M_DISTRICTOFFICER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //public void DropTable_M_DISTRICTOFFICER()
        //{
        //    using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
        //    {
        //        // Drop Table
        //        db.DropTable<Table.M_DISTRICTOFFICER>();
        //    }
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（M_CABINET）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_M_CABINET()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.M_CABINET>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（M_CLUB）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_M_CLUB()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.M_CLUB>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_CLUBSLOGAN）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_CLUBSLOGAN()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_CLUBSLOGAN>();
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_MEETINGSCHEDULE）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_MEETINGSCHEDULE()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_MEETINGSCHEDULE>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_DIRECTOR）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_DIRECTOR()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_DIRECTOR>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_MEETINGPROGRAM）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_MEETINGPROGRAM()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_MEETINGPROGRAM>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（T_INFOMATION_CLUB）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_T_INFOMATION_CLUB()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.T_INFOMATION_CLUB>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// データベーステーブル削除（M_MEMBER）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void DropTable_M_MEMBER()
        {
            using (SQLiteConnection db = new SQLite.SQLiteConnection(DbPath))
            {
                // Drop Table
                db.DropTable<Table.M_MEMBER>();
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// データ取得
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A_APLLOGテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.A_APLLOG[] Get_A_APLLOG(string command)
        {
            List<Table.A_APLLOG> items = new List<Table.A_APLLOG>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.A_APLLOG>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.A_APLLOG[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A_ACCOUNTテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.A_ACCOUNT[] Get_A_ACCOUNT(string command)
        {
            List<Table.A_ACCOUNT> items = new List<Table.A_ACCOUNT>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.A_ACCOUNT>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.A_ACCOUNT[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A_SETTINGテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.A_SETTING[] Get_A_SETTING(string command)
        {
            List<Table.A_SETTING> items = new List<Table.A_SETTING>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.A_SETTING>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.A_SETTING[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A_FILEPATHテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.A_FILEPATH[] Get_A_FILEPATH(string command)
        {
            List<Table.A_FILEPATH> items = new List<Table.A_FILEPATH>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.A_FILEPATH>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.A_FILEPATH[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_SLOGANテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_SLOGAN[] Get_T_SLOGAN(string command)
        {
            List<Table.T_SLOGAN> items = new List<Table.T_SLOGAN>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_SLOGAN>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_SLOGAN[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_LETTERテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_LETTER[] Get_T_LETTER(string command)
        {
            List<Table.T_LETTER> items = new List<Table.T_LETTER>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_LETTER>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_LETTER[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_EVENTRETテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_EVENTRET[] Get_T_EVENTRET(string command)
        {
            List<Table.T_EVENTRET> items = new List<Table.T_EVENTRET>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_EVENTRET>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_EVENTRET[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_EVENTテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_EVENT[] Get_T_EVENT(string command)
        {
            List<Table.T_EVENT> items = new List<Table.T_EVENT>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_EVENT>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_EVENT[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_INFOMATION_CABIテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_INFOMATION_CABI[] Get_T_INFOMATION_CABI(string command)
        {
            List<Table.T_INFOMATION_CABI> items = new List<Table.T_INFOMATION_CABI>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_INFOMATION_CABI>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_INFOMATION_CABI[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_MAGAZINEテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_MAGAZINE[] Get_T_MAGAZINE(string command)
        {
            List<Table.T_MAGAZINE> items = new List<Table.T_MAGAZINE>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_MAGAZINE>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_MAGAZINE[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_MAGAZINEBUYテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_MAGAZINEBUY[] Get_T_MAGAZINEBUY(string command)
        {
            List<Table.T_MAGAZINEBUY> items = new List<Table.T_MAGAZINEBUY>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_MAGAZINEBUY>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_MAGAZINEBUY[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// M_DISTRICTOFFICERテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //public Table.M_DISTRICTOFFICER[] Get_M_DISTRICTOFFICER(string command)
        //{
        //    List<Table.M_DISTRICTOFFICER> items = new List<Table.M_DISTRICTOFFICER>();

        //    try
        //    {
        //        // データ取得
        //        using (SQLiteConnection db = new SQLiteConnection(DbPath))
        //        {   // Select
        //            items = db.Query<Table.M_DISTRICTOFFICER>(command);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return items.Count > 0 ? items.ToArray() : (new Table.M_DISTRICTOFFICER[0]);

        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// M_CABINETテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.M_CABINET[] Get_M_CABINET(string command)
        {
            List<Table.M_CABINET> items = new List<Table.M_CABINET>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.M_CABINET>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.M_CABINET[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// M_CLUBテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.M_CLUB[] Get_M_CLUB(string command)
        {
            List<Table.M_CLUB> items = new List<Table.M_CLUB>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.M_CLUB>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.M_CLUB[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_CLUBSLOGANテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_CLUBSLOGAN[] Get_T_CLUBSLOGAN(string command)
        {
            List<Table.T_CLUBSLOGAN> items = new List<Table.T_CLUBSLOGAN>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_CLUBSLOGAN>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_CLUBSLOGAN[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_MEETINGSCHEDULEテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_MEETINGSCHEDULE[] Get_T_MEETINGSCHEDULE(string command)
        {
            List<Table.T_MEETINGSCHEDULE> items = new List<Table.T_MEETINGSCHEDULE>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_MEETINGSCHEDULE>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_MEETINGSCHEDULE[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_DIRECTORテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_DIRECTOR[] Get_T_DIRECTOR(string command)
        {
            List<Table.T_DIRECTOR> items = new List<Table.T_DIRECTOR>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_DIRECTOR>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_DIRECTOR[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_MEETINGPROGRAMテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_MEETINGPROGRAM[] Get_T_MEETINGPROGRAM(string command)
        {
            List<Table.T_MEETINGPROGRAM> items = new List<Table.T_MEETINGPROGRAM>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_MEETINGPROGRAM>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_MEETINGPROGRAM[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_INFOMATION_CLUBテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_INFOMATION_CLUB[] Get_T_INFOMATION_CLUB(string command)
        {
            List<Table.T_INFOMATION_CLUB> items = new List<Table.T_INFOMATION_CLUB>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.T_INFOMATION_CLUB>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_INFOMATION_CLUB[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// M_MEMBERテーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.M_MEMBER[] Get_M_MEMBER(string command)
        {
            List<Table.M_MEMBER> items = new List<Table.M_MEMBER>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.M_MEMBER>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.M_MEMBER[0]);

        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 複数テーブルからの取得
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// HOME_EVENT(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR)テーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.HOME_EVENT[] Get_HOME_EVENT(string command)
        {
            List<Table.HOME_EVENT> items = new List<Table.HOME_EVENT>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.HOME_EVENT>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.HOME_EVENT[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// MAGAZINE_LIST(T_MAGAZINE/T_MAGAZINEBUY)テーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.MAGAZINE_LIST[] Get_MAGAZINE_LIST(string command)
        {
            List<Table.MAGAZINE_LIST> items = new List<Table.MAGAZINE_LIST>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.MAGAZINE_LIST>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.MAGAZINE_LIST[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// CLUB_MPROG(T_MEETINGPROGRAM/T_MEETINGSCHEDULE)テーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.CLUB_MPROG[] Get_CLUB_MPROG(string command)
        {
            List<Table.CLUB_MPROG> items = new List<Table.CLUB_MPROG>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.CLUB_MPROG>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.CLUB_MPROG[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get_DIRECTOR_LIST(T_DIRECTOR/T_EVENTRET)テーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.DIRECTOR_LIST[] Get_DIRECTOR_LIST(string command)
        {
            List<Table.DIRECTOR_LIST> items = new List<Table.DIRECTOR_LIST>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.DIRECTOR_LIST>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.DIRECTOR_LIST[0]);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get_EVENT_LIST(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR)テーブルデータ取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.EVENT_LIST[] Get_EVENT_LIST(string command)
        {
            List<Table.EVENT_LIST> items = new List<Table.EVENT_LIST>();

            try
            {
                // データ取得
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {   // Select
                    items = db.Query<Table.EVENT_LIST>(command);
                }
            }
            catch
            {
                throw;
            }

            return items.Count > 0 ? items.ToArray() : (new Table.EVENT_LIST[0]);

        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 取得したデータのテーブル展開
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 設定ファイル情報をSQLiteファイルから取得する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void SetSetting()
        {
            Db_A_Setting = null;

            // データ取得
            try
            {
                foreach (Table.A_SETTING row in Get_A_SETTING("SELECT * FROM A_SETTING"))
                {
                    Db_A_Setting = new Table.A_SETTING
                    {
                        DistrictCode = row.DistrictCode,
                        DistrictName = row.DistrictName,
                        CabinetName = row.CabinetName,
                        PeriodStart = row.PeriodStart,
                        PeriodEnd = row.PeriodEnd,
                        DistrictID = row.DistrictID,
                        MagazineMoney = row.MagazineMoney,
                        EventDataDay = row.EventDataDay
                    };

                }
            }
            catch (Exception ex)
            {
                //DisplayAlert("Alert", $"SQLite検索エラー(スローガン) : &{ex.Message}", "OK");
                throw;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報をSQLiteファイルから取得する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void SetAccount()
        {
            Db_A_Account = null;

            // データ取得
            try
            {
                foreach (Table.A_ACCOUNT row in Get_A_ACCOUNT("SELECT * FROM A_ACCOUNT"))
                {
                    Db_A_Account = new Table.A_ACCOUNT
                    {
                        Region = row.Region,
                        Zone = row.Zone,
                        ClubCode = row.ClubCode,
                        ClubName = row.ClubName,
                        MemberCode = row.MemberCode,
                        MemberFirstName = row.MemberFirstName,
                        MemberLastName = row.MemberLastName,
                        AccountDate = row.AccountDate
                    };

                }
                LoginInfo = Db_A_Account.ClubName + "　" + Db_A_Account.MemberFirstName + " " + Db_A_Account.MemberLastName;
            }
            catch (Exception ex)
            {
                ///DisplayAlert("Alert", $"SQLite検索エラー(スローガン) : &{ex.Message}", "OK");
                throw;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ファイルパス情報をSQLiteファイルから取得する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void GetFilePath(string dataClass)
        {
            Db_A_FilePath = null;

            // データ取得
            try
            {
                foreach (Table.A_FILEPATH row in Get_A_FILEPATH("SELECT * FROM A_FILEPATH " +
                                                                "WHERE DataClass = '" + dataClass + "'"))
                {
                    Db_A_FilePath = new Table.A_FILEPATH
                    {
                        DataClass = row.DataClass,
                        FilePath = row.FilePath
                    };

                }
            }
            catch (Exception ex)
            {
                ///DisplayAlert("Alert", $"SQLite検索エラー(スローガン) : &{ex.Message}", "OK");
                throw;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// データ登録
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A_ACCOUNTテーブルデータ登録
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void Set_A_ACCOUNT(Table.A_ACCOUNT account)
        {
            // データ登録
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(DbPath))
                {
                    _ = db.DropTable<Table.A_ACCOUNT>();
                    _ = db.CreateTable<Table.A_ACCOUNT>();
                    _ = db.Insert(new Table.A_ACCOUNT()
                    {
                        Region = account.Region,
                        Zone = account.Zone,
                        ClubCode = account.ClubCode,
                        ClubName = account.ClubName,
                        MemberCode = account.MemberCode,
                        MemberFirstName = account.MemberFirstName,
                        MemberLastName = account.MemberLastName,
                        AccountDate = account.AccountDate
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// T_EVENTRETテーブルデータ更新
        /// </summary>
        /// <param name="command">更新用SQLコマンド</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Table.T_EVENTRET[] Set_T_EVENTRET(string command)
        {

            SQLiteConnection db = new SQLiteConnection(DbPath);
            List<Table.T_EVENTRET> items = db.Query<Table.T_EVENTRET>(command);
            if (items.Count > 0)
            {
                // Update
                db.Update(items);
            }

            return items.Count > 0 ? items.ToArray() : (new Table.T_EVENTRET[0]);

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// ファイル操作
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// SQLiteファイルパス＆ファイル名取得
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string[] GetFileName()
        {
            string[] names = Directory.GetFiles(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
            return names;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ファイルコンテンツ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MultipartFormDataContent GetSendFileContent()
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            content = new MultipartFormDataContent();
            // DBデータ種別（文字列データ）
            content.Add(new StringContent("ACCOUNT"), "DbType");
            // 処理日時（文字列データ）
            content.Add(new StringContent(nowDt.ToString()), "AplTime");
            // Sqliteファイル（バイナリデータ）
            ByteArrayContent sqlite = new ByteArrayContent(File.ReadAllBytes(DbPath));
            content.Add(sqlite, "Sqlite", Path.GetFileName(DbPath));

            return content;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ファイルコンテンツ（HOME）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MultipartFormDataContent GetSendFileContent_HOME()
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            content = new MultipartFormDataContent();
            // DBデータ種別（文字列データ）
            content.Add(new StringContent("HOME"), "DbType");
            // 処理日時（文字列データ）
            content.Add(new StringContent(nowDt.ToString()), "AplTime");
            // Sqliteファイル（バイナリデータ）
            ByteArrayContent sqlite = new ByteArrayContent(File.ReadAllBytes(DbPath));
            content.Add(sqlite, "Sqlite", Path.GetFileName(DbPath));

            return content;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 同期にてSQLiteファイルを送受信する（ファイル保管まで行う）
        /// 受信－ファイル書き出し
        /// </summary>
        /// <param name="sendContent"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public async Task<HttpResponseMessage> AsyncPostFileForWebAPI(MultipartFormDataContent sendContent)
        {
            HttpResponseMessage response = _httpClient.PostAsync(webServiceUrl, sendContent).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var content = response.Content)
                using (var stream = await content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(DbPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.CopyTo(fileStream);
                }
            }
            return response;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 同期にてSQLiteファイルを送受信する（ファイル保管まで行う）
        /// 受信－ファイル書き出し
        /// テスト用画面に情報の表示含む
        /// </summary>
        /// <param name="label"></param>
        /// <param name="sendContent"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public async Task<HttpResponseMessage> AsyncPostFileForWebAPI(Label label, MultipartFormDataContent sendContent)
        {
            //MultipartFormDataContent sendContent = GetSendFileContent();
            label.Text += "PostAsync Start\r\n";
            //HttpResponseMessage response = await _httpClient.PostAsync(webServiceUrl, sendContent);
            HttpResponseMessage response = _httpClient.PostAsync(webServiceUrl, sendContent).Result;
            label.Text += "PostAsync End\r\n";
            label.Text += "HttpStatusCode " + response.StatusCode + "\r\n";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var content = response.Content)
                using (var stream = await content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(DbPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    label.Text += "File output Start\r\n";
                    stream.CopyTo(fileStream);
                    label.Text += "File output End\r\n";
                }
            }
            return response;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力年月日の年度を取得する
        /// </summary>
        /// <param name="dateTime">年度を取得する日時</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetFiscal(string chkDate)
        {
            int chkRet;
            string nowFiscal = string.Empty;
            string dayStr = string.Empty;
            string yearStr = string.Empty;

            // 入力日時の年を取得する。
            yearStr = chkDate.Substring(0, 4);
            // 入力日時の月日を取得する            
            dayStr = chkDate.Substring(5, 5);

            // 戻り値に今年を設定する。
            nowFiscal = yearStr;

            // 年度開始月日と入力月日を比較する。
            chkRet = Db_A_Setting.PeriodStart.CompareTo(dayStr);
            // 年度開始月日より入力月日が前の場合
            if (chkRet > 0)
            {
                // 戻り値に去年を設定する。
                nowFiscal = (int.Parse(yearStr) - 1).ToString();
            }

            return nowFiscal;
        }

    }

}
