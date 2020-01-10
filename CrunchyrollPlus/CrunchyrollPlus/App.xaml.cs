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
            
            
            MainPage = new NavigationPage(new Loading());
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
