using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaView : ContentView
    {
        public MediaView(Media media)
        {
            InitializeComponent();
            episodeScreenshot.Source = media.largeImage;
            episodeName.Text = media.name;
            
        }
    }
}