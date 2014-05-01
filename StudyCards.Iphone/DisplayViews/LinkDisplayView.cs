using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;

namespace StudyCards.Iphone.DisplayViews
{
    public class LinkDisplayView : UIView, IDisplayView
    {
        private const float TOP_MARGIN = 6.0F;
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private DrawingContent _content;
        private LinkContent __link;
        //UIControls
        private UIButton __linkView;

        public DrawingContent Content
        { 
            get{ return _content; }
            set
            {
                _content = value;
                __link = _content as LinkContent;

                if (!__isLoaded)
                    return;

                this.DrawContent();
            }
        }

        public LinkDisplayView()
        {
            this.CreateUIControls();
            this.AddUIControls();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.DrawContent();
            this.UpdateLayout();
            __isLoaded = true;
        }

        private void CreateUIControls()
        {
            __linkView = new UIButton(UIButtonType.System);
        }

        private void AddUIControls()
        {
            this.Add(__linkView);
        }

        private void DrawContent()
        {
            __linkView.TitleLabel.TextAlignment = DrawingUtils.ConvertToUITextAlignment(__link.Alignment);
            __linkView.TitleLabel.Font = UIFont.SystemFontOfSize(__link.FontSize);
            __linkView.SetTitle(__link.Label, UIControlState.Normal);
        }

        private void UpdateLayout()
        {
            float height = __link.FontSize + 2 * TOP_MARGIN;
            PointF position = new PointF(0, (this.Frame.Height - height) / 2);
            __linkView.Frame = new RectangleF(position, new SizeF(this.Frame.Width, height));
        }
    }
}

