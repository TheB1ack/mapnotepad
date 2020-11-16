using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MapNotepad.Services.Permission
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionsAsync<T>() where T : Permissions.BasePermission, new();

        public Task<PermissionStatus> RequestPermissionsAsync<T>() where T : Permissions.BasePermission, new();

    }
}
