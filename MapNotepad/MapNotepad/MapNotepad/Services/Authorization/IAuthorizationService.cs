using System.Threading.Tasks;

namespace MapNotepad.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        Task<bool> SignUpAsync(string userName, string userEmail, string userPassword);
        Task<bool> SignInAsync(string userEmail, string userPassword);
        void LogOut();
    }
}
