using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using JikanDotNet;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Homepage : TabbedPage
    {
        public Homepage()
        {
            if (User.signedIn)
            {
                Children.Add(new Queue());
                // Add recently watched
            }
            InitializeComponent();



            // DependencyService.Get<IDeviceOrientationService>().ForceLandscape();
            // InitQueue();
            
        }

        
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
       
        
    }
}