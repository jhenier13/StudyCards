using System;
using MonoTouch.UIKit;
using StudyCards.Iphone.SubViews;
using System.Drawing;
using StudyCards.Mobile;
using System.Collections.Generic;

namespace StudyCards.Iphone
{
    public class CardDisplayView : UIView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private Background _cardBackground;
        private List<CardRelation> _frontCardElements;
        private List<CardRelation> _backCardElements;
        //UIControls
        private UICardView __frontCard;
        private UICardView __backCard;

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

        public Background CardBackground
        {
            get{ return  _cardBackground; }
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
            __backCard.Hidden = true;
        }

        private void AddUIControls()
        {
            this.Add(__frontCard);
            this.Add(__backCard);
        }

        private void UpdateLayout()
        {
            __frontCard.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
            __backCard.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
        }
    }
}

