using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LionsApl
{
    class IComManager
    {
        private string ReqItem1 = "DbType";
        private string ReqItem2 = "AplTime";

        private string MagazineItem1 = "MagazineDataNo";
        private string MagazineItem2 = "Magazine";
        private string MagazineItem3 = "BuyDate";
        private string MagazineItem4 = "BuyNumber";
        private string MagazineItem5 = "MagazinePrice";
        private string MagazineItem6 = "MoneyTotal";
        private string MagazineItem7 = "Region";
        private string MagazineItem8 = "Zone";
        private string MagazineItem9 = "ClubCode";
        private string MagazineItem10 = "ClubNameShort";
        private string MagazineItem11 = "MemberCode";
        private string MagazineItem12 = "MemberName";
        private string MagazineItem13 = "EditUser";
        private string MagazineItem14 = "EditDate";

        private string EventItem1 = "DataNo";
        private string EventItem2 = "Answer";
        private string EventItem3 = "AnswerLate";
        private string EventItem4 = "AnswerEarly";
        private string EventItem5 = "Online";
        private string EventItem6 = "Option1";
        private string EventItem7 = "Option2";
        private string EventItem8 = "Option3";
        private string EventItem9 = "Option4";
        private string EventItem10 = "Option5";
        private string EventItem11 = "OtherCount";

        private static IComManager _single = null;
        private HttpClient _httpClient = null;

        // SQLiteファイルパス
        private string _dbFile;

        public string DbType1 = "ACCOUNT";
        public string DbType2 = "HOME";
        public string DbType3 = "MAGAZINE";
        public string DbType4 = "EVENTRET";

        //public string DbPath { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LionsAplDB.db3");

        // コンテンツ
        public MultipartFormDataContent content;
        public static String webServiceUrl = ((App)Application.Current).WebServiceUrl;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private IComManager(string dbFile)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(600);  // タイムアウト：10分
            _dbFile = dbFile;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// デストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        ~IComManager()
        {
            IComManager.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static IComManager GetInstance(string dbFile)
        {
            if (_single == null)
            {
                _single = new IComManager(dbFile);
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
        /// <summary>
        /// 同期にてテキスト情報を送信する
        /// </summary>
        /// <param name="sendContent">送付コンテンツ情報</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public async Task<HttpResponseMessage> AsyncPostTextForWebAPI()
        {
            HttpResponseMessage response = _httpClient.PostAsync(webServiceUrl, content).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var rescontent = response.Content)
                using (var stream = await rescontent.ReadAsStreamAsync()) { }
            }
            return response;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌購入情報をコンテンツに設定
        /// </summary>
        /// <param name="eventret">出欠情報</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void SetContentToMAGAZINE(CMAGAZINE cmagazine)
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            content = new MultipartFormDataContent();
            // DBデータ種別（文字列データ）
            content.Add(new StringContent(DbType3), ReqItem1);
            // 処理日時（文字列データ）
            content.Add(new StringContent(nowDt.ToString()), ReqItem2);

            // 地区誌データNo.（文字列データ）
            content.Add(new StringContent(cmagazine.MagazineDataNo.ToString()), MagazineItem1);
            // 地区誌名（文字列データ）
            content.Add(new StringContent(cmagazine.Magazine), MagazineItem2);
            // 購入日（文字列データ）
            content.Add(new StringContent(cmagazine.BuyDate), MagazineItem3);
            // 冊子数（文字列データ）
            content.Add(new StringContent(cmagazine.BuyNumber.ToString()), MagazineItem4);
            // 購入価格（税込）（文字列データ）
            content.Add(new StringContent(cmagazine.MagazinePrice.ToString()), MagazineItem5);
            // 購入金額（文字列データ）
            content.Add(new StringContent(cmagazine.MoneyTotal.ToString()), MagazineItem6);
            // リジョン（文字列データ）
            content.Add(new StringContent(cmagazine.Region), MagazineItem7);
            // ゾーン（文字列データ）
            content.Add(new StringContent(cmagazine.Zone), MagazineItem8);
            // クラブコード（文字列データ）
            content.Add(new StringContent(cmagazine.ClubCode), MagazineItem9);
            // クラブ名（略称）（文字列データ）
            content.Add(new StringContent(cmagazine.ClubNameShort), MagazineItem10);
            // 会員番号（文字列データ）
            content.Add(new StringContent(cmagazine.MemberCode), MagazineItem11);
            // 会員名（文字列データ）
            content.Add(new StringContent(cmagazine.MemberName), MagazineItem12);
            // 更新者（文字列データ）
            content.Add(new StringContent(cmagazine.EditUser), MagazineItem13);
            // 更新名（文字列データ）
            content.Add(new StringContent(cmagazine.EditDate), MagazineItem14);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出欠情報をコンテンツに設定
        /// </summary>
        /// <param name="eventret">出欠情報</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void SetContentToEVENTRET(CEVENTRET eventret)
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            content = new MultipartFormDataContent();
            // DBデータ種別（文字列データ）
            content.Add(new StringContent(DbType4), ReqItem1);
            // 処理日時（文字列データ）
            content.Add(new StringContent(nowDt.ToString()), ReqItem2);

            // データNo.（文字列データ）
            content.Add(new StringContent(eventret.DataNo.ToString()), EventItem1);
            // 出欠（文字列データ）
            content.Add(new StringContent(eventret.Answer), EventItem2);
            // 出席（遅刻）（文字列データ）
            content.Add(new StringContent(eventret.AnswerLate), EventItem3);
            // 出席（早退）（文字列データ）
            content.Add(new StringContent(eventret.AnswerEarly), EventItem4);
            // オンライン参加（文字列データ）
            content.Add(new StringContent(eventret.Online), EventItem5);
            // オプション1（文字列データ）
            content.Add(new StringContent(eventret.Option1), EventItem6);
            // オプション2（文字列データ）
            content.Add(new StringContent(eventret.Option2), EventItem7);
            // オプション3（文字列データ）
            content.Add(new StringContent(eventret.Option3), EventItem8);
            // オプション4（文字列データ）
            content.Add(new StringContent(eventret.Option4), EventItem9);
            // オプション5（文字列データ）
            content.Add(new StringContent(eventret.Option5), EventItem10);
            // 本人以外の参加数（文字列データ）
            content.Add(new StringContent(eventret.OtherCount.ToString()), EventItem11);

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 同期にてSQLiteファイルを送受信する（ファイル保管まで行う）
        /// 受信－ファイル書き出し
        /// </summary>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public async Task<HttpResponseMessage> AsyncPostFileForWebAPI()
        {
            HttpResponseMessage response = _httpClient.PostAsync(webServiceUrl, content).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var rescontent = response.Content)
                using (var stream = await rescontent.ReadAsStreamAsync())
                using (var fileStream = new FileStream(_dbFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.CopyTo(fileStream);
                }
            }
            return response;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 送付ファイル情報をコンテンツに設定
        /// </summary>
        /// <param name="type"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void GetSendFileContent(string type)
        {
            // 処理日時取得
            DateTime nowDt = DateTime.Now;

            content = new MultipartFormDataContent();
            // DBデータ種別（文字列データ）
            content.Add(new StringContent(type), ReqItem1);
            // 処理日時（文字列データ）
            content.Add(new StringContent(nowDt.ToString()), ReqItem2);
            // Sqliteファイル（バイナリデータ）
            ByteArrayContent sqlite = new ByteArrayContent(File.ReadAllBytes(_dbFile));
            content.Add(sqlite, "Sqlite", Path.GetFileName(_dbFile));

        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 地区誌購入情報クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CMAGAZINE
    {
        public CMAGAZINE(int magazineDataNo,
                         string magazine,
                         string buyDate,
                         int buyNumber,
                         int magazinePrice,
                         int moneyTotal,
                         string region,
                         string zone,
                         string clubCode,
                         string clubNameShort,
                         string memberCode,
                         string memberName,
                         string editUser,
                         string editDate)
        {
            MagazineDataNo = magazineDataNo;
            Magazine = magazine;
            BuyDate = buyDate;
            BuyNumber = buyNumber;
            MagazinePrice = magazinePrice;
            MoneyTotal = moneyTotal;
            Region = region;
            Zone = zone;
            ClubCode = clubCode;
            ClubNameShort = clubNameShort;
            MemberCode = memberCode;
            MemberName = memberName;
            EditUser = editUser;
            EditDate = editDate;
        }
        public int MagazineDataNo { get; set; }
        public string Magazine { get; set; }
        public string BuyDate { get; set; }
        public int BuyNumber { get; set; }
        public int MagazinePrice { get; set; }
        public int MoneyTotal { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string EditUser { get; set; }
        public string EditDate { get; set; }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 出欠情報クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CEVENTRET
    {
        public CEVENTRET(int dataNo,
                         string answer,
                         string answerLate,
                         string anserEarly,
                         string online,
                         string option1,
                         string option2,
                         string option3,
                         string option4,
                         string option5,
                         int otherCount)
        {
            DataNo = dataNo;
            Answer = answer;
            AnswerLate = answerLate;
            AnswerEarly = anserEarly;
            Online = online;
            Option1 = option1;
            Option2 = option2;
            Option3 = option3;
            Option4 = option4;
            Option5 = option5;
            OtherCount = otherCount;
        }
        public int DataNo { get; set; }
        public string Answer { get; set; }
        public string AnswerLate { get; set; }
        public string AnswerEarly { get; set; }
        public string Online { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public int OtherCount { get; set; }
    }
}
