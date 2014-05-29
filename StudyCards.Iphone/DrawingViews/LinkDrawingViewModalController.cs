using System;
using LobaSoft.IOS.UIComponents.Frames;
using MonoTouch.UIKit;
using System.Drawing;

namespace StudyCards.Iphone.DrawingViews
{
    public class LinkDrawingViewModalController : DrawingViewModalController
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private string __labelValue;
        private string __urlValue;
        //UIControls
        private GridView __innerFrame;
        private UILabel __title;
        private UITextField __label;
        private UILabel __httpLabel;
        private UITextField __url;
        private UIButton __accept;
        private UIButton __cancel;
        private UIView __windowBackground;

        public string Label
        { 
            get{ return (__label == null) ? string.Empty : __label.Text; } 
            set
            {
                __labelValue = value;

                if (__label == null)
                    return;

                __label.Text = __labelValue;
            }
        }

        public string Url
        { 
            get{ return (__url == null) ? string.Empty : __url.Text; }
            set
            {
                __urlValue = value;

                if (__url == null)
                    return;

                __url.Text = __urlValue;
            }
        }

        public LinkDrawingViewModalController()
        {
            this.View.BackgroundColor = UIColor.FromWhiteAlpha(0.4F, 0.3F);
        }

        public override void LoadView()
        {
            base.LoadView();
            this.CreateGrid();
            this.CreateUIControls();
            this.AddUIControls();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!__isLoaded)
                this.UpdateLayout();

            __label.Text = __labelValue;
            __url.Text = __urlValue;

            __isLoaded = true;
        }

        public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            __label.ResignFirstResponder();
            __url.ResignFirstResponder();
        }

        public override bool HasNextModalController()
        {
            return false;
        }

        public override IDrawingViewModalController NextModalController()
        {
            throw new InvalidOperationException("This ModalController doesn't has next modal controller");
        }

        private void CreateGrid()
        {
            __innerFrame = new GridView();
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.AddRowsAndColumns("0.2*;25;40;40;25;0.8*", "0.5*;130;130;0.5*");
        }

        private void CreateUIControls()
        {
            __windowBackground = new UIView();
            __windowBackground.BackgroundColor = UIColor.Gray;

            __title = new UILabel();
            __title.Text = "Web-Link properties";
            __title.TextAlignment = UITextAlignment.Center;

            __label = new UITextField();
            __label.BorderStyle = UITextBorderStyle.RoundedRect;
            __label.Placeholder = "Label";
            __label.EditingChanged += this.LabelText_Changed;

            __httpLabel = new UILabel();
            __httpLabel.AdjustsFontSizeToFitWidth = true;
            __httpLabel.Frame = new RectangleF(0, 0, 40, 0);
            __httpLabel.Text = "http://";

            __url = new UITextField();
            __url.KeyboardType = UIKeyboardType.Url;
            __url.BorderStyle = UITextBorderStyle.RoundedRect;
            __url.Placeholder = "URL";

            __accept = new UIButton(UIButtonType.System);
            __accept.Enabled = false;
            __accept.SetTitle("Ok", UIControlState.Normal);
            __accept.TouchDown += this.Accept_TouchDown;

            __cancel = new UIButton(UIButtonType.System);
            __cancel.SetTitle("Cancel", UIControlState.Normal);
            __cancel.TouchDown += this.Cancel_TouchDown;
        }

        private void AddUIControls()
        {
            __innerFrame.AddChild(__windowBackground, 1, 1, 4, 2);
            __innerFrame.AddChild(__title, 1, 1, 1, 2);
            __innerFrame.AddChild(__label, 2, 1, 1, 2);
            __innerFrame.SetMargin(__label, new SubViewThickness(4.0F));
            __innerFrame.AddChild(__httpLabel, 3, 1, 1, 2, true, false, GridHorizontalAlignment.Left);
            __innerFrame.AddChild(__url, 3, 1, 1, 2);
            __innerFrame.SetMargin(__url, new SubViewThickness(45, 4.0F, 4.0F, 4.0F));
            __innerFrame.AddChild(__accept, 4, 1);
            __innerFrame.AddChild(__cancel, 4, 2);

            this.Add(__innerFrame);
        }

        private void UpdateLayout()
        {
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.UpdateChildrenLayout();
        }

        private void Accept_TouchDown(object sender, EventArgs e)
        {
            this.Accept();
        }

        private void Cancel_TouchDown(object sender, EventArgs e)
        {
            this.Cancel();
        }

        private void LabelText_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(__label.Text))
                __accept.Enabled = false;
            else
                __accept.Enabled = true;
        }
    }
}

