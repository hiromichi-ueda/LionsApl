
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using PCLAppConfig;
using Android.Content.Res;
using Xamarin.Essentials;
using Android.Content;

namespace LionsApl.Droid
{
    [Activity(Label = "LionsApl", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,ScreenOrientation =ScreenOrientation.Portrait )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Android.Util.Log.Debug(Title, "OnCreate");
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AiForms.Dialogs.Dialogs.Init(this);
            // Configファイル取得準備
            if (ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            }

            LoadApplication(new App());

            // アラーム動作設定
            if (Intent.Data is null) {
                setAlarm();
            }
        }

        /// <summary>
        /// 2021/12/10
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            Android.Util.Log.Debug(Title, "OnRestoreInstanceState");

            base.OnRestoreInstanceState(savedInstanceState);
        }
        protected override void OnUserLeaveHint()
        {
            Android.Util.Log.Debug(Title, "OnUserLeaveHint");

            base.OnUserLeaveHint();
        }
        protected override void OnStart()
        {
            Android.Util.Log.Debug(Title, "OnStart");
            base.OnStart();
        }
        protected override void OnResume()
        {
            Android.Util.Log.Debug(Title, "OnResume");
            base.OnResume();
        }
        protected override void OnPause()
        {
            Android.Util.Log.Debug(Title, "OnPause");

            base.OnPause();
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            Android.Util.Log.Debug(Title, "OnSaveInstanceState");

            base.OnSaveInstanceState(outState);
        }
        protected override void OnStop()
        {
            Android.Util.Log.Debug(Title, "OnStop");

            base.OnStop();
        }
        protected override void OnRestart()
        {
            Android.Util.Log.Debug(Title, "OnRestart");
            base.OnRestart();
        }
        protected override void OnDestroy()
        {
            Android.Util.Log.Debug(Title, "OnDestroy");

            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// フォントサイズを固定化するための処理
        /// （Androidデバイス設定の影響でアプリのフォントサイズが変わらないようにする）
        /// </summary>
        public override Android.Content.Res.Resources Resources
        {
            get 
            {
                if (DeviceInfo.Version.Major < 7 ||
                    (DeviceInfo.Version.Minor == 7 && DeviceInfo.Version.Minor == 0))
                {
                    // Android API 24(Android7.0)以前は古い方式で実装
                    Android.Content.Res.Resources res = base.Resources;
                    Configuration config = new Configuration();
                    config.SetToDefaults();
                    res?.UpdateConfiguration(config, res.DisplayMetrics);
                    return res;
                }
                else
                {
                    // Android API 25(Android7.1)以降は新しい方式で実装
                    var config = new Configuration();
                    config.SetToDefaults();
                    return CreateConfigurationContext(config)?.Resources;
                }
            }
        }

        /// <summary>
        /// アラーム動作設定
        /// </summary>
        private void setAlarm()
        {
            Intent intent = new Intent(this, typeof(AlarmReceiver)); // ReceivedActivityを呼び出すインテントを作成
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent); // ブロードキャストを投げるPendingIntentの作成

            //アラームマネージャーの取得
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            long interval = ((App)Xamarin.Forms.Application.Current).AndroidAlarmInterval * 60 * 1000;
            long trigger = SystemClock.ElapsedRealtime() + interval;
            //アラームマネージャーにセット
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, trigger, interval, pendingIntent);
        }

        /// <summary>
        /// アラーム動作解除
        /// </summary>
        private void cancelAlarm()
        {
            Intent intent = new Intent(this, typeof(AlarmReceiver)); // ReceivedActivityを呼び出すインテントを作成
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent); // ブロードキャストを投げるPendingIntentの作成

            //アラームマネージャーの取得
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            //アラームマネージャーをキャンセル
            alarmManager.Cancel(pendingIntent);
        }

    }
}