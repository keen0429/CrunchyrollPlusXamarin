using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace CrunchyrollPlus
{
    public struct Media
    {
        public string seriesId { get; set; }
        public string iD { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string fullImage { get; set; }
        public bool freeAvailable { get; set; }
        public bool premiumAvailable { get; set; }
        public int playhead { get; set; }
        public string episodeNumber { get; set; }
        public string collectionId { get; set; }
        public int duration { get; set; }

        public string thumbnail { get; set; }

        public string largeImageStar { get; set; }

        //public Media(string iD, string name, string description, string largeImage, bool freeAvailable, bool premiumAvailable,
        //    int playhead, string seriesId, string episodeNumber, string collectionId, string largeImageStar, int duration,string thumbnail)
        //{
            
        //    this.iD = iD;
        //    this.name = name;
        //    this.description = description;
        //    this.fullImage = largeImage;
        //    this.freeAvailable = freeAvailable;
        //    this.premiumAvailable = premiumAvailable;
        //    this.playhead = playhead;
        //    this.seriesId = seriesId;
        //    this.episodeNumber = episodeNumber;
        //    this.collectionId = collectionId;
        //    this.largeImageStar = largeImageStar;
        //    this.duration = duration;

        //}
        public Media(JObject o)
        {
            Console.WriteLine("LOG: MEDIA CONSTRUCTOR REACHED");
            iD = o["media_id"].ToString();
            name = (string)o["name"];
            description = (string)o["description"];
            fullImage = (string)o["screenshot_image"]["full_url"];
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
                playhead = -1;
            }
            collectionId = (string)o["collection_id"];
            largeImageStar = (string)o["screenshot_image"]["fwidestar_url"];


            if (o.ContainsKey("duration")) {
                duration = (int)o["duration"];
            }
            else
            {
                duration = -1;
            }
            double thumbnailWidth = Application.Current.MainPage.Width * 0.3;


            if (thumbnailWidth < 100)
            {
                thumbnail = (string)o["screenshot_image"]["medium_url"];

            }
            else if (thumbnailWidth < 300)
            {
                thumbnail = (string)o["screenshot_image"]["large_url"];

            }
            else
            {
                thumbnail = (string)o["screenshot_image"]["full_url"];

            }


        }
    }
}
