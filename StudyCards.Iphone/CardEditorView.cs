using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Views;
using StudyCards.Mobile.Presenters;
using System.Collections.Generic;
using UIComponents.Frames;
using StudyCards.Iphone.SubViews;
using System.Drawing;
using MonoTouch.Foundation;

namespace StudyCards.Iphone
{
    public class CardEditorView : UIViewController, ICardEditorView
    {
        //Flags
        private bool __layoutInitialized = false;
        //Attributes
        private CardEditorPresenter __presenter;
        private Background __deskBackground;
        private IList<CardRelation> __frontElements;
        private IList<CardRelation> __backElements;
        //UIControls
        private GridView __innerFrame;
        private UICardEditorView __frontCard;
        private UICardEditorView __backCard;
        private UIButton __selectFront;
        private UIButton __selectBack;
        private UIBarButtonItem __done;
        private UIBarButtonItem __chooseTemplate;
        private UIBarButtonItem __cancel;

        public Background DeskBackground
        { 
            get { return __deskBackground; } 
            set
            {
                if (__deskBackground == value)
                    return;

                __deskBackground = value;
                __frontCard.CardBackground = __deskBackground;
                __backCard.CardBackground = __deskBackground;
            }
        }

        public IList<CardRelation> FrontElements
        {
            get{ return __frontElements; }
            set
            {
                if (__frontElements == value)
                    return;

                __frontElements = value;
                __frontCard.CardElements = __frontElements as List<CardRelation>;
            }
        }

        public IList<CardRelation> BackElements
        {
            get{ return __backElements; }
            set
            {
                if (__backElements == value)
                    return;

                __backElements = value;
                __backCard.CardElements = __backElements as List<CardRelation>;
            }
        }

        public CardEditorView(Desk desk, int index)
        {
            __presenter = new CardEditorPresenter(this, desk, index);
            this.EdgesForExtendedLayout = UIRectEdge.None;
            this.View.BackgroundColor = UIColor.White;
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
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, this.KeyboardUpNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, this.KeyboardDownNotification);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!__layoutInitialized)
            {
                this.UpdateLayout();
                __layoutInitialized = true;
            }

