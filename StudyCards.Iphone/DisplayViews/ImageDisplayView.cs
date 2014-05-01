using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Drawing;
using StudyCards.Mobile.DrawingElements;

namespace StudyCards.Iphone.DisplayViews
{
    public class ImageDisplayView : UIView, IDisplayView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private DrawingContent _content;
        private ImageContent __image;
        //UIControls
        private UIImageView __imageContainer;

        public DrawingContent Content
        { 
            get{ return _content; }
            set
            {
                _content = value;
                __image = _content as ImageContent;

                if (!__isLoaded)
                    return;

                this.DrawContent();
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

        public ImageDisplayView()
        {
            this.CreateUIControls();
            this.AddUIControls();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.UpdateLayout();
            this.DrawContent();
            __isLoaded = true;
        }

        private void DrawContent()
        {
            UIImage image = UIImage.FromFile(__image.Source);
            __imageContainer.Image = image;
        }

        private void CreateUIControls()
        {
            __imageContainer = new UIImageView();
            __imageContainer.ContentMode = UIViewContentMode.ScaleAspectFit;
        }

        private void AddUIControls()
        {
            this.Add(__imageContainer);
        }

        private void UpdateLayout()
        {
            __imageContainer.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
        }
    }
}

