using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Extentions
{
    public static class PinExtention
    {
        public static CustomPin ConvertToCustomPin(this Pin pin, string description, int userId)
        {
            CustomPin customPin = new CustomPin()
                {
                    Name = pin.Label,
                    Description = description,
                    PositionLong = pin.Position.Longitude,
                    PositionLat = pin.Position.Latitude,
                    UserId = userId
                };
            
            return customPin;
        }
    }
}
