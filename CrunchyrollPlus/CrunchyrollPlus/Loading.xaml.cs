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
        public Loading()
        {
            InitializeComponent();
            StartSession();
        }
        public async void StartSession()
        {
            const string deviceType = "com.crunchyroll.windows.desktop";

            string url = $"/start_session.0.json?access_token=LNDJgOit5yaRIWN&device_id={Guid.NewGuid()}&device_type={deviceType}";

            HttpResponseMessage httpResponseMessage = await App.crunchyClient.PostAsync(url, null);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(result);
                if ((bool)jObject["error"])
                {
                    Console.WriteLine("LOG: ERROR");
                }
                else
                {
                    JObject data = (JObject)jObject["data"];
                    App.sessionId = (string)data["session_id"];
                    await Navigation.PushAsync(new Login(this));
                }


            }
        }
    }
    
}