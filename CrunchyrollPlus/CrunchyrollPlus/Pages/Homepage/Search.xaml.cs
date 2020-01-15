using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        CrunchyrollApi crunchyApi = CrunchyrollApi.GetSingleton();
        public Search()
        {
            InitializeComponent();
        }

        private async void SearchAnime(object sender, EventArgs e)
        {
            searchResults.Children.Clear();
            Debug.WriteLine("LOG: SEARCH: CALL");
            CrunchyrollApi.AutocompleteResponse res = await crunchyApi.Autocomplete(searchQuery.Text);
            if (res.success)
            {
                foreach(Series i in res.series)
                {
                    searchResults.Children.Add(new SearchResult(i));
                }
            }
        }
    }
}