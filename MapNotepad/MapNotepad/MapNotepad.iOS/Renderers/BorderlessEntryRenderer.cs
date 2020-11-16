using MapNotepad.Controls;
using MapNotepad.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace MapNotepad.iOS.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {

        #region -- EntryRenderer implementation --

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Control is null");
            }

        }

        #endregion

    }
}