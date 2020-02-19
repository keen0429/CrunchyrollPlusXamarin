using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : ContentPage
    {
        CrunchyrollApi crunchyrollApi = CrunchyrollApi.GetSingleton();
        public Loading()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            StartSession();
        }
        public async void StartSession(bool uS = true)
        {
            CrunchyrollApi.SessionResponse sessionResponse;
            bool usedUs = false;
            if (Application.Current.Properties.ContainsKey("useUsSession") && (bool)Application.Current.Properties["useUsSession"]&&uS)
            {
                sessionResponse = await crunchyrollApi.StartSession(true);
                usedUs = true;
            }

            else
            {
                sessionResponse = await crunchyrollApi.StartSession(false);
            }
            if(sessionResponse.success)
            {
                
                if (sessionResponse.authed)
                {
                    User.signedIn = true;
                    await Navigation.PushAsync(new Homepage());
                }
                else
                {
                    await Navigation.PushAsync(new Login(this));
                }
            }
            else
            {
                if (usedUs)
                {
                    DependencyService.Get<IToastService>().ShowToastShort("Couldn't get US session, trying default");
                    StartSession(false);
                }
                else
                {
                    DependencyService.Get<IToastService>().ShowToastShort("Couldn't start session, please try again later");
                }
                //TODO: Error handeling
            }
            
        }
    }
    
}