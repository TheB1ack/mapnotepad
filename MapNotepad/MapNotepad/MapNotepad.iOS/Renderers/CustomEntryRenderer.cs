using CoreGraphics;
using MapNotepad.iOS.Renderers;
using MapNotepad.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MapNotepad.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundColor = UIColor.White;
                Control.Layer.CornerRadius = 3;
                Control.Layer.BorderColor = UIColor.FromRGB(41, 214, 149).CGColor;
                Control.Layer.BorderWidth = 3;
            }

        }
    }
}