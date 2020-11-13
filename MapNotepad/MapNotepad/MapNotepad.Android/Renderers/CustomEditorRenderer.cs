﻿using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Widget;
using MapNotepad.Controls;
using MapNotepad.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MapNotepad.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
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
                    var shape = new ShapeDrawable(new RoundRectShape(cornersOut, new RectF(10,10,10,10), null));
                    shape.Paint.Color = Xamarin.Forms.Color.FromHex("#29D695").ToAndroid();
                    shape.Paint.SetStyle(Paint.Style.FillAndStroke);
                    
                    nativeEditText.Background = shape;
                }
            }
        }
    }
}