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

        public bool IsAuthorized
        {
            get
            {
                return _settingsService.UserId != -1;
            }
        }

        public AuthorizationService(IRepositoryService repositoryService, ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
        }

        public async Task<bool> SignUpAsync(string userName, string userEmail, string userPassword)
        {           
            var items = await _repositoryService.GetItemsAsync<User>();
            User userResult = items.Where(x => x.Email == userEmail).FirstOrDefault();
            bool isSignedUp = true;

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
            bool isSignedIn = false;
            var items = await _repositoryService.GetItemsAsync<User>();
            User userResult = items.Where(x => x.Email.ToUpper().Equals(userEmail.ToUpper())).FirstOrDefault();

            if (userResult?.Password == userPassword)
            {
                _settingsService.UserId = userResult.Id;
                isSignedIn = true;
            }

            return isSignedIn;
        }
        public void LogOut()
        {
            _settingsService.UserId = -1;
        }
    }
}

