using MapNotepad.Models;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ISettingsService _settingsService;

        public AuthorizationService(IRepositoryService repositoryService, 
                                    ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
        }

        #region -- IAuthorizationService implementation --

        public bool IsAuthorized
        {
            get => _settingsService.UserId != -1;
        }

        public async Task<bool> SignUpAsync(string userName, string userEmail, string userPassword)
        {
            var isSignedUp = true;
            var userResult = await GetUserByMail(userEmail);

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
            var userResult = await GetUserByMail(userEmail);

            if (userResult?.Password == userPassword)
            {
                _settingsService.UserId = userResult.Id;
                isSignedIn = true;
            }
            else
            {
                isSignedIn = false;
            }

            return isSignedIn;
        }

        public async Task<User> GetUserByMail(string userEmail)
        {
            var items = await _repositoryService.GetItemsAsync<User>(x => x.Email.ToUpper().Equals(userEmail.ToUpper()));
            return items.FirstOrDefault();
        }

        public void LogOut()
        {
            _settingsService.ClearData();
        }

        #endregion

    }
}

