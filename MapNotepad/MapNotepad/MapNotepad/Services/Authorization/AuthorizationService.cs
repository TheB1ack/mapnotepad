using MapNotepad.Models;
using MapNotepad.Services.Repository;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryService _repository;
        public AuthorizationService(IRepositoryService repository)
        {
            _repository = repository;
        }
        public async Task<bool> SingUpAsync(string name, string email, string password)
        {
            bool flag = true;
            var items = await _repository.GetItemsAsync<User>();
            User userResult = items.Where(x => x.Email == email).FirstOrDefault();
            if (userResult != null)
            {
                flag =  false;
            }
            else
            {
                User user = new User()
                {
                    Name = name,
                    Email = email,
                    Password = password,
                };
                await _repository.SaveItemAsync(user);
            }

            return flag;
        }
        public async Task<bool> SingInAsync(string email, string password)
        {
            bool flag = false;
            var items = await _repository.GetItemsAsync<User>();
            User userResult = items.Where(x => x.Email.Equals(email)).FirstOrDefault();

            if (userResult?.Password == password)
            {
                CrossSettings.Current.AddOrUpdateValue("UserId", userResult.Id);
                flag =  true;
            }

            return flag;
        }
        public void LogOut()
        {
            CrossSettings.Current.Remove("UserId");
        }
    }
}

