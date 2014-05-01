using System;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;

namespace StudyCards.Iphone.DisplayViews
{
    public static class DisplayViewFactory
    {
        public static IDisplayView CreateDisplayView(DrawingContent content)
        {
            if (content is TextContent)
                return new TextDisplayView();
            else if (content is LineContent)
                return new LineDisplayView();
            else if (content is LinkContent)
                return new LinkDisplayView();
            else if (content is ImageContent)
                return new ImageDisplayView();
            else
                throw new ArgumentException("This content doesn't has a DisplayView");
        }
    }
}

