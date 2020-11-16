using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MapNotepad.iOS
{

    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {

        #region -- FormsApplicationDelegate implementation --

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init("AIzaSyAdxufr7FaU3UZJSpBbJABw30XPjer6NU4");

            LoadApplication(new App(new iOSInitializer()));

            Plugin.InputKit.Platforms.iOS.Config.Init();

            return base.FinishedLaunching(app, options);
        }

        #endregion

        public class iOSInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {

            }

        }

    }
}
