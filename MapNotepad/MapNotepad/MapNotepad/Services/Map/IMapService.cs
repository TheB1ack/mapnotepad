using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Services.Map
{
    public interface IMapService
    {
        void SaveMapPosition(CameraPosition position);
        CameraPosition GetSavedMapPosition();
    }
}
