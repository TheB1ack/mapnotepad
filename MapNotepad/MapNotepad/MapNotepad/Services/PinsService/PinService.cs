using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Repository;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Services.PinsService
{
    public class PinService : IPinService
    {
        private readonly IRepositoryService _repositoryService;

        public PinService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task AddPinAsync(string name, string description, CustomPin mapPin)
        {
            int userId = CrossSettings.Current.GetValueOrDefault("UserId", -1);
            mapPin.Description = description ?? "";
            mapPin.Name = name;
            mapPin.UserId = userId;

            await _repositoryService.SaveItemAsync<CustomPin>(mapPin);
        }

        public async Task<ObservableCollection<CustomPin>> GetPinsByText(string searchText) 
        {
            var items = await _repositoryService.GetItemsAsync<CustomPin>();
            var searchedItems = items.Where(x => (x.Name.ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLong.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.Description.ToString().ToUpper().Contains(searchText.ToUpper()))
                                    || (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper()))).ToList();

            return new ObservableCollection<CustomPin>(searchedItems);
            
        }

        public async Task RemovePinAsync(CustomPin item)
        {
            await _repositoryService.DeleteItemAsync(item);
        }
        public async Task<ObservableCollection<CustomPin>> GetPinsAsync()
        {
            var oldPins = await _repositoryService.GetItemsAsync<CustomPin>();
            var newPins = new ObservableCollection<CustomPin>(oldPins);
            return newPins;
        }
    }
}
