using System;

namespace StudyCards.Mobile
{
    public class Background
    {
        public string Name { get; set; }

        public bool IsDefault { get; internal set; }

        public string Location{ get; internal set; }

        public Background()
        {
        }

        public static int Compare(Background backgroundA, Background backgroundB)
        {
            return string.Compare(backgroundA.Name, backgroundB.Name);
        }
    }
}

