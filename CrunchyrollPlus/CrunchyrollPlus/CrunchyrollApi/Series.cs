using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;


namespace CrunchyrollPlus
{
    public struct Series
    {
        public string id;
        public string name;
        public string description;
        public string fullImagePortrait;
        public string largeImageLandscape;
        public Series(string id, string name, string description, string fullImagePortrait,  string largeImageLandscape)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.fullImagePortrait = fullImagePortrait;
            this.largeImageLandscape = largeImageLandscape;
        }
        public Series(JObject s)
        {
            id = (string)s["series_id"];
            name = (string)s["name"];
            description = (string)s["description"];
            fullImagePortrait = (string)s["portrait_image"]["full_url"];
            largeImageLandscape = (string)s["landscape_image"]["large_url"];

        }
    }
}
