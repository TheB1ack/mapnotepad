

using MapNotepad.Effects;
using MapNotepad.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("MyCompany")]
[assembly: ExportEffect(typeof(BackgroundiOSEffect), nameof(BackgroundEffect))]
namespace MapNotepad.iOS.Effects
{
    public class BackgroundiOSEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (this.Control != null)
            {
                Control.BackgroundColor = UIColor.FromRGB(41, 214, 149);
            }
        }

        protected override void OnDetached()
        {

        }
    }
}