using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pin
{
    public interface IPinService
    {
         Task AddPinAsync(string name, string description, Xamarin.Forms.GoogleMaps.Pin mapPin);
         void EditPin();
         void RemovePin();
        Task<ObservableCollection<CustomPin>> GetPinsAsync();
    }
}
