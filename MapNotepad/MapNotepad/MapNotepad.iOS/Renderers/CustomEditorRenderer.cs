using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MapNotepad.Controls;
using MapNotepad.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MapNotepad.iOS.Renderers
{
    class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
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