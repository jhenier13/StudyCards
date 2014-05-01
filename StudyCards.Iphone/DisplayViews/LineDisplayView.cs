using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;

namespace StudyCards.Iphone.DisplayViews
{
    public class LineDisplayView : UIView, IDisplayView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private DrawingContent _content;
        private LineContent __line;
        //UIControls
        private UITextView __lineView;

        public DrawingContent Content
        { 
            get{ return _content; }
            set
            {
                _content = value;
                __line = _content as LineContent;

                if (!__isLoaded)
                    return;

                this.DrawContent();
            }
        }

        public LineDisplayView()
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
            __lineView.Text = __line.Content;
            __lineView.TextColor = DrawingUtils.CreateColor(__line.Color);
            __lineView.TextAlignment = DrawingUtils.ConvertToUITextAlignment(__line.Alignment);
            __lineView.Font = DrawingUtils.CreateFont(__line.FontFamily, __line.FontSize, __line.IsBold);
        }

        private void CreateUIControls()
        {
            __lineView = new UITextView();
            __lineView.Editable = false;
            __lineView.BackgroundColor = UIColor.Clear;
        }

        private void AddUIControls()
        {
            this.Add(__lineView);
        }

        private void UpdateLayout()
        {
            PointF center = new PointF(this.Frame.Width / 2, this.Frame.Height / 2);
            __lineView.Center = center;

            SizeF availableSize = this.Frame.Size;
            SizeF fittedSize = __lineView.SizeThatFits(availableSize);
            __lineView.Bounds = new RectangleF(0, 0, fittedSize.Width, fittedSize.Height);
        }
    }
}

