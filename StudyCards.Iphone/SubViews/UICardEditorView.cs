using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Iphone.DrawingViews;

namespace StudyCards.Iphone.SubViews
{
    public class UICardEditorView : UIView
    {
        //Flags
        private bool __loaded = false;
        //Atributes
        private Background _cardBackground;
        private List<CardRelation> _cardElements;
        //UIControls
        private UIImageView __backgroundImage;
        private Dictionary<TemplateElement,IDrawingView> __drawingViews;

        public Background CardBackground
        {
            get{ return _cardBackground; }
            set
            {
                if (_cardBackground == value)
                    return;

                _cardBackground = value;

                if (!__loaded)
                    return;

                this.SetImageBackground();
            }
        }

        public List<CardRelation> CardElements
        {
            get { return _cardElements; }
            set
            {
                _cardElements = value;

                if (!__loaded)
                    return;

                this.DrawTemplateElements();
            }
        }

        public UIViewController ParentController{ get; set; }

        public UICardEditorView()
        {
            __drawingViews = new Dictionary<TemplateElement, IDrawingView>();

            this.CreateUIControls();
            this.Layer.CornerRadius = 10.0F;
            this.ClipsToBounds = true;
        }

        public override void LayoutSubviews()
        {
            __backgroundImage.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);

            if (!__loaded)
            {
                this.AddUIControls();
                this.SetImageBackground();
                this.DrawTemplateElements();
            }

            __loaded = true;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.ResignFirstResponder();
        }

        public override bool ResignFirstResponder()
        {
            bool resign = base.ResignFirstResponder();

            foreach (UIView item in this.Subviews)
                item.ResignFirstResponder();

            return resign;
        }

        private IDrawingView __currentResponder;

        public void AdjustFirstResponder(SizeF keyboardSize)
        {
            foreach (UIView item in this.Subviews)
            {
                if (item.IsFirstResponder)
                {
                    __currentResponder = item as IDrawingView;
                    break;
                }
            }

            if (__currentResponder == null)
                return;

            RectangleF utilFrame = __currentResponder.UsedFrame;
            float responderBottom = utilFrame.Y + utilFrame.Height;
            float keyboardTop = this.Frame.Height - keyboardSize.Height;

            float adjustHeight = (responderBottom > keyboardTop) ? responderBottom - keyboardTop : 0;

            if (adjustHeight == 0)
                return;

            RectangleF overlappedFrame = new RectangleF(0, keyboardTop, this.Frame.Width, adjustHeight);
            __currentResponder.AdjustToKeyboard(overlappedFrame);
        }

        public void RestoreFirstResponder()
        {
            if (__currentResponder == null)
                return;

            __currentResponder.RestoreToHideKeyboard();
            __currentResponder = null;
        }

        public void CommitCardViewData()
        {
            foreach (CardRelation singleRelation in this.CardElements)
            {
                TemplateElement element = singleRelation.Element;
                IDrawingView drawingView = __drawingViews[element];
                drawingView.CommitData();
            }
        }

        private void CreateUIControls()
        {
            __backgroundImage = new UIImageView();
        }

        private void AddUIControls()
        {
            this.Add(__backgroundImage);
        }

        private void SetImageBackground()
        {
            if (this.CardBackground == null)
                return;

            UIImage image = UIImage.FromFile(this.CardBackground.Location);
            __backgroundImage.Image = image;
        }

        private void DrawTemplateElements()
        {
            if (this.CardElements == null)
                return;

            __drawingViews.Clear();
            this.ClearSubViews();

            foreach (CardRelation singleRelation in this.CardElements)
            {
                TemplateElement element = singleRelation.Element;
                DrawingContent content = singleRelation.Content;

                IDrawingView elementView = DrawingViewFactory.CreateDrawingView(element);
                elementView.RequiresModalController += this.DrawingView_RequiresModalController;
                elementView.Content = content;
                UIView drawingView = elementView as UIView;
                PointF viewPosition = this.ScalePosition(element.Position.X, element.Position.Y);
                SizeF viewSize = this.ScaleSize(element.Size.Width, element.Size.Height);
                drawingView.Frame = new RectangleF(viewPosition, viewSize);

                this.Add(drawingView);
                __drawingViews.Add(element, elementView);
            }
        }

        private void ClearSubViews()
        {
            UIView[] subViews = this.Subviews;

            for (int i = 0; i < subViews.Length; i++)
            {
                if (__backgroundImage == subViews[i])
                    continue;

                subViews[i].RemoveFromSuperview();
            }
        }

        private PointF ScalePosition(float xPosition, float yPosition)
        {
            float scaledXPosition = xPosition * this.Frame.Width / Template.WIDTH;
            float scaledYPosition = yPosition * this.Frame.Height / Template.HEIGHT;

            return new PointF(scaledXPosition, scaledYPosition);
        }

        private SizeF ScaleSize(float width, float height)
        {
            float scaledWidth = width * this.Frame.Width / Template.WIDTH;
            float scaledHeight = height * this.Frame.Height / Template.HEIGHT;

            return new SizeF(scaledWidth, scaledHeight);
        }

        private void DrawingView_RequiresModalController(object sender, RequiresModalControllerEventArgs e)
        {
            if (this.ParentController == null)
                return;

            if (e.ModalController == null)
                return;

            e.ModalController.DrawingView = sender as IDrawingView;
            e.ModalController.ViewModalAccepted += this.DrawingViewModalController_Accepted;
            e.ModalController.ViewModalCanceled += this.DrawingViewModalController_Canceled;

            UIViewController viewController = e.ModalController as UIViewController;

            this.ParentController.PresentViewController(viewController, true, null);
        }

        private void DrawingViewModalController_Canceled(object sender, EventArgs e)
        {
            IDrawingViewModalController modalController = sender as IDrawingViewModalController;
            modalController.DrawingView.DrawingModalControllerCanceled(modalController);

            this.ParentController.DismissViewController(true, null);
        }

        private void DrawingViewModalController_Accepted(object sender, EventArgs e)
        {
            IDrawingViewModalController modalController = sender as IDrawingViewModalController;

            if (modalController.HasNextModalController())
            {
                IDrawingViewModalController nextModal = modalController.NextModalController();
                nextModal.ViewModalAccepted += this.DrawingViewModalController_Accepted;
                nextModal.ViewModalCanceled += this.DrawingViewModalController_Canceled;

                UIViewController nextViewController = nextModal as UIViewController;

                //Remove the current modal controller
                this.ParentController.DismissViewController(false, new NSAction(() =>
                {
                    //Show the next modal controller when is removed
                    this.ParentController.PresentViewController(nextViewController, true, null);
                }));

                return;
            }

            modalController.DrawingView.DrawingModalControllerAccepted(modalController);

            this.ParentController.DismissViewController(true, null);
        }
    }
}

