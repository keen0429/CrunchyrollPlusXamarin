using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaView : ContentView
    {
        Media media;
        CrunchyrollApi crunchy = CrunchyrollApi.GetSingleton();
        Series series;
        public MediaView(Media media)
        {
            
            InitializeComponent();
            
            episodeScreenshot.Source = media.largeImage;
            episodeName.Text = media.name;
            episodeCount.Text ="Episode "+ media.episodeNumber;
            this.media = media;
            Init();
            
        }
        private async void Init()
        {
            CrunchyrollApi.GetSeriesResponse res = await crunchy.GetSeries(media.seriesId);
            if (res.success)
            {
                Debug.WriteLine("LOG: SUCCESS SHOW");
                series = res.series;
            }
            else
            {
                Debug.WriteLine("LOG: FAILED SHOW");
                Debug.WriteLine("LOG: FAIL: " + res.message);

                //Error handeling;
            }
        }
        private async void OnOpenShow(object sender, EventArgs e)
        {

            Debug.WriteLine("LOG: SER: " +series.fullImagePortrait);
            await Navigation.PushAsync(new ShowPage(series));
        }
    }
}