using MapNotepad.Models;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using Plugin.Settings;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.PinsService
{
    public class PinService : IPinService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ISettingsService _settingsService;
        public PinService(IRepositoryService repositoryService, ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
        }
        public async Task AddPinAsync(string pinName, string pinDescription, CustomPin mapPin)
        {
            int userId = _settingsService.UserId;

            mapPin.Description = pinDescription ?? "";
            mapPin.Name = pinName;
            mapPin.UserId = userId;

            await _repositoryService.SaveItemAsync(mapPin);
        }

        public async Task<ObservableCollection<CustomPin>> GetPinsByTextAsync(string searchText) 
        {
            var items = await GetPinsAsync();
            var searchedItems = items.Where(x => (x.Name.ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLong.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.Description.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper()))).ToList();

            return new ObservableCollection<CustomPin>(searchedItems);   
        }
        public async Task UpdatePinAsync(CustomPin pin)
        {
            await _repositoryService.UpdateItemAsync<CustomPin>(pin);
        }
        public async Task RemovePinAsync(CustomPin pin)
        {
            await _repositoryService.DeleteItemAsync(pin);
        }
        public async Task<ObservableCollection<CustomPin>> GetPinsAsync()
        {
            int userId = _settingsService.UserId;
            var repositoryItems = await _repositoryService.GetItemsAsync<CustomPin>();
            var newPins = new ObservableCollection<CustomPin>(repositoryItems.Where(x => x.UserId == userId));

            return newPins;
        }
    }
}