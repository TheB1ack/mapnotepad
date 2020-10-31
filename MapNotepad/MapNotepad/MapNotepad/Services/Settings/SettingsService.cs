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
    }
}