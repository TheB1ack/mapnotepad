using MapNotepad.Control;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Behaviors
{
    class CustomMapBehavior: Behavior<CustomMap>
    {
        protected override void OnAttachedTo(CustomMap map)
        {
            map.CameraChanged += OnCameraChanged;
            base.OnAttachedTo(map);
        }

        protected override void OnDetachingFrom(CustomMap map)
        {
            map.CameraChanged -= OnCameraChanged;
            base.OnDetachingFrom(map);
        }

        void OnCameraChanged(object sender, CameraChangedEventArgs args)
        {
            var position = args.Position;
            //((Map)sender)
            //((Map)sender).MoveToRegion(MapSpan.FromCenterAndRadius);
        }
    }
}
