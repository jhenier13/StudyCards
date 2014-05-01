using System;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;

namespace StudyCards.Iphone.DrawingViews
{
    public static class DrawingViewFactory
    {
        public static IDrawingView CreateDrawingView(TemplateElement drawingElement)
        {
            if (drawingElement is LineElement)
                return new LineDrawingView();
            else if (drawingElement is TextElement)
                return new TextDrawingView();
            else if (drawingElement is LinkElement)
                return new LinkDrawingView();
            else if (drawingElement is ImageElement)
                return new ImageDrawingView();
            else
                throw new ArgumentException("A DrawingView for this DrawingElement doesn't exist");
        }
    }
}

