using Android.App;
using Android.Content;
using Android.Support.V4.App;
using System;

namespace LionsApl.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted,
                        "android.intent.action.QUICKBOOT_POWERON",
                        "com.htc.intent.action.QUICKBOOT_POWERON",
                        "android.intent.action.PACKAGE_INSTALL",
                        "android.intent.action.PACKAGE_ADDED",
                        Intent.ActionMyPackageReplaced })]
    public class AlarmReceiver : BroadcastReceiver
    {
        [Obsolete]
        public override void OnReceive(Context context, Intent intent)
        {
            // SQLLiteの件数
            int badgeCount = GetBadgeCount();
            if (badgeCount <= 0)
            {
                // 未読情報が0件または取得時にエラーが発生した場合は通知をしない
                return;
            }

            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            // アクティビティを起動するインテント
            // Flagsなどは用途に応じて適宜書き換える。
            var trigger = context.PackageManager
                .GetLaunchIntentForPackage(context.PackageName)
                .AddFlags(ActivityFlags.SingleTop);

            // PendingIntentを噛ますことで、インテントを直ちに発行するのではなく、
            // 通知欄をタップされたタイミングで発行する
            var pendingIntent = PendingIntent.GetActivity(context, 0, trigger, PendingIntentFlags.UpdateCurrent);

            // 通知を生成
            var message = "未読情報が" + badgeCount.ToString() + "件あります。";
            var builder = new NotificationCompat.Builder(context)
                .SetVibrate(new long[] { 0, 200, 100, 200, 100, 200 })              // バイブレートパターン
                .SetSmallIcon(Android.Resource.Drawable.StarBigOn)                  // アイコン
                .SetAutoCancel(true)                                                // タップ後の通知削除
                .SetContentTitle("LionsApl")                                        // 通知タイトル
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(message))   // 通知スタイル
                .SetContentText(message)                                            // 通知テキスト
                .SetPriority((int)NotificationPriority.High)                        // 優先度
                .SetContentIntent(pendingIntent);                                   // 通知がクリックされた時の送信

            // 通知を発射
            manager.Notify(1, builder.Build());
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