using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using PCLAppConfig;
using Xamarin.Forms;
using Android.Content.Res;
using Xamarin.Essentials;

namespace LionsApl.Droid
{
    [Activity(Label = "LionsApl", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
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
       
    }
}