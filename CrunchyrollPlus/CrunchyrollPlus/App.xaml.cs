using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CrunchyrollPlus
{
    public partial class App : Application
    {
        public static HttpClient crunchyClient;
        public static string sessionId;
        public App()
        {

            InitializeComponent();
            crunchyClient = new HttpClient();
            crunchyClient.BaseAddress = new Uri("https://api.crunchyroll.com");
            StartSession();
            MainPage = new MainPage();
        }
        public static string GetPath(string req, string data)
        {
            return $"/{req}.0.json?session_id={sessionId}{data}";
        }
        
        public async void StartSession()
        {
            const string deviceType = "com.crunchyroll.windows.desktop";

            string url = $"/start_session.0.json?access_token=LNDJgOit5yaRIWN&device_id={Guid.NewGuid()}&device_type={deviceType}";

            HttpResponseMessage httpResponseMessage = await crunchyClient.PostAsync(url, null);
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
                    sessionId = (string)data["session_id"];
                    Console.WriteLine("LOG: done");
                }


            }
        }
        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
