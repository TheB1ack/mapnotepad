using MapNotepad.Controls;
using MapNotepad.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MapNotepad.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {

        #region -- EntryRenderer implementation --

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
            else
            {
                System.Diagnostics.Debug.WriteLine("Control is null");
            }

        }

        #endregion

    }
}