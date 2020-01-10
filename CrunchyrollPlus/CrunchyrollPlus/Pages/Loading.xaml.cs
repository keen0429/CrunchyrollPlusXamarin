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
            StartSession();
        }
        public async void StartSession()
        {
            CrunchyrollApi.SessionResponse sessionResponse = await crunchyrollApi.StartSession();
            if(sessionResponse.success)
            {
                if (sessionResponse.authed)
                {
                    await Navigation.PushAsync(new Homepage());
                }
                else
                {
                    await Navigation.PushAsync(new Login(this));
                }
            }
            else
            {
                //TODO: Error handeling
            }
            
        }
    }
    
}