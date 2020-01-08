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
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            crunchyClient = new HttpClient();
            crunchyClient.BaseAddress = new Uri("https://api.crunchyroll.com");
            
            MainPage = new NavigationPage(new Loading());
        }
        public static string GetPath(string req, string data)
        {
            return $"/{req}.0.json?session_id={sessionId}{data}";
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
