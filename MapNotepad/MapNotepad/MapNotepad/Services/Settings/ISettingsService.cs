using Plugin.Settings.Abstractions;

namespace MapNotepad.Services.Settings
{
    public interface ISettingsService
    {
        int UserId { get; set; }
        double MapLongitude { get; set; }
        double MapLatitude { get; set; }
        double MapZoom { get; set; }
        void ClearData();
    }
}
