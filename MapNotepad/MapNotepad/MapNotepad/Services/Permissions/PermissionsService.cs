using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;


namespace MapNotepad.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissions _permissions;

        public PermissionsService(IPermissions permissions)
        {
            _permissions = permissions;
        }

        #region -- IterfaceName implementation --

        public Task<PermissionStatus> CheckPermissionsAsync<T>() where T: BasePermission, new()
        {
            return _permissions.CheckPermissionStatusAsync<T>();
        }
        public Task<PermissionStatus> RequestPermissionsAsync<T>() where T : BasePermission, new()
        {
            return _permissions.RequestPermissionAsync<T>();
        }
        public Task<bool> ShowRequestPermissionRationaleAsync(Permission permission) 
        {
            return _permissions.ShouldShowRequestPermissionRationaleAsync(permission);
        }

        #endregion

    }
}
