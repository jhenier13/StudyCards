using System;
using MonoTouch.UIKit;
using LobaSoft.IOS.UIComponents.Frames;
using System.Drawing;

namespace StudyCards.Iphone.DrawingViews
{
    public class ImageDrawingViewModalController : DrawingViewModalController
    {
        //Flags
        private bool __isLoaded;
        //Attributes
        private UIImage __selectedImage;
        private string __tagValue;
        //UIControls
        private GridView __innerFrame;
        private UIView __windowBackground;
        private UILabel __title;
        private UIImageView __image;
        private UITextField __tag;
        private UIButton __ok;
        private UIButton __cancel;

        public UIImage SelectedImage
        {
            get{ return __selectedImage; }
            set
            {
                __selectedImage = value;

                if (__image == null)
                    return;

                __image.Image = __selectedImage;
            }
        }

        public string Tag
        {
            get { return (__tag == null) ? string.Empty : __tag.Text; }
            set
            {
                __tagValue = value;

                if (__tagValue == null)
                    return;

                __tag.Text = __tagValue;
            }
        }

        public ImageDrawingViewModalController()
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

            __image.Image = __selectedImage;
            __tag.Text = __tagValue;

            __isLoaded = true;
        }

        public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            __tag.ResignFirstResponder();
        }

        public override bool HasNextModalController()
        {
            return false;
        }

        public override IDrawingViewModalController NextModalController()
        {
            throw new InvalidOperationException("This ModalController doesn't has a next modalcontroller");
        }

        private void CreateGrid()
        {
            __innerFrame = new GridView();
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.AddRowsAndColumns("0.5*;35;40;120;35;0.5*", "0.5*;100;100;0.5*");
        }

        private void CreateUIControls()
        {
            __windowBackground = new UIView();
            __windowBackground.BackgroundColor = UIColor.Gray;

            __title = new UILabel();
            __title.AdjustsFontSizeToFitWidth = true;
            __title.TextAlignment = UITextAlignment.Center;
            __title.Text = "Add image Tag";

            __image = new UIImageView();
            __image.ContentMode = UIViewContentMode.ScaleAspectFit;

            __tag = new UITextField();
            __tag.BorderStyle = UITextBorderStyle.RoundedRect;
            __tag.Placeholder = "Tag";

            __ok = new UIButton(UIButtonType.System);
            __ok.SetTitle("Ok", UIControlState.Normal);
            __ok.TouchDown += this.Ok_TouchDown;

            __cancel = new UIButton(UIButtonType.System);
            __cancel.SetTitle("Cancel", UIControlState.Normal);
            __cancel.TouchDown += this.Cancel_TouchDown;
        }

        private void AddUIControls()
        {
            __innerFrame.AddChild(__windowBackground, 1, 1, 4, 2);
            __innerFrame.AddChild(__title, 1, 1, 1, 2);
            __innerFrame.AddChild(__image, 3, 1, 1, 2);
            __innerFrame.SetMargin(__image, new SubViewThickness(3.0F));
            __innerFrame.AddChild(__tag, 2, 1, 1, 2);
            __innerFrame.SetMargin(__tag, new SubViewThickness(3.0F));
            __innerFrame.AddChild(__ok, 4, 1);
            __innerFrame.AddChild(__cancel, 4, 2);

            this.Add(__innerFrame);
        }

        private void UpdateLayout()
        {
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.UpdateChildrenLayout();
        }

        private void Ok_TouchDown(object sender, EventArgs e)
        {
            this.Accept();
        }

        private void Cancel_TouchDown(object sender, EventArgs e)
        {
            this.Cancel();
        }
    }
}

