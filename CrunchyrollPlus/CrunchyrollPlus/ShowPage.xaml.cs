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
    public partial class ShowPage : ContentPage
    {
        CrunchyrollApi crunchyrollApi = CrunchyrollApi.GetSingleton();
        Collection[] collections = null;

        public ShowPage(Series series)
        {
            InitializeComponent();
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
            CrunchyrollApi.GetCollectionsResponse res = await crunchyrollApi.GetCollections(series.id);
            if (res.success)
            {
                collections = res.collections;
                PopulatePicker(res.collections);
                UpdateMedia(0);
            }
            else
            {
                // Error handeling
            }
        }


        private void GetCollection()
        {

        }
        private void GetMedia()
        {

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
            if (res.success)
            {
                foreach (Media i in res.medias)
                {
                    medias.Children.Add(new MediaView(i));
                }
            }
            else
            {
                // Error handeling
            }
        }
    }
}