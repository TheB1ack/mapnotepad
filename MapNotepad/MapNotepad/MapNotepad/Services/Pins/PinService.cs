﻿using MapNotepad.Enums;
using MapNotepad.Models;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pins
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

        #region -- IterfaceName implementation --

        public Task AddPinAsync(CustomPin pin)
        {
            int userId = _settingsService.UserId; 
            pin.UserId = userId;

            return _repositoryService.SaveItemAsync(pin);
        }

        public async Task<IEnumerable<CustomPin>> GetPinsByTextAsync(string searchText, SearchCategories category)
        {

            var items = await GetPinsAsync();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                items = items.Where(x => (x.Name.ToUpper().Contains(searchText.ToUpper())) ||
                                         (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper())) ||
                                         (x.PositionLong.ToString().ToUpper().Contains(searchText.ToUpper())) ||
                                         (x.Description.ToString().ToUpper().Contains(searchText.ToUpper())) ||
                                         (x.PositionLat.ToString().ToUpper().Contains(searchText.ToUpper())));
            }  

            if(category != 0)
            {
                items = items.Where(x => (x.Category == (int)category));
            }

            return items;
        }

        public Task UpdatePinAsync(CustomPin pin)
        {
            return _repositoryService.UpdateItemAsync<CustomPin>(pin);
        }

        public Task RemovePinAsync(CustomPin pin)
        {
            return _repositoryService.DeleteItemAsync(pin);
        }

        public Task<IEnumerable<CustomPin>> GetPinsAsync()
        {
            int userId = _settingsService.UserId;
            return  _repositoryService.GetItemsAsync<CustomPin>(x => x.UserId == userId);
        }

        public async Task<bool> CheckPinName(string name)
        {
            bool isValid = true;

            var items = await GetPinsAsync();
            var searchedPin = items.FirstOrDefault(x => x.Name == name);

            if (searchedPin != null)
            {
                isValid = false;
            }

            return isValid;
        }

        #endregion
    }
}