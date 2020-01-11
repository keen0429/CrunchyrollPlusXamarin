using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CrunchyrollPlus
{
    struct Media
    {
        string iD;
        string name;
        string description;
        string largeImage;
        bool freeAvailable;
        bool premiumAvailable;
        public int playhead;

        public Media(string iD, string name, string description, string largeImage, bool freeAvailable, bool premiumAvailable, int playhead)
        {
            this.iD = iD;
            this.name = name;
            this.description = description;
            this.largeImage = largeImage;
            this.freeAvailable = freeAvailable;
            this.premiumAvailable = premiumAvailable;
            this.playhead = playhead;
        }
        public Media(JObject o)
        {
            iD = o["media_id"].ToString();
            name = (string)o["name"];
            description = (string)o["description"];
            largeImage = (string)o["screenshot_image"]["large_url"];
            freeAvailable = (bool)o["free_available"];
            premiumAvailable = (bool)o["premium_available"];
            if (o.ContainsKey("playhead"))
            {
                playhead = (int)o["playhead"];
            }
            else
            {
                playhead = 0;
            }



        }
    }
}
