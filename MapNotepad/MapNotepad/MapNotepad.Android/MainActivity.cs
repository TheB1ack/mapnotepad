using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Prism;
using Prism.Ioc;
using Acr.UserDialogs;
using System.Linq;
using Xamarin.Forms.Platform.Android;

namespace MapNotepad.Droid
{
    [Activity(Label = "Map Notepad", Icon = "@mipmap/MainIcon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait,  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : FormsAppCompatActivity
    {

        #region -- FormsAppCompatActivity implementation --

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (permissions.Contains("android.permission.ACCESS_COARSE_LOCATION") || permissions.Contains("android.permission.ACCESS_FINE_LOCATION"))
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permissions array doesn't contains ACCESS_COARSE_LOCATION and ACCESS_FINE_LOCATION");
            }

        }

        #endregion

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {

            }
        }
    }
}