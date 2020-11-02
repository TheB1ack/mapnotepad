using Plugin.Settings.Abstractions;

namespace MapNotepad.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private ISettings _appSettings;

        public SettingsService(ISettings appSettings)
        {
            _appSettings = appSettings;
        }

        public int UserId
        {
            get
            {
                return _appSettings.GetValueOrDefault(nameof(UserId), -1);
            }
            set
            {
                _appSettings.AddOrUpdateValue(nameof(UserId), value);
            }
        }
        public double MapLongitude
        {
            get
            {
                return _appSettings.GetValueOrDefault(nameof(MapLongitude), -1d);
            }
            set
            {
                _appSettings.AddOrUpdateValue(nameof(MapLongitude), value);
            }
        }
        public double MapLatitude
        {
            get
            {
                return _appSettings.GetValueOrDefault(nameof(MapLatitude), -1d);
            }
            set
            {
                _appSettings.AddOrUpdateValue(nameof(MapLatitude), value);
            }
        }
        public double MapZoom
        {
            get
            {
                return _appSettings.GetValueOrDefault(nameof(MapZoom), -1d);
            }
            set
            {
                _appSettings.AddOrUpdateValue(nameof(MapZoom), value);
            }
        }
    }
}