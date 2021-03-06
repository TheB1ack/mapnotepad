﻿using MapNotepad.Models;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ISettingsService _settingsService;

        public AuthorizationService(IRepositoryService repositoryService, ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
        }

        #region -- Public properties --

        public bool IsAuthorized
        {
            get => _settingsService.UserId != -1;
        }

        #endregion

        #region -- IterfaceName implementation --

        public async Task<bool> SignUpAsync(string userName, string userEmail, string userPassword)
        {           
            var items = await _repositoryService.GetItemsAsync<User>();
            User userResult = items.FirstOrDefault(x => x.Email == userEmail);

            var isSignedUp = true;

            if (userResult != null)
            {
                isSignedUp = false;
            }
            else
            {
                User user = new User()
                {
                    Name = userName,
                    Email = userEmail,
                    Password = userPassword,
                };

                await _repositoryService.SaveItemAsync(user);
            }

            return isSignedUp;
        }
        public async Task<bool> SignInAsync(string userEmail, string userPassword)
        {
            var isSignedIn = false;

            var items = await _repositoryService.GetItemsAsync<User>();
            User userResult = items.FirstOrDefault(x => x.Email.ToUpper().Equals(userEmail.ToUpper()));

            if (userResult?.Password == userPassword)
            {
                _settingsService.UserId = userResult.Id;
                isSignedIn = true;
            }
            else
            {
                Debug.WriteLine("userResult war null or password doesn't match");
            }

            return isSignedIn;
        }
        public void LogOut()
        {
            _settingsService.UserId = -1;
        }

        #endregion

    }
}

