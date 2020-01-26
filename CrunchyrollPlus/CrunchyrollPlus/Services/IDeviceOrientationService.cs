using System;
using System.Collections.Generic;
using System.Text;

namespace CrunchyrollPlus
{
    public interface IDeviceOrientationService
    {
        void ForceLandscape();
        void ForcePortrait();
    }
}
