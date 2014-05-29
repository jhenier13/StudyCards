using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;

namespace StudyCards.Iphone.DrawingViews
{
    public class LinkDrawingView : UIView, IDrawingView
    {
        private const float FIELD_TOP_MARGIN = 4;
        private const float FIELD_BOTTOM_MARGIN = 4;
        private const float FIELD_DEFAULT_HEIGHT = 35;
        private static UIColor LINK_COLOR = UIColor.Blue;
        private const string DEFAULT_LABEL = "[ Link ]";
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private string __label;
        private string __webLink;
        private DrawingContent _content;
        private LinkContent __link;
        //UIControls
        private UIButton __openWeb;

        public DrawingContent Content
        {
            get{ return _content; }
            set
            {
                _content = value; 
                __link = (LinkContent)_content;
                __label = __link.Label;
                __webLink = __link.WebLink;

                if (!__isLoaded)
                    return;

                this.DrawContent();
            }
        }

        public RectangleF TotalFrame
        {
            get{ return this.Frame; }
        }

        public RectangleF UsedFrame
        {
            get
            { 
                PointF absolutePosition = new PointF(this.Frame.X + __openWeb.Frame.X, this.Frame.Y + __openWeb.Frame.Y);
                RectangleF absoluteFrame = new RectangleF(absolutePosition, __openWeb.Frame.Size);
                return absoluteFrame; 
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
                this.UpdateLayout();
            }
        }

        public event RequiresModalControllerEventHandler RequiresModalController;

        public LinkDrawingView()
        {
            this.CreateUIControls();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!__isLoaded)
            {
                this.AddUIControls();
                this.DrawContent();
            }

            __isLoaded = true;
        }

        public void AdjustToKeyboard(RectangleF keyboardOverlapingFrame)
        {
            //Do nothing
        }

        public void RestoreToHideKeyboard()
        {
            //Do nothing
        }

        public void CommitData()
        {
            __link.Label = __label;
            __link.WebLink = __webLink;
        }

        public void DrawingModalControllerAccepted(IDrawingViewModalController modalController)
        {
            LinkDrawingViewModalController linkModalController = modalController as LinkDrawingViewModalController;

            __label = linkModalController.Label;
            __webLink = linkModalController.Url;

            __openWeb.SetTitle(__label, UIControlState.Normal);
        }

        public void DrawingModalControllerCanceled(IDrawingViewModalController modalController)
        {
        }

        private void DrawContent()
        {
            if (__link == null)
                return;

            string label = (string.IsNullOrEmpty(__label)) ? DEFAULT_LABEL : __label;
            __openWeb.SetTitle(label, UIControlState.Normal);
            __openWeb.SetTitleColor(LINK_COLOR, UIControlState.Normal);
            __openWeb.TitleLabel.TextAlignment = DrawingUtils.ConvertToUITextAlignment(__link.Alignment);
            __openWeb.TitleLabel.Font = UIFont.SystemFontOfSize(__link.FontSize);
        }

        private void CreateUIControls()
        {
            __openWeb = new UIButton(UIButtonType.System);
            __openWeb.TouchDown += this.WebLink_TouchDown;
            float yPosition = (this.Frame.Height - FIELD_DEFAULT_HEIGHT) / 2;
            __openWeb.Frame = new RectangleF(0, yPosition, this.Frame.Width, FIELD_DEFAULT_HEIGHT);
        }

        private void AddUIControls()
        {
            float yPosition = (this.Frame.Height - FIELD_DEFAULT_HEIGHT) / 2;
            __openWeb.Frame = new RectangleF(0, yPosition, this.Frame.Width, FIELD_DEFAULT_HEIGHT);
            this.Add(__openWeb);
        }

        private void UpdateLayout()
        {
            if (!__isLoaded)
                return;

            float yPosition = (this.Frame.Height - FIELD_DEFAULT_HEIGHT) / 2;
            __openWeb.Frame = new RectangleF(0, yPosition, this.Frame.Width, FIELD_DEFAULT_HEIGHT);
        }

        private void RequiresModalView(RequiresModalControllerEventArgs e)
        {
            var handler = this.RequiresModalController;

            if (handler != null)
                handler(this, e);
        }

        private void WebLink_TouchDown(object sender, EventArgs e)
        {
            LinkDrawingViewModalController linkWindow = new LinkDrawingViewModalController();
            linkWindow.Label = __label;
            linkWindow.Url = __webLink;
            RequiresModalControllerEventArgs requireArgs = new RequiresModalControllerEventArgs();
            requireArgs.ModalController = linkWindow;

            this.RequiresModalView(requireArgs);
        }
    }
}

