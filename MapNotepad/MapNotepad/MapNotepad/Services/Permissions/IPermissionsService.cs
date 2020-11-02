using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace MapNotepad.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionsAsync<T>() where T : BasePermission, new();
        Task<PermissionStatus> RequestPermissionsAsync<T>() where T : BasePermission, new();
        Task<bool> ShowRequestPermissionRationaleAsync(Permission permission);
    }
}
