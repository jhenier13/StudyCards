using System;

namespace StudyCards.Iphone.DrawingViews
{
    public interface IDrawingViewModalController
    {
        IDrawingView DrawingView { get; set; }

        event EventHandler ViewModalAccepted;
        event EventHandler ViewModalCanceled;

        bool HasNextModalController();

        IDrawingViewModalController NextModalController();
    }
}

