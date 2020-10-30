using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Services.PinsService
{
    public interface IPinService
    {
        Task AddPinAsync(string name, string description, CustomPin mapPin);
        Task<ObservableCollection<CustomPin>> GetPinsByText(string searchText);
        Task UpdatePinAsync(CustomPin pin);
        Task RemovePinAsync(CustomPin item);
        Task<ObservableCollection<CustomPin>> GetPinsAsync();
    }
}
