using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Foundation;
using PCLAppConfig;
using SQLite;
using UIKit;

namespace LionsApl.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            AiForms.Dialogs.Dialogs.Init();
            // Configファイル取得準備
            if (ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            }
            LoadApplication(new App());

            // バックグラウンドフェッチの間隔を設定
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            // PUSH通知許可
            this.PushRegist();

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// DidEnterBackground イベントハンドラ
        /// Backgrond 状態に遷移した状態
        /// </summary>
        /// <param name="uiApplication">アプリケーション</param>
        public override void DidEnterBackground(UIApplication uiApplication)
        {
            System.Diagnostics.Debug.WriteLine("---> DidEnterBackground");

            // SQLLiteの件数
            int badgeCount = GetBadgeCount();
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeCount;

            base.DidEnterBackground(uiApplication);
        }

        /// <summary>
        /// PerformFetch
        /// </summary>
        /// <param name="app"></param>
        /// <param name="completionHandler"></param>
        public override void PerformFetch(UIApplication app, Action<UIBackgroundFetchResult> completionHandler)
        {
            var result = UIBackgroundFetchResult.NoData;
            DateTime nowDt = DateTime.Now;
            UILocalNotification notification = new UILocalNotification();

            try
            {
                // SQLLiteの件数
                int badgeCount = GetBadgeCount();

                if (badgeCount > 0)
                {
                    int intNowDt = nowDt.Hour * 10000 + nowDt.Minute * 100 + nowDt.Second;
                    if (intNowDt >= ((App)Xamarin.Forms.Application.Current).iOSNotFetchFromTime ||
                        intNowDt <= ((App)Xamarin.Forms.Application.Current).iOSNotFetchToTime)
                    {
                        result = UIBackgroundFetchResult.NewData;
                        return;
                    }

                    notification = new UILocalNotification();
                    //notification.FireDate = NSDate.FromTimeIntervalSinceNow(15);
                    notification.FireDate = NSDate.FromTimeIntervalSinceNow(10);
                    //notification.AlertTitle = "Alert Title"; // required for Apple Watch notifications
                    notification.AlertAction = "LionsApl";
                    notification.AlertBody = "未読情報が" + badgeCount.ToString() + "件あります。";
                    notification.SoundName = UILocalNotification.DefaultSoundName;
                    notification.ApplicationIconBadgeNumber = badgeCount;
                    UIApplication.SharedApplication.ScheduleLocalNotification(notification);
                }
                else
                {
                    // 通知バッジ
                    UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                }

                result = UIBackgroundFetchResult.NewData;

            }
            catch
            {
                result = UIBackgroundFetchResult.Failed;
                //System.Diagnostics.Debug.WriteLine(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                completionHandler(result);
            }
        }

        /// <summary>
        /// 通知機能の認証
        /// </summary>
        private void PushRegist()
        {

            System.Diagnostics.Debug.WriteLine("---> PushRegist");

            try
            {

                // 許可をもらう通知タイプの種類を定義
                UIUserNotificationType types = UIUserNotificationType.Badge | // アイコンバッチ
                                               UIUserNotificationType.Sound | // サウンド
                                               UIUserNotificationType.Alert;  // テキスト

                // UIUserNotificationSettingsの生成
                UIUserNotificationSettings nSettings = UIUserNotificationSettings.GetSettingsForTypes(types, null);

                // アプリケーションに登録
                UIApplication.SharedApplication.RegisterUserNotificationSettings(nSettings);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✖PushRegist Error: {ex.Message}");

            }

        }

        /// <summary>
        /// WebServicesApi
        /// </summary>
        public void WebServicesApi()
        {
            // WEBサービス処理
            var rxcui = "198440";
            //var request = HttpWebRequest.Create(string.Format(@"https://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui));
            var request = HttpWebRequest.Create(string.Format(@"http://www.ap.insat.co.jp/LionsAplWeb/LionsAplIisHandler.ashx", rxcui));
            request.ContentType = "application/json";
            request.Method = "GET";

            // WEBサービス結果
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }
                    //Assert.NotNull(content);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報をSQLiteファイルから取得する。(アラート通知)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int GetBadgeCount()
        {
            int badgeCount = 0;
            
            try
            {
                // 未読件数取得
                badgeCount = ((App)Xamarin.Forms.Application.Current).GetBadgeCount();
            }
            catch
            {
                badgeCount = -1;
            }
            return badgeCount;

        }
    }
}
