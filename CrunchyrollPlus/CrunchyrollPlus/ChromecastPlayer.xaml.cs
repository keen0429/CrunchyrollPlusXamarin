using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SharpCaster;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChromecastPlayer : ContentPage
    {
        ChromecastWrapper wrapper = ChromecastWrapper.GetSingleton();
        public ChromecastPlayer()
        {
            InitializeComponent();
            DependencyService.Get<IFullscreenService>().ExitFullscreen();
            DependencyService.Get<IDeviceOrientationService>().ForcePortrait();
            PlayPause(wrapper,new EventArgs());
        }
        async void PlayPause(object sender, EventArgs e)
        {
            var temp = wrapper.ChromecastService.ChromeCastClient.ChromecastStatus.Applications;
        }
        async void Exit(object sender, EventArgs e)
        {

        }
        async void CastMenu (object sender, EventArgs e)
        {

        }
        async void Forward(object sender, EventArgs e)
        {

        }
        async void Rewind(object sender, EventArgs e)
        {

        }
        async void Skip(object sender, EventArgs e)
        {

        }
    }
}