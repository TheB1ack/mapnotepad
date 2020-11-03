using Android.Graphics.Drawables;
using MapNotepad.Droid.Effects;
using MapNotepad.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MyCompany")]
[assembly: ExportEffect(typeof(BackgroundAndriodEffect), nameof(BackgroundEffect))]
namespace MapNotepad.Droid.Effects
{
    public class BackgroundAndriodEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Rgb(41, 214, 149));
            }
        }

        protected override void OnDetached()
        {

        }
    }
}