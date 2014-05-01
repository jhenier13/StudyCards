using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;
using UIComponents.CustomControls;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.DrawingViews
{
    public class TextDrawingView : UIView, IDrawingView
    {
        private const float LEFT_MARGIN = 3;
        private const float TOP_MARGIN = 3;
        private const string DEFAULT_TEXT = "[ Description ]";
        //Flags
        private bool __isLoaded;
        //Attributes
        private DrawingContent _content;
        private TextContent __text;
        //UIControls
        private UICustomTextView __inputField;

        public DrawingContent Content
        {
            get{ return _content; }
            set
            {
                _content = value;
                __text = (TextContent)value;

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
            //The InputField has the same size that the container, so we return the container frame
            get{ return this.Frame; }
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

        public override bool IsFirstResponder
        {
            get
            {
                return __inputField.IsFirstResponder;
            }
        }

        public event RequiresModalControllerEventHandler RequiresModalController;

        public TextDrawingView()
        {
            this.CreateUIControls();
            this.BackgroundColor = UIColor.Clear;
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

        public override bool ResignFirstResponder()
        {
            return __inputField.ResignFirstResponder();
        }

        private RectangleF __containerFrameBeforeAnimation;

        public void AdjustToKeyboard(RectangleF keyboardOverlapingFrame)
        {
            float remainingMovement = 0;
            float adjustedYPosition = this.Frame.Y - keyboardOverlapingFrame.Height;
            __containerFrameBeforeAnimation = this.Frame;

            if (adjustedYPosition < 0)
            {
                remainingMovement = adjustedYPosition * -1;
                adjustedYPosition = 0;
            }

            UIView.Animate(0.4, new NSAction(() =>
            {
                this.Frame = new RectangleF(this.Frame.X, adjustedYPosition, this.Frame.Width, this.Frame.Height - remainingMovement);
            }));
        }

        public void RestoreToHideKeyboard()
        {
            UIView.Animate(0.4, new NSAction(() =>
            {
                this.Frame = __containerFrameBeforeAnimation;
            }));
        }

        public void CommitData()
        {
            __text.Text = __inputField.Text;
        }

        public void DrawingModalControllerAccepted(IDrawingViewModalController modalController)
        {
            //Do nothing
        }

        public void DrawingModalControllerCanceled(IDrawingViewModalController modalController)
        {
            //Do nothing
        }

        private void DrawContent()
        {
            if (__text == null)
                return;

            __inputField.Font = DrawingUtils.CreateFont(__text.FontFamily, __text.FontSize, false);
            __inputField.TextColor = DrawingUtils.CreateColor(__text.Color);

            __inputField.Text = __text.Text;
        }

        private void CreateUIControls()
        {
            __inputField = new UICustomTextView();
            __inputField.Layer.CornerRadius = 5;
            __inputField.BackgroundColor = UIColor.FromWhiteAlpha(0.8F, 0.3F);
            __inputField.TextAlignment = UITextAlignment.Justified;
            __inputField.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);
            __inputField.Placeholder = DEFAULT_TEXT;
        }

        private void AddUIControls()
        {
            __inputField.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);
            this.Add(__inputField);
        }

        private void UpdateLayout()
        {
            if (!__isLoaded)
                return;

            __inputField.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);
        }
    }
}

