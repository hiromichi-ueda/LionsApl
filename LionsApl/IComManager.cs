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

        private string EventItem1 = "DataNo";
        private string EventItem2 = "Answer";
        private string EventItem3 = "AnswerLate";
        private string EventItem4 = "AnswerEarly";
        private string EventItem5 = "Option1";
        private string EventItem6 = "Option2";
        private string EventItem7 = "Option3";
        private string EventItem8 = "Option4";
        private string EventItem9 = "Option5";
        private string EventItem10 = "OtherCount";

        private static IComManager _single = null;
        private HttpClient _httpClient = null;

        public string DbType1 = "ACCOUNT";
        public string DbType2 = "HOME";
        public string DbType3 = "MAGAZINE";
        public string DbType4 = "EVENTRET";

        public string DbPath { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LionsAplDB.db3");
        public MultipartFormDataContent content;
        public static String webServiceUrl = ((App)Application.Current).WebServiceUrl;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private IComManager()
        {
            _httpClient = new HttpClient();

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
        public static IComManager GetInstance()
        {
            if (_single == null)
            {
                _single = new IComManager();
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
        /// 出欠情報登録情報をコンテンツに設定
        /// </summary>
        /// <param name="eventret">出欠情報</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void GetSendEventContent(CEventReg eventret)
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
            // オプション1（文字列データ）
            content.Add(new StringContent(eventret.Option1), EventItem5);
            // オプション2（文字列データ）
            content.Add(new StringContent(eventret.Option2), EventItem6);
            // オプション3（文字列データ）
            content.Add(new StringContent(eventret.Option3), EventItem7);
            // オプション4（文字列データ）
            content.Add(new StringContent(eventret.Option4), EventItem8);
            // オプション5（文字列データ）
            content.Add(new StringContent(eventret.Option5), EventItem9);
            // オプション1（文字列データ）
            content.Add(new StringContent(eventret.OtherCount.ToString()), EventItem10);

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
                using (var fileStream = new FileStream(DbPath, FileMode.Create, FileAccess.Write, FileShare.None))
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
            ByteArrayContent sqlite = new ByteArrayContent(File.ReadAllBytes(DbPath));
            content.Add(sqlite, "Sqlite", Path.GetFileName(DbPath));

        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// イベント登録クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CEventReg
    {
        public CEventReg(int dataNo,
                         string answer,
                         string answerLate,
                         string anserEarly,
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
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public int OtherCount { get; set; }
    }
}
