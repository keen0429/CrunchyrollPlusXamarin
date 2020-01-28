using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FormsVideoLibrary;


namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        CrunchyrollApi crunchyApi = CrunchyrollApi.GetSingleton();
        string mediaId;
        int index;
        Media[] medias;
        //string url = "https://dl.v.vrv.co/evs/e6214caef17a0f7a82d17e689d523c3b/assets/8849cd688573ec6291d129fd8ebbc3a5_,3556948.mp4,3556949.mp4,3556947.mp4,3556945.mp4,3556946.mp4,.urlset/master.m3u8?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cCo6Ly9kbC52LnZydi5jby9ldnMvZTYyMTRjYWVmMTdhMGY3YTgyZDE3ZTY4OWQ1MjNjM2IvYXNzZXRzLzg4NDljZDY4ODU3M2VjNjI5MWQxMjlmZDhlYmJjM2E1XywzNTU2OTQ4Lm1wNCwzNTU2OTQ5Lm1wNCwzNTU2OTQ3Lm1wNCwzNTU2OTQ1Lm1wNCwzNTU2OTQ2Lm1wNCwudXJsc2V0L21hc3Rlci5tM3U4IiwiQ29uZGl0aW9uIjp7IkRhdGVMZXNzVGhhbiI6eyJBV1M6RXBvY2hUaW1lIjoxNTgwMjAwOTc4fX19XX0_&Signature=g~whi2pWg53ZeyvpkbRwqvNioqVlHLckzwm0mafjzGpQbzQrgGXR9UVJJVC5vpV8RmPtv~5hSNAlUFIS25iJaqRDXjzWXzlzIXE4fA7s4DrcwGbDQ1qlXJAuSmPIDfslXX9sPQlPkFPlea42qcVvx~l0k~O39OWofIPeQAA0uFWntP0zkCsVolLCRQaEhSQx3k7k59fCw0p5w7hL96zw2n3lmWZSXJxEnDDOGn8zi0ggZaiDVsuarET-7oCc1EyyGM-WWNCpBR43YyjlZBSeszG~bG4wJ0bRntPk~PWeQo0RfCRXpk3l6hfHcuqEb3GJJATXBimU7V46i-SAra-7PQ__&Key-Pair-Id=DLVR";
        public Player(string mediaId,int index, Media[] medias)
        {
            
            InitializeComponent();
            this.mediaId = mediaId;
            this.index = index;
            this.medias = medias;
            InitSource();
        }
        async private void InitSource()
        {
            CrunchyrollApi.StreamDataResponse res = await crunchyApi.GetStreamData(mediaId);

            videoPlayer.Source = FormsVideoLibrary.VideoSource.FromUri(res.url);
            videoPlayer.Duration.Add(new TimeSpan(0, 0, res.playhead));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("LOG: METHOD RAN");
            IDeviceOrientationService service = DependencyService.Get<IDeviceOrientationService>();
            service.ForceLandscape();
            DependencyService.Get<IFullscreenService>().EnterFullscreen();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DependencyService.Get<IDeviceOrientationService>().ForcePortrait();
            DependencyService.Get<IFullscreenService>().ExitFullscreen();

        }

        private void playPause_Clicked(object sender, EventArgs e)
        {
            if (videoPlayer.Status == VideoStatus.Playing)
            {
                videoPlayer.Pause();
                playPause.Source = "play.png";
            }
            else if (videoPlayer.Status == VideoStatus.Paused)
            {
                videoPlayer.Play();
                playPause.Source = "pause.png";
            }
            else
            {

            }
            
        }

        private void rewind_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("LOG: REWIND");

            videoPlayer.Position = videoPlayer.Position.Subtract(new TimeSpan(0, 0, 10));
        }

        private void forward_Clicked(object sender, EventArgs e)
        {
            videoPlayer.Position = videoPlayer.Position.Add(new TimeSpan(0, 0, 10));

        }


        async private void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}