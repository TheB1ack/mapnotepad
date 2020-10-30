using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using MapNotepad.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables.Shapes;
using MapNotepad.Renderer;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MapNotepad.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var corners = new float[]{ 75, 75,
                                           75, 75,
                                           75, 75,
                                           75, 75};

                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
                var nativeEditText = (EditText)Control;
                var shape = new ShapeDrawable(new RoundRectShape(corners,null,null));
                shape.Paint.Color = Xamarin.Forms.Color.Black.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);

                nativeEditText.Background = shape;
            }
        }
    }
}