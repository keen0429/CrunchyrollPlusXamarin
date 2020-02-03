using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowPage : ContentPage
    {
        CrunchyrollApi crunchyrollApi = CrunchyrollApi.GetSingleton();
        Collection[] collections = null;
        private int currentMaxMediaShow =0;
        private Media[] medias;
        Series series;
        bool inQueue = false;

        public ShowPage(Series series)
        {
            InitializeComponent();
            this.series = series;
            showThumbnail.Source = series.fullImagePortrait;
            showThumbnail.WidthRequest = Application.Current.MainPage.Width;
            showThumbnail.HeightRequest = Application.Current.MainPage.Width * 1.5; // this is the ratio for crunchyroll portrait images
            description.Text = series.description;
            name.Text = series.name;
            showThumbnail.Source = series.fullImagePortrait;
            Init(series);

        }
        private async void Init(Series series)
        {
            InitQueueButton();
            CrunchyrollApi.GetCollectionsResponse res = await crunchyrollApi.GetCollections(series.id);
            if (res.success)
            {
                
                
                collections = res.collections;
                PopulatePicker(res.collections);
            }
            else
            {
                // Error handeling
            }
        }

        private async void InitQueueButton()
        {
            CrunchyrollApi.GetQueueResponse res = await crunchyrollApi.GetQueue();

            if (res.success)
            {
                foreach(CrunchyrollApi.QueueEntry i in res.entry)
                {
                    if (i.series.id == series.id)
                    {
                        inQueue =  true;
                        break;
                    }
                    
                }
                if (inQueue) queueToggle.Text = "Remove from queue";
                queueToggle.IsVisible = true;

            }
        }
        
        async void ToggleQueue(object sender, EventArgs e)
        {
            if (inQueue)
            {
                if (await crunchyrollApi.RemoveFromQueue(series.id))
                {
                    queueToggle.Text = "Add to queue";
                    inQueue = false;
                }
                else
                {
                    // Error handeling
                }
            }
            else
            {
                if (await crunchyrollApi.AddToQueue(series.id))
                {
                    queueToggle.Text = "Remove from queue";
                    inQueue = true;
                }
                else
                {
                    // Error handeling
                }
            }
            
        }


        private void PopulatePicker(Collection[] collections)
        {
            foreach (Collection i in collections)
            {
                selectCollection.Items.Add(i.name);
            }
            selectCollection.SelectedIndex = 0;
        }
         void OnCollectionChange(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            UpdateMedia(picker.SelectedIndex);

        }
        private async void UpdateMedia(int index)
        {
            CrunchyrollApi.ListMediaResponse res = await crunchyrollApi.GetMedias(collections[index].id);

            medias = res.medias;
            showMedias.Children.Clear();
            if (res.success)
            {
                int max = res.medias.Length > 30 ? 30 : res.medias.Length;
                currentMaxMediaShow = max;
                for(int i = 0; i < max; i++)
                {
                    showMedias.Children.Add( new MediaView(res.medias[i],false,res.medias,i));
                }
                if (max != res.medias.Length)
                {

                    Button button = new Button { Text = "Show more episodes" };
                    button.Clicked += ExtendMedia;
                    container.Children.Add(button);
                }





            }
            else
            {
                // Error handeling//
            }
        }
        private async void ExtendMedia(object sender, EventArgs e)
        {
            int max = medias.Length > (currentMaxMediaShow + 30) ? currentMaxMediaShow + 30 : medias.Length;
            if (max == medias.Length) container.Children.Remove((Button)sender);
            for(int i = currentMaxMediaShow; i < max; i++)
            {
                showMedias.Children.Add(new MediaView(medias[i], false,medias,i));
            }
            currentMaxMediaShow = max;
        }
    }
}