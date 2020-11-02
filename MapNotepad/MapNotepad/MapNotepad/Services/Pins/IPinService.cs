using MapNotepad.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pins
{
    public interface IPinService
    {
        Task AddPinAsync(CustomPin pin);
        Task<IEnumerable<CustomPin>> GetPinsByTextAsync(string searchText);
        Task UpdatePinAsync(CustomPin pin);
        Task RemovePinAsync(CustomPin pin);
        Task<IEnumerable<CustomPin>> GetPinsAsync();
        Task<bool> CheckPinName(string name);
    }
}
