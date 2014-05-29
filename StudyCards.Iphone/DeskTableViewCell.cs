using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace StudyCards.Iphone
{
    public class DeskTableViewCell : UITableViewCell
    {
        private const float TOP_MARGIN = 4.0F;
        private const float RIGHT_MARGIN = 10.0F;
        private const float BOTTOM_MARGIN = 4.0F;
        private const float LEFT_MARGIN = 10.0F;
        private const float NAME_HEIGHT = 40.0F;
        public UIView __innerContent;

        public UIImageView BackgroundContainer { get; private set; }

        public UILabel NameLabel { get; private set; }

        public DeskTableViewCell(string cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            __innerContent = new UIView();
            __innerContent.Layer.CornerRadius = 10.0F;
            __innerContent.ClipsToBounds = true;
            __innerContent.Layer.BorderColor = UIColor.Brown.CGColor;
            __innerContent.Layer.BorderWidth = 3.5F;

            this.BackgroundContainer = new UIImageView();
            this.BackgroundContainer.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            __innerContent.Add(this.BackgroundContainer);

            this.NameLabel = new UILabel();
            this.NameLabel.BackgroundColor = UIColor.FromWhiteAlpha(0.2F, 0.4F);
            this.NameLabel.TextColor = UIColor.White;
            this.NameLabel.TextAlignment = UITextAlignment.Center;
            this.NameLabel.Font = UIFont.BoldSystemFontOfSize(25);
            this.NameLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            __innerContent.Add(this.NameLabel);

            this.ContentView.Add(__innerContent);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            __innerContent.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.ContentView.Frame.Width - LEFT_MARGIN - RIGHT_MARGIN, this.ContentView.Frame.Height - TOP_MARGIN - BOTTOM_MARGIN);
            this.NameLabel.Bounds = new RectangleF(0, 0, __innerContent.Frame.Width, __innerContent.Frame.Height);
            this.NameLabel.Center = new PointF(__innerContent.Frame.Width / 2, __innerContent.Frame.Height / 2);
        }
    }
}

