using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Services.Map
{
    public interface IMapService
    {
        Task SaveMapPosition(CameraPosition position);
        Task<CameraPosition> GetSavedMapPosition();
    }
}
