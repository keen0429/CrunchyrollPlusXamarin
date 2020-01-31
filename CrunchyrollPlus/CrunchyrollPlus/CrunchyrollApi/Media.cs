using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CrunchyrollPlus
{
    public struct Media
    {
        public string seriesId;
        public string iD;
        public string name;
        public string description;
        public string largeImage;
        public bool freeAvailable;
        public bool premiumAvailable;
        public int playhead;
        public string episodeNumber;
        public string collectionId;

        public string largeImageStar;

        public Media(string iD, string name, string description, string largeImage, bool freeAvailable, bool premiumAvailable,
            int playhead, string seriesId, string episodeNumber, string collectionId, string largeImageStar)
        {
            
            this.iD = iD;
            this.name = name;
            this.description = description;
            this.largeImage = largeImage;
            this.freeAvailable = freeAvailable;
            this.premiumAvailable = premiumAvailable;
            this.playhead = playhead;
            this.seriesId = seriesId;
            this.episodeNumber = episodeNumber;
            this.collectionId = collectionId;
            this.largeImageStar = largeImageStar;
        }
        public Media(JObject o)
        {
            Console.WriteLine("LOG: MEDIA CONSTRUCTOR REACHED");
            iD = o["media_id"].ToString();
            name = (string)o["name"];
            description = (string)o["description"];
            largeImage = (string)o["screenshot_image"]["large_url"];
            freeAvailable = (bool)o["free_available"];
            premiumAvailable = (bool)o["premium_available"];
            seriesId = (string)o["series_id"];
            episodeNumber = (string)o["episode_number"];
            if (o.ContainsKey("playhead"))
            {
                playhead = (int)o["playhead"];
            }
            else
            {
                playhead = 0;
            }
            collectionId = (string)o["collection_id"];
            largeImageStar = (string)o["screenshot_image"]["fwidestar_url"];



        }
    }
}
