using MapNotepad.Services.Map;
using MapNotepad.Services.Settings;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Services.MapService
{
    public class MapService : IMapService
    {
        private readonly ISettingsService _settingsService;
        public MapService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public void SaveMapPosition(CameraPosition position)
        {
            _settingsService.MapLatitude = position.Target.Latitude;
            _settingsService.MapLongitude = position.Target.Longitude;
            _settingsService.MapZoom = position.Zoom;

        }
        public CameraPosition GetSavedMapPosition()
        {
            CameraPosition newCameraPosition = null;

            if (_settingsService.MapLatitude != -1d)
            {
                var position = new Position(_settingsService.MapLatitude, _settingsService.MapLongitude);
                newCameraPosition = new CameraPosition(position, _settingsService.MapZoom);
            }

            return newCameraPosition;
        }
    }
}
