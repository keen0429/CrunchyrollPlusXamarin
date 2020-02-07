using System;
using System.Collections.Generic;
using System.Text;

namespace CrunchyrollPlus
{
    public interface IToastService
    {
        void ShowToastShort(string message);
        void ShowToastLong(string message);

    }
}
