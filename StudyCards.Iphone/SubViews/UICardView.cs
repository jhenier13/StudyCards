using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;

namespace StudyCards.Iphone.SubViews
{
    public class UICardView : UIView
    {
        //Attributes
        private Background __cardBackground;

        public Background CardBackground
        {
            get{ return __cardBackground; }
            set
            {
                __cardBackground = value; 
                this.SetBackgroundImage();
            }
        }

        public UICardView()
        {
        }

        public UICardView(Background cardBackground)
        {
            this.CardBackground = cardBackground;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.SetBackgroundImage();

            this.Layer.CornerRadius = 10.0F;
            this.ClipsToBounds = true;
        }

        private void SetBackgroundImage()
        {
            if (this.CardBackground == null)
                return;

            UIImage image = new UIImage(this.CardBackground.Location);
            UIImage stretchedImage = image.Scale(this.Frame.Size);
            UIColor backgroundImage = UIColor.FromPatternImage(stretchedImage);
            this.BackgroundColor = backgroundImage;
        }
    }
}

