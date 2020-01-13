using System;
using System.Collections.Generic;
using System.Text;

namespace CrunchyrollPlus
{
    public struct Collection
    {
        string name;
        string id;

        public Collection(string name, string id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
