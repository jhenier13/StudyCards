using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Drawing;

namespace StudyCards.Iphone.SubViews
{
    public class UIAddCardView : UIView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private UIImage __background;
        //UIControls
        private UIImageView __backgroundViewer;
        private UIButton __addCard;
        private UIView __addCardContainer;

        public UIImage CardBackground
        {
            get { return __background; }
            set
            {
                __background = value;
                this.SetCardBackground();
            }
        }

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

        public event EventHandler AddTouchDown;

        public UIAddCardView()
        {
            this.Layer.CornerRadius = 10.0F;
            this.ClipsToBounds = true;

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
            __backgroundViewer = new UIImageView();
            __backgroundViewer.ContentMode = UIViewContentMode.ScaleAspectFill;

            __addCardContainer = new UIView();
            __addCardContainer.Layer.CornerRadius = 10.0F;
            __addCardContainer.Layer.BorderColor = UIColor.Gray.CGColor;
            __addCardContainer.Layer.BorderWidth = 1.0F;
            __addCardContainer.Layer.ShadowColor = UIColor.Gray.CGColor;
            __addCardContainer.Layer.ShadowOpacity = 0.8F;
            __addCardContainer.Layer.ShadowRadius = 2.0F;
            __addCardContainer.Layer.ShadowOffset = new SizeF(2, 2);
            __addCardContainer.Layer.ContentsScale = 0.9F;

            __addCard = new UIButton(UIButtonType.Custom);
            __addCard.SetImage(UIImage.FromFile("AddIcon.png"), UIControlState.Normal);

            __addCard.TouchDown += (object sender, EventArgs e) =>
            {
                var handler = this.AddTouchDown;

                if (handler != null)
                    handler(this, new EventArgs());
            };
        }

        private void AddUIControls()
        {
            this.Add(__backgroundViewer);
            __addCardContainer.Add(__addCard);
            this.Add(__addCardContainer);
        }

        private void SetCardBackground()
        {
            __backgroundViewer.Image = __background;
        }

        private void UpdateLayout()
        {
            __backgroundViewer.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
            __addCardContainer.Center = new PointF(this.Frame.Width / 2, this.Frame.Height / 2);
            __addCardContainer.Bounds = new RectangleF(0, 0, 150, 150);
            __addCard.Frame = new RectangleF(10, 10, 130, 130);
        }
    }
}

