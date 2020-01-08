using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

namespace CrunchyrollPlus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void Login(object sender, EventArgs args)
        {
            Console.WriteLine("login");
        }
        async void SendLoginRequest()
        {
            string username = this.username.Text;
            string password = this.password.Text;

            HttpResponseMessage httpResponseMessage = await App.crunchyClient.PostAsync(App.GetPath("login", $"&account={username}&password={password}"), null);
        }
    }
}
