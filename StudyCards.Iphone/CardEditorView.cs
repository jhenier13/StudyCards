using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Views;
using StudyCards.Mobile.Presenters;
using System.Collections.Generic;
using LobaSoft.IOS.UIComponents.Frames;
using StudyCards.Iphone.SubViews;
using System.Drawing;
using MonoTouch.Foundation;
using LobaSoft.IOS.UIComponents;
using StudyCards.Iphone.HelpViews;

namespace StudyCards.Iphone
{
    public class CardEditorView : UIViewController, ICardEditorView, IDisposableView
    {
        private const string FRONT_TEMPLATE = "Front Template";
        private const string BACK_TEMPLATE = "Back Template";
        private const double TRANSITION_DURATION = 0.6;
        private const double KEYBOARD_ADJUSTMENT_DURATION = 0.4;
        //Attributes
        private CardEditorPresenter __presenter;
        private UIImage __deskBackground;
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
        private UIButton __settings;
        //Helpers UIControls
        private UIView __statusBarBackground;
        //Auxiliars
        private TemplateDialog __frontTemplateDialog;
        private TemplateDialog __backTemplateDialog;

        public UIImage DeskBackground
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

        public Template CurrentFrontTemplate { get; set; }

        public Template CurrentBackTemplate { get; set; }

        public CardEditorView(Desk desk, int index)
        {
            __presenter = new CardEditorPresenter(this, desk, index);
            this.View.BackgroundColor = UIColor.White;
        }

        public CardEditorView(Desk desk, Card card)
        {
            __presenter = new CardEditorPresenter(this, desk, card);
            this.View.BackgroundColor = UIColor.White;
        }

        public void AttachEventHandlers()
        {
            __selectFront.TouchDown += this.SelectFront_TouchDown;
            __selectBack.TouchDown += this.SelectBack_TouchDown;
            __done.Clicked += this.Done_Click;
            __cancel.Clicked += this.Cancel_Click;
            __chooseTemplate.Clicked += this.ChooseTemplate_Click;
            __settings.TouchDown += this.Settings_TouchDown;

            __frontCard.ParentController = this;
            __backCard.ParentController = this;
        }

        public void DetachEventHandlers()
        {
            __selectFront.TouchDown -= this.SelectFront_TouchDown;
            __selectBack.TouchDown -= this.SelectBack_TouchDown;
            __done.Clicked -= this.Done_Click;
            __cancel.Clicked -= this.Cancel_Click;
            __chooseTemplate.Clicked -= this.ChooseTemplate_Click;
            __settings.TouchDown -= this.Settings_TouchDown;

            __frontCard.ParentController = null;
            __backCard.ParentController = null;
        }

        public void CleanSubViews()
        {
        }

        public void AddSubViews()
        {
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
            this.AttachEventHandlers();
            this.UpdateLayout();

            if (__frontTemplateDialog != null)
            {
                __frontTemplateDialog.SelectionChanged -= this.FrontTemplateDialog_SelectionChanged;
                __frontTemplateDialog = null;
            }

            if (__backTemplateDialog != null)
            {
                __backTemplateDialog.SelectionChanged -= this.BackTemplateDialog_SelectionChanged;
                __backTemplateDialog = null;
            }

            __presenter.LoadData();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.DetachEventHandlers();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.DismissKeyboard();
        }

        private void UpdateLayout()
        {
            float navigationBarTotalHeight = IphoneEnviroment.StatusBarHeight + this.NavigationController.NavigationBar.Frame.Height;
            __innerFrame.Frame = new RectangleF(0, navigationBarTotalHeight, this.View.Frame.Width, this.View.Frame.Height - navigationBarTotalHeight);
            __innerFrame.UpdateChildrenLayout();

            __settings.Frame = new RectangleF(5, navigationBarTotalHeight, 35, 35);
            __statusBarBackground.Frame = new RectangleF(0, 0, this.View.Frame.Width, IphoneEnviroment.StatusBarHeight);
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

            __backCard = new UICardEditorView();
            __backCard.Hidden = true;

            __selectFront = new UIButton(UIButtonType.System);
            __selectFront.SetTitle("Front", UIControlState.Normal);
            __selectFront.Selected = true;

            __selectBack = new UIButton(UIButtonType.System);
            __selectBack.SetTitle("Back", UIControlState.Normal);
            __selectBack.Selected = false;

            __done = new UIBarButtonItem(UIBarButtonSystemItem.Done);

            __cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);