            __presenter.LoadData();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.DismissKeyboard();
        }

        public void UpdateLayout()
        {
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.UpdateChildrenLayout();
        }

        private void CreateGrid()
        {
            __innerFrame = new GridView();
            __innerFrame.AddRowsAndColumns("0.5*;25;25;0.5*", "50;1.0*");
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
        }

        private void CreateUIControls()
        {
            __frontCard = new UICardEditorView();
            __frontCard.ParentController = this;
            __backCard = new UICardEditorView();
            __backCard.ParentController = this;
            __backCard.Hidden = true;

            __selectFront = new UIButton(UIButtonType.System);
            __selectFront.SetTitle("Front", UIControlState.Normal);
            __selectFront.TouchDown += this.SelectFront_TouchDown;
            __selectFront.Selected = true;

            __selectBack = new UIButton(UIButtonType.System);
            __selectBack.SetTitle("Back", UIControlState.Normal);
            __selectBack.TouchDown += this.SelectBack_TouchDown;
            __selectBack.Selected = false;

            __done = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            __done.Clicked += this.Done_Click;

            __cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            __cancel.Clicked += this.Cancel_Click;

            __chooseTemplate = new UIBarButtonItem();
            __chooseTemplate.Title = "Choose template";
            __chooseTemplate.Style = UIBarButtonItemStyle.Plain;
            __chooseTemplate.Clicked += ChooseTemplate_Click;
        }

        private void AddUIControls()
        {
            __innerFrame.AddChild(__selectFront, 1, 0);
            __innerFrame.AddChild(__selectBack, 2, 0);
            __innerFrame.AddChild(__backCard, 0, 1, 4, 1);
            __innerFrame.SetMargin(__backCard, new SubViewThickness(5));
            __innerFrame.AddChild(__frontCard, 0, 1, 4, 1);
            __innerFrame.SetMargin(__frontCard, new SubViewThickness(5));

            this.NavigationItem.HidesBackButton = true;
            this.NavigationItem.LeftBarButtonItem = __cancel;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __done, __chooseTemplate };

            this.Add(__innerFrame);
        }

        private PointF __originalFrontButonPosition;
        private PointF __originalBackButtonPosition;

        private void AdjustSelectButtons(SizeF keyboardSize)
        {
            __originalFrontButonPosition = __selectFront.Frame.Location;
            __originalBackButtonPosition = __selectBack.Frame.Location;

            float selectBackBottom = __selectBack.Frame.Y + __selectBack.Frame.Height;
            float keyboardFrameTop = this.View.Frame.Height - keyboardSize.Height;

            float adjustHeight = (selectBackBottom > keyboardFrameTop) ? (selectBackBottom - keyboardFrameTop) : 0;

            if (adjustHeight == 0)
                return;

            UIView.Animate(0.4, new NSAction(() =>
            {
                __selectFront.Frame = new RectangleF(__selectFront.Frame.X, __selectFront.Frame.Y - adjustHeight, __selectFront.Frame.Width, __selectFront.Frame.Height);
                __selectBack.Frame = new RectangleF(__selectBack.Frame.X, __selectBack.Frame.Y - adjustHeight, __selectBack.Frame.Width, __selectBack.Frame.Height);
            }));
        }

        private void ReturnSelectButtonsToOriginalPlace()
        {
            UIView.Animate(0.4, new NSAction(() =>
            {
                __selectFront.Frame = new RectangleF(__originalFrontButonPosition, __selectFront.Frame.Size);
                __selectBack.Frame = new RectangleF(__originalBackButtonPosition, __selectBack.Frame.Size);
            }));
        }

        private void AdjustCurrentCard(SizeF keyboardSize)
        {
            if (__selectFront.Selected)
                __frontCard.AdjustFirstResponder(keyboardSize);
            else
                __backCard.AdjustFirstResponder(keyboardSize);
        }

        private void RestoreCurrentCard()
        {
            if (__selectFront.Selected)
                __frontCard.RestoreFirstResponder();
            else
                __backCard.RestoreFirstResponder();
        }

        private void DismissKeyboard()
        {
            __frontCard.ResignFirstResponder();
            __backCard.ResignFirstResponder();

            this.RestoreCurrentCard();
        }

        private void KeyboardUpNotification(NSNotification notification)
        {
            RectangleF keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            UIWindow appWindow = UIApplication.SharedApplication.Windows[0];
            UIView mainView = appWindow.RootViewController.View;
            keyboardFrame = mainView.ConvertRectFromView(keyboardFrame, appWindow);

            this.AdjustSelectButtons(keyboardFrame.Size);
            this.AdjustCurrentCard(keyboardFrame.Size);
        }

        private void KeyboardDownNotification(NSNotification notification)
        {
            this.ReturnSelectButtonsToOriginalPlace();
            this.RestoreCurrentCard();
        }

        private void SelectFront_TouchDown(object sender, EventArgs e)
        {
            this.DismissKeyboard();

            if (__selectFront.Selected)
                return;

            __frontCard.Hidden = false;
            __selectFront.Selected = true;
            __backCard.Hidden = true;
            __selectBack.Selected = false;
        }

        private void SelectBack_TouchDown(object sender, EventArgs e)
        {
            this.DismissKeyboard();

            if (__selectBack.Selected)
                return;

            __frontCard.Hidden = true;
            __selectFront.Selected = false;
            __backCard.Hidden = false;
            __selectBack.Selected = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DismissKeyboard();
            this.NavigationController.PopViewControllerAnimated(true);
        }

        private void Done_Click(object sender, EventArgs e)
        {
            this.DismissKeyboard();

            __frontCard.CommitCardViewData();
            __backCard.CommitCardViewData();

            __presenter.Save();

            this.NavigationController.PopViewControllerAnimated(true);
        }

        private void ChooseTemplate_Click(object sender, EventArgs e)
        {
            this.DismissKeyboard();
        }
    }
}

