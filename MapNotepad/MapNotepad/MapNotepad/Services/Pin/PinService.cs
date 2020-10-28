using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Repository;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pin
{
    public class PinService : IPinService
    {
        private readonly IRepositoryService _repositoryService;

        public PinService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task AddPinAsync(string name, string description, Xamarin.Forms.GoogleMaps.Pin mapPin)
        {
            int userId = CrossSettings.Current.GetValueOrDefault("UserId", -1);
            CustomPin pin = mapPin.ConvertToCustomPin(description, userId);

            await _repositoryService.SaveItemAsync<CustomPin>(pin);
        }

        public void EditPin()
        {

        }

        public void RemovePin()
        {

        }
        public async Task<ObservableCollection<CustomPin>> GetPinsAsync()
        {
            var oldPins = await _repositoryService.GetItemsAsync<CustomPin>();
            var newPins = new ObservableCollection<CustomPin>(oldPins);
            return newPins;
        }
    }
}