            __chooseTemplate = new UIBarButtonItem();
            __chooseTemplate.Title = FRONT_TEMPLATE;
            __chooseTemplate.Style = UIBarButtonItemStyle.Plain;

            __settings = new UIButton(UIButtonType.Custom);
            UIImage settingsEnabledIcon = UIImage.FromFile("SettingsEnabledIcon.png");
            UIImage settingsDisabledIcon = UIImage.FromFile("SettingsDisabledIcon.png");
            __settings.SetImage(settingsDisabledIcon, UIControlState.Normal);
            __settings.SetImage(settingsEnabledIcon, UIControlState.Selected);
            settingsEnabledIcon.Dispose();
            settingsDisabledIcon.Dispose();

            __statusBarBackground = new UIView();
            __statusBarBackground.BackgroundColor = UIColor.FromWhiteAlpha(0.8F, 0.5F);
            __statusBarBackground.Hidden = true;
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
            UIBarButtonItem dummy = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
            dummy.Width = 25;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __done, dummy, __chooseTemplate };

            this.Add(__innerFrame);
            this.Add(__settings);
            this.Add(__statusBarBackground);
        }

        private PointF __originalFrontButonPosition;
        private PointF __originalBackButtonPosition;

        private void AdjustSelectButtons(SizeF keyboardSize)
        {
            __originalFrontButonPosition = __selectFront.Frame.Location;
            __originalBackButtonPosition = __selectBack.Frame.Location;

            float selectBackBottom = __selectBack.Frame.Y + __selectBack.Frame.Height;
            float keyboardFrameTop = __innerFrame.Frame.Height - keyboardSize.Height;

            float adjustHeight = (selectBackBottom > keyboardFrameTop) ? (selectBackBottom - keyboardFrameTop) : 0;

            if (adjustHeight == 0)
                return;

            UIView.Animate(KEYBOARD_ADJUSTMENT_DURATION, new NSAction(() =>
            {
                __selectFront.Frame = new RectangleF(__selectFront.Frame.X, __selectFront.Frame.Y - adjustHeight, __selectFront.Frame.Width, __selectFront.Frame.Height);
                __selectBack.Frame = new RectangleF(__selectBack.Frame.X, __selectBack.Frame.Y - adjustHeight, __selectBack.Frame.Width, __selectBack.Frame.Height);
            }));
        }

        private void ReturnSelectButtonsToOriginalPlace()
        {
            UIView.Animate(KEYBOARD_ADJUSTMENT_DURATION, new NSAction(() =>
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
            __chooseTemplate.Title = FRONT_TEMPLATE;

            UIView.Transition(__backCard, __frontCard, TRANSITION_DURATION, UIViewAnimationOptions.TransitionFlipFromBottom, null);
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
            __chooseTemplate.Title = BACK_TEMPLATE;
            UIView.Transition(__frontCard, __backCard, TRANSITION_DURATION, UIViewAnimationOptions.TransitionFlipFromBottom, null);
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

            __frontCard.CommitCardViewData();
            __backCard.CommitCardViewData();

            if (__selectFront.Selected)
            {
                __frontTemplateDialog = new TemplateDialog(this.CurrentFrontTemplate);
                __frontTemplateDialog.SelectionChanged += this.FrontTemplateDialog_SelectionChanged;
                this.NavigationController.PushViewController(__frontTemplateDialog, true);
            }
            else
            {
                __backTemplateDialog = new TemplateDialog(this.CurrentBackTemplate);
                __backTemplateDialog.SelectionChanged += this.BackTemplateDialog_SelectionChanged;
                this.NavigationController.PushViewController(__backTemplateDialog, true);
            }
        }

        private void Settings_TouchDown(object sender, EventArgs e)
        {
            __settings.Selected = !__settings.Selected;

            if (__settings.Selected)
            {
                this.NavigationController.SetNavigationBarHidden(true, true);
                __statusBarBackground.Hidden = false;
            }
            else
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                __statusBarBackground.Hidden = true;
            }
        }

        private void FrontTemplateDialog_SelectionChanged(object sender, EventArgs e)
        {
            TemplateDialog dialog = sender as TemplateDialog;
            Template selectedTemplate = dialog.SelectedTemplate;
            __presenter.ChangeFrontTemplate(selectedTemplate);
        }

        private void BackTemplateDialog_SelectionChanged(object sender, EventArgs e)
        {
            TemplateDialog dialog = sender as TemplateDialog;
            Template selectedTemplate = dialog.SelectedTemplate;
            __presenter.ChangeBackTemplate(selectedTemplate);
        }
    }
}

