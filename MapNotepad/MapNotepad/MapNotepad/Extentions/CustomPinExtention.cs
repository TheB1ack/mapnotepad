using MapNotepad.Models;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Extentions
{
    public static class CustomPinExtention
    {
        public static Pin ConvertToPin(this CustomPin customPin)
        {           
            Pin pin = new Pin()
            {
                Label = customPin.Name,
                Position = new Position(customPin.PositionLat, customPin.PositionLong)
            };

            return pin;
        }
    }
}
