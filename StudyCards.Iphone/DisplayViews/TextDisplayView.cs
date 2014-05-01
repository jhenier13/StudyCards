using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;

namespace StudyCards.Iphone.DisplayViews
{
    public class TextDisplayView : UIView, IDisplayView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private DrawingContent _content;
        private TextContent __text;
        //UIControls
        private UITextView __textView;

        public DrawingContent Content
        {
            get{ return _content; }
            set
            {
                _content = value;
                __text = _content as TextContent;

                if (!__isLoaded)
                    return;

                this.DrawContent();
            }
        }

        public TextDisplayView()
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

        private void DrawContent()
        {
            __textView.Text = __text.Text;
            __textView.TextAlignment = UITextAlignment.Center;
            __textView.TextColor = DrawingUtils.CreateColor(__text.Color);
            __textView.Font = UIFont.FromName(__text.FontFamily, __text.FontSize);
        }

        private void CreateUIControls()
        {
            __textView = new UITextView();
            __textView.BackgroundColor = UIColor.Clear;
            __textView.Editable = false;
        }

        private void AddUIControls()
        {
            this.Add(__textView);
        }

        private void UpdateLayout()
        {
            PointF centerPosition = new PointF(this.Frame.Width / 2, this.Frame.Height / 2);
            __textView.Center = centerPosition;

            SizeF availableSize = new SizeF(this.Frame.Width, this.Frame.Height);
            SizeF fittingSize = __textView.SizeThatFits(availableSize);
            __textView.Bounds = new RectangleF(0, 0, fittingSize.Width, fittingSize.Height);
        }
    }
}

