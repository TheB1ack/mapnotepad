using Xamarin.Essentials;
using System.Threading.Tasks;


namespace MapNotepad.Services.Permission
{
    public class PermissionsService : IPermissionsService
    {
           
        #region -- IPermissionsService implementation --

        public Task<PermissionStatus> CheckPermissionsAsync<T>() where T: Permissions.BasePermission, new()
        {
            return Permissions.CheckStatusAsync<T>();
        }

        public Task<PermissionStatus> RequestPermissionsAsync<T>() where T : Permissions.BasePermission, new()
        {
            return Permissions.RequestAsync<T>();
        }

        #endregion

    }
}
