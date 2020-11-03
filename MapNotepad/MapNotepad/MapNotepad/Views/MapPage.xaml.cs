using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Internals;

namespace MapNotepad.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            this.frame.TranslateTo(0,150);
        }

        #region -- Private helpers --

        private void OnPinClicked(object sender, PinClickedEventArgs args)
        {
            this.frame.TranslateTo(0, 0, 600);
        }
        private void OnMapClicked(object sender, MapClickedEventArgs args)
        {
            this.frame.TranslateTo(0, 150, 600);
        }

        #endregion

    }
}
