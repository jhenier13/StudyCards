using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Drawing;
using StudyCards.Mobile.DrawingElements;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.DrawingViews
{
    public class LineDrawingView : UIView, IDrawingView
    {
        private const float FIELD_TOP_MARGIN = 4;
        private const float FIELD_BOTTOM_MARGIN = 4;
        private const float FIELD_DEFAULT_HEIGHT = 35;
        private const string DEFAULT_CONTENT = "[ Title ]";
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private DrawingContent _content;
        private LineContent __line;
        //UIControls
        private UITextField __inputField;

        public DrawingContent Content
        {
            get{ return _content; }
            set
            {
                _content = value;
                __line = (LineContent)_content;

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
                PointF absolutePosition = new PointF(this.Frame.X + __inputField.Frame.X, this.Frame.Y + __inputField.Frame.Y);
                RectangleF absoluteFrame = new RectangleF(absolutePosition, __inputField.Frame.Size);
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

        public override bool IsFirstResponder
        {
            get
            {
                return __inputField.IsFirstResponder;
            }
        }

        public event RequiresModalControllerEventHandler RequiresModalController;

        public LineDrawingView()
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

        private bool __needsContainerRestore = false;
        private RectangleF __containerFrameBeforeAnimation;
        private RectangleF __inputFieldFrameBeforeAnimation;

        public void AdjustToKeyboard(RectangleF keyboardOverlapingFrame)
        {
            float fieldAdjustedYPosition = __inputField.Frame.Y - keyboardOverlapingFrame.Height;

            if (fieldAdjustedYPosition < 0)
            {
                //The space that the field still requires to move (because the container)
                float fieldRemainingMovement = fieldAdjustedYPosition * -1;
                fieldAdjustedYPosition = 0;

                __containerFrameBeforeAnimation = this.Frame;
                __inputFieldFrameBeforeAnimation = __inputField.Frame;
                __needsContainerRestore = true;

                UIView.Animate(DrawingViewConstants.KEYBOARD_ADJUST_ANIMATIONS_DURATION, new NSAction(() =>
                {
                    __inputField.Frame = new RectangleF(__inputField.Frame.X, 0, __inputField.Frame.Width, __inputField.Frame.Height);
                    this.Frame = new RectangleF(this.Frame.X, this.Frame.Y - fieldRemainingMovement, this.Frame.Width, this.Frame.Height);
                }));
            }
            else
            {
                UIView.Animate(DrawingViewConstants.KEYBOARD_ADJUST_ANIMATIONS_DURATION, new NSAction(() =>
                {
                    __inputField.Frame = new RectangleF(__inputField.Frame.X, fieldAdjustedYPosition, __inputField.Frame.Width, __inputField.Frame.Height);
                }));
            }
        }

        public void RestoreToHideKeyboard()
        {
            if (__needsContainerRestore)
            {
                UIView.Animate(DrawingViewConstants.KEYBOARD_ADJUST_ANIMATIONS_DURATION, new NSAction(() =>
                {
                    __inputField.Frame = __inputFieldFrameBeforeAnimation;
                    this.Frame = __containerFrameBeforeAnimation;
                }), new NSAction(() =>
                {
                    __needsContainerRestore = false;
                }));
            }
            else
            {
                UIView.Animate(DrawingViewConstants.KEYBOARD_ADJUST_ANIMATIONS_DURATION, new NSAction(() =>
                {
                    RectangleF originalFrame = this.InputFieldProperFrame();
                    __inputField.Frame = originalFrame;
                }));
            }
        }

        public void CommitData()
        {
            __line.Content = __inputField.Text;
        }

        public void DrawingModalControllerAccepted(IDrawingViewModalController modalController)
        {
            //Do nothing
        }

        public void DrawingModalControllerCanceled(IDrawingViewModalController modalController)
        {
            //DO nothing
        }

        private void DrawContent()
        {
            if (__line == null)
                return;

            __inputField.Frame = this.InputFieldProperFrame();

            __inputField.TextAlignment = DrawingUtils.ConvertToUITextAlignment(__line.Alignment);
            __inputField.Font = DrawingUtils.CreateFont(__line.FontFamily, __line.FontSize, __line.IsBold);
            __inputField.TextColor = DrawingUtils.CreateColor(__line.Color);

            __inputField.Text = __line.Content;
        }

        private void CreateUIControls()
        {
            __inputField = new UITextField();
            __inputField.Placeholder = DEFAULT_CONTENT;
            __inputField.BorderStyle = UITextBorderStyle.RoundedRect;
            __inputField.VerticalAlignment = UIControlContentVerticalAlignment.Center;

            __inputField.Frame = this.InputFieldProperFrame();
        }

        private void AddUIControls()
        {
            __inputField.Frame = this.InputFieldProperFrame();
            this.Add(__inputField);
        }

        private void UpdateLayout()
        {
            if (!__isLoaded)
                return;

            __inputField.Frame = this.InputFieldProperFrame();
        }

        private RectangleF InputFieldProperFrame()
        {
            if (__line == null)
                return this.InputFieldDefaultFrame();
            else
                return this.InputFieldCustomFrame();
        }

        private RectangleF InputFieldDefaultFrame()
        {
            float yPosition = (this.Frame.Height - FIELD_DEFAULT_HEIGHT) / 2;
            RectangleF frame = new RectangleF(0, yPosition, this.Frame.Width, FIELD_DEFAULT_HEIGHT);

            return frame;
        }

        private RectangleF InputFieldCustomFrame()
        {
            float height = __line.FontSize + FIELD_TOP_MARGIN + FIELD_BOTTOM_MARGIN;
            float yPosition = (this.Frame.Height - height) / 2;
            RectangleF frame = new RectangleF(0, yPosition, this.Frame.Width, height);

            return frame;
        }
    }
}

