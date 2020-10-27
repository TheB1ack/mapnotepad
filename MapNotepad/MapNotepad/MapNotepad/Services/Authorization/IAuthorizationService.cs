using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> SingUpAsync(string name, string email, string password);
        Task<bool> SingInAsync(string email, string password);
        void LogOut();
    }
}
