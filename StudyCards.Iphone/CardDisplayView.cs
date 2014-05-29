using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Iphone.DrawingViews;
using StudyCards.Iphone.SubViews;
using LobaSoft.IOS.UIComponents;

namespace StudyCards.Iphone
{
    public class CardDisplayView : UIView, IDisposableView
    {
        //Flags
        private bool __isLoaded = false;
        private bool __isFront = true;
        private bool __gesturesAttached = false;
        //Attributes
        private List<CardRelation> _frontCardElements;
        private List<CardRelation> _backCardElements;
        //UIControls
        private UICardView __frontCard;
        private UICardView __backCard;
        private UIImage _cardBackground;
        //Gestures
        private UISwipeGestureRecognizer __upwardSwipe;
        private UISwipeGestureRecognizer __downwardSwipe;

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;

                if (!__isLoaded)
                    return;

                this.UpdateLayout();
            }
        }

        public UIImage CardBackground
        {
            get { return _cardBackground; }
            set
            {
                _cardBackground = value;
                __frontCard.CardBackground = _cardBackground;
                __backCard.CardBackground = _cardBackground;
            }

        }

        public List<CardRelation> FrontCardElements
        {
            get{ return _frontCardElements; }
            set
            {
                _frontCardElements = value;
                __frontCard.Elements = _frontCardElements;
            }
        }

        public List<CardRelation> BackCardElements
        {
            get{ return _backCardElements; }
            set
            {
                _backCardElements = value;
                __backCard.Elements = _backCardElements;
            }
        }

        public CardDisplayView()
        {
            this.CreateUIControls();
            this.AddUIControls();
            this.CreateGestures();
            this.AttachGestures();
        }

        public void AttachEventHandlers()
        {
            this.AttachGestures();
        }

        public void DetachEventHandlers()
        {
            this.DetachGestures();
        }

        public void CleanSubViews()
        {
        }

        public void AddSubViews()
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.UpdateLayout();
            __isLoaded = true;
        }

        private void CreateUIControls()
        {
            __frontCard = new UICardView();
            __backCard = new UICardView();
        }

        private void AddUIControls()
        {
            this.Add(__frontCard);
        }

        private void UpdateLayout()
        {
            __frontCard.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
            __backCard.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
        }

        private void CreateGestures()
        {
            __upwardSwipe = new UISwipeGestureRecognizer(this.UpwardSwipeEvent);
            __upwardSwipe.NumberOfTouchesRequired = 1;
            __upwardSwipe.CancelsTouchesInView = false;
            __upwardSwipe.Direction = UISwipeGestureRecognizerDirection.Up;
            __upwardSwipe.ShouldRecognizeSimultaneously = new UIGesturesProbe((a, b) => true);

            __downwardSwipe = new UISwipeGestureRecognizer(this.DownwardSwipeEvent);
            __downwardSwipe.NumberOfTouchesRequired = 1;
            __downwardSwipe.Direction = UISwipeGestureRecognizerDirection.Down;
            __downwardSwipe.CancelsTouchesInView = false;
            __downwardSwipe.ShouldRecognizeSimultaneously = new UIGesturesProbe((a, b) => true);
        }

        private void AttachGestures()
        {
            if (__gesturesAttached)
                return;

            this.AddGestureRecognizer(__upwardSwipe);
            this.AddGestureRecognizer(__downwardSwipe);
            __gesturesAttached = true;
        }

        private void DetachGestures()
        {
            if (!__gesturesAttached)
                return;

            this.RemoveGestureRecognizer(__upwardSwipe);
            this.RemoveGestureRecognizer(__downwardSwipe);
            __gesturesAttached = false;
        }

        private void UpwardSwipeEvent(UISwipeGestureRecognizer gesture)
        {
            if (__isFront)
                UIView.Transition(__frontCard, __backCard, DrawingViewConstants.CARD_FLIP_ANIMATION_DURATION, UIViewAnimationOptions.TransitionFlipFromTop | UIViewAnimationOptions.CurveEaseInOut, null);
            else
                UIView.Transition(__backCard, __frontCard, DrawingViewConstants.CARD_FLIP_ANIMATION_DURATION, UIViewAnimationOptions.TransitionFlipFromTop | UIViewAnimationOptions.CurveEaseInOut, null);

            __isFront = !__isFront;
        }

        private void DownwardSwipeEvent(UISwipeGestureRecognizer gesture)
        {
            if (__isFront)
                UIView.Transition(__frontCard, __backCard, DrawingViewConstants.CARD_FLIP_ANIMATION_DURATION, UIViewAnimationOptions.TransitionFlipFromBottom | UIViewAnimationOptions.CurveEaseInOut, null);
            else
                UIView.Transition(__backCard, __frontCard, DrawingViewConstants.CARD_FLIP_ANIMATION_DURATION, UIViewAnimationOptions.TransitionFlipFromBottom | UIViewAnimationOptions.CurveEaseInOut, null);

            __isFront = !__isFront;
        }
    }
}

