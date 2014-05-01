using System;
using MonoTouch.UIKit;

namespace StudyCards.Iphone.DrawingViews
{
    public abstract class DrawingViewModalController : UIViewController, IDrawingViewModalController
    {
        public IDrawingView DrawingView { get; set; }

        public event EventHandler ViewModalCanceled;
        public event EventHandler ViewModalAccepted;

        public DrawingViewModalController()
        {
        }

        protected void Cancel()
        {
            var handler = this.ViewModalCanceled;

            if (handler != null)
                handler(this, new EventArgs());
        }

        protected void Accept()
        {
            var handler = this.ViewModalAccepted;

            if (handler != null)
                handler(this, new EventArgs());
        }

        public abstract bool HasNextModalController();

        public abstract IDrawingViewModalController NextModalController();
    }
}

