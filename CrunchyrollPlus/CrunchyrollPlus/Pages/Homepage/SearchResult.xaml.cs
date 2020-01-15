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
    public partial class SearchResult : ContentView
    {
        Series s;
        public SearchResult(Series series)
        {
            s = series;

            InitializeComponent();
            thumbnail.Source = series.largeImageLandscape;
            showName.Text = series.name;
            double width = Application.Current.MainPage.Width*0.4;
            row.Height = width * 0.56255545696;

        }
        async void OnOpenShow(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowPage(s));
        }
    }
}