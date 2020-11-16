using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using MapNotepad.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables.Shapes;
using MapNotepad.Controls;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MapNotepad.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) 
                                   : base(context) 
        {
            
        }

        #region -- EntryRenderer implementation --

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var cornersOut = new float[]
                {
                    10, 10,
                    10, 10,
                    10, 10,
                    10, 10
                };

                if (Control is EditText nativeEditText)
                {
                    var shape = new ShapeDrawable(new RoundRectShape(cornersOut, new RectF(10, 10, 10, 10), null));
                    shape.Paint.Color = Xamarin.Forms.Color.FromHex("#29D695").ToAndroid();
                    shape.Paint.SetStyle(Paint.Style.FillAndStroke);

                    nativeEditText.Background = shape;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Control isn't EditText");
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Control is null");
            }

        }

        #endregion

    }
}