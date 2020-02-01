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
            usSessionSwitch.IsToggled = (bool)Application.Current.Properties["usSession"];

        }

        private void UsSessionChanged(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["usSession"] = usSessionSwitch.IsToggled;

        }
    }
}