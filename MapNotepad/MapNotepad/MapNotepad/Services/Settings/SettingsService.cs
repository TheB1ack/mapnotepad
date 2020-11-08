using Plugin.Settings.Abstractions;

namespace MapNotepad.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettings _appSettings;

        public SettingsService(ISettings appSettings)
        {
            _appSettings = appSettings;
        }

        #region -- Public properties --

        public int UserId 
        {
            get => _appSettings.GetValueOrDefault(nameof(UserId), -1);

            set => _appSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        public double MapLongitude
        {
            get => _appSettings.GetValueOrDefault(nameof(MapLongitude), -1d);

            set => _appSettings.AddOrUpdateValue(nameof(MapLongitude), value);
        }

        public double MapLatitude
        {
            get => _appSettings.GetValueOrDefault(nameof(MapLatitude), -1d);

            set => _appSettings.AddOrUpdateValue(nameof(MapLatitude), value);
        }

        public double MapZoom
        {
            get => _appSettings.GetValueOrDefault(nameof(MapZoom), -1d);

            set => _appSettings.AddOrUpdateValue(nameof(MapZoom), value);
        }

        #endregion

        #region -- IterfaceName implementation --

        public void ClearData()
        {
            UserId = -1;
            MapLatitude = -1d;
            MapLongitude = -1d;
            MapZoom = -1d;
        }

        #endregion
    }
}