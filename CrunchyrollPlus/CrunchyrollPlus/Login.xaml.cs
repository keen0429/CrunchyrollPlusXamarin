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
    public partial class Login : ContentPage
    {
        public Login(Loading loading)
        {
            InitializeComponent();
            Navigation.RemovePage(loading);
        }
        
        async void SendLoginRequest(object sender, EventArgs args)
        {
            string username = this.username.Text;
            string password = this.password.Text;

            HttpResponseMessage httpResponseMessage = await App.crunchyClient.PostAsync(App.GetPath("login", $"&account={username}&password={password}"), null);
        }
    }
}
