using System;
using StudyCards.Mobile.DrawingElements;

namespace StudyCards.Mobile
{
    public abstract class DrawingContent
    {
        public DrawPoint Position { get; set; }

        public DrawSize Size { get; set; }

        internal abstract string GenerateContentValue();

        internal abstract void LoadContentValue(string element);

        internal virtual void CommitExtraData(object parameter)
        {
        }
    }
}

