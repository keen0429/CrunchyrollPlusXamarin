using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            usSessionSwitch.IsToggled = Application.Current.Properties.ContainsKey("useUsSession")&&(bool)Application.Current.Properties["useUsSession"];

        }

        private async void UsSessionChanged(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["useUsSession"] = usSessionSwitch.IsToggled;
            await Application.Current.SavePropertiesAsync();

        }

        private async void LogOut(object sender, EventArgs e)
        {
            Application.Current.Properties["auth"] = null;
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopToRootAsync();
        }

    }
}