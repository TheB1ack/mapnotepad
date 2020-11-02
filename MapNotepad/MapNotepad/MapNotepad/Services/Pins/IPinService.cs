using MapNotepad.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pins
{
    public interface IPinService
    {
        Task AddPinAsync(string pinName, string pinDescription, CustomPin mapPin);
        Task<ObservableCollection<CustomPin>> GetPinsByTextAsync(string searchText);
        Task UpdatePinAsync(CustomPin pin);
        Task RemovePinAsync(CustomPin pin);
        Task<ObservableCollection<CustomPin>> GetPinsAsync();
    }
}
