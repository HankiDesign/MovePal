using HSLMapApp.Views;
using Xamarin.Forms;

namespace HSLMapApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(CalculatePage), typeof(CalculatePage));
        }
    }
}
