using MapNotepad.Models;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Extentions
{
    public static class CustomPinExtension
    {
        public static Pin ConvertToPin(this CustomPin customPin)
        {           
            return new Pin
            {
                Label = customPin.Name,
                Position = new Position(customPin.PositionLat, customPin.PositionLong)
            };
        }
    }
}
