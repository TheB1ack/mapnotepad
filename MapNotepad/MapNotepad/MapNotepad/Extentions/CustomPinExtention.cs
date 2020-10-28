using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Extentions
{
    public static class CustomPinExtention
    {
        public static Pin ConvertToPin(this CustomPin customPin)
        {
            Pin pin = null;
            pin = new Pin()
            {
                Label = customPin.Name,
                Position = new Position(customPin.PositionLat, customPin.PositionLong)
            };

            return pin;
        }
    }
}
