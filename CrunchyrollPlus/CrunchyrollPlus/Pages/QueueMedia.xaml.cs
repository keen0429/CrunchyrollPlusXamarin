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
    public partial class QueueMedia : ContentView
    {
        public QueueMedia(Media media)
        {
            
            InitializeComponent();
            episodeName.Text = media.name;
            thumbnail.Source = media.largeImage;
        }
    }
}