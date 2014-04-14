using System;
using MonoTouch.Foundation;
using StudyCards.Mobile;

namespace StudyCards.Iphone
{
    public class IphoneResourcesManager : IDeviceResourcesManager
    {
        public string ResourcesDirectory { get { return NSBundle.MainBundle.BundlePath; } }

        public IphoneResourcesManager()
        {
        }
    }
}

