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
        public string fullImageLandscape;
        public Series(string id, string name, string description, string fullImagePortrait,  string fullImageLandscape)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.fullImagePortrait = fullImagePortrait;
            this.fullImageLandscape = fullImageLandscape;
        }
        public Series(JObject s)
        {
            id = (string)s["series_id"];
            name = (string)s["name"];
            description = (string)s["description"];
            fullImagePortrait = (string)s["portrait_image"]["full_url"];
            fullImageLandscape = (string)s["landscape_image"]["full_url"];

        }
    }
}
