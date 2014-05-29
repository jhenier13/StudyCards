using System;
using MonoTouch.UIKit;
using StudyCards.Iphone.SubViews;
using LobaSoft.IOS.UIComponents;

namespace StudyCards.Iphone
{
    public partial class DeskViewerView
    {
        private void DetachCardHandlers(UIView cardViewer)
        {
            if (cardViewer is UIAddCardView)
                (cardViewer as UIAddCardView).AddTouchDown -= this.AddCard_AddTouchDown;

            if (cardViewer is IDisposableView)
                (cardViewer as IDisposableView).DetachEventHandlers();
        }

        private void AttachCardHandlers(UIView cardViewer)
        {
            if (cardViewer is UIAddCardView)
                (cardViewer as UIAddCardView).AddTouchDown += this.AddCard_AddTouchDown;

            if (cardViewer is IDisposableView)
                (cardViewer as IDisposableView).AttachEventHandlers();
        }

        private void AttachGesturesRecognizers()
        {
            this.View.AddGestureRecognizer(__tapGesture);
        }

        private void DetachGesturesRecognizers()
        {
            this.View.RemoveGestureRecognizer(__tapGesture);
        }

        private void AttachAllCardsEventHandlers()
        {
            this.AttachCardHandlers(__previousCard);
            this.AttachCardHandlers(__currentCard);
            this.AttachCardHandlers(__nextCard);
        }

        private void DetachAllCardsEventHandlers()
        {
            this.DetachCardHandlers(__previousCard);
            this.DetachCardHandlers(__currentCard);
            this.DetachCardHandlers(__nextCard);
        }
    }
}

