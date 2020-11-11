using Xamarin.Forms;

namespace MapNotepad.Views
{
    public partial class HomeTabbedPage : TabbedPage
    {
        public HomeTabbedPage()
        {
            InitializeComponent();    
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //this.Parent = new NavigationPage();
        }
    }
}