using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile.Views;
using StudyCards.Iphone.SubViews;
using System.Drawing;
using System.Collections.Generic;

namespace StudyCards.Iphone
{
    public class DeskViewerView : UIViewController, IDeskViewerView
    {
        private const float TOP_MARGIN = 15;
        private const float BOTTOM_MARGIN = 15;
        private const float LEFT_MARGIN = 15;
        private const float RIGHT_MARGIN = 15;
        //Flags
        private bool __layoutInitialized = false;
        //Attributes
        private bool __requiresFullScreen;
        private DeskViewerPresenter __presenter;
        private int _currentIndex;
        private int _totalCards;
        //UIControls
        //        private CardDisplayView __cardDisplay;
        private UIBarButtonItem __options;
        private UIBarButtonItem __add;
        private UIBarButtonItem __ciclic;
        private UIBarButtonItem __previous;
        private UIBarButtonItem __next;
        private UIBarButtonItem __shuffle;

        public Background DeskBackground{ get; set; }

        public List<CardRelation> CurrentCardFrontElements { get; set; }

        public List<CardRelation> CurrentCardBackElements { get; set; }

        public int CurrentIndex
        { 
            get { return _currentIndex; }
            set
            {
                _currentIndex = value;
                this.DrawCardsCounter();
            }
        }

        public int TotalCards
        { 
            get { return _totalCards; }
            set
            {
                _totalCards = value;
                this.DrawCardsCounter();
            }
        }

        public DeskViewerView(Desk desk)
        {
            __presenter = new DeskViewerPresenter(this, desk);
            this.View.BackgroundColor = UIColor.White;
        }

        public override bool PrefersStatusBarHidden()
        {
            return __requiresFullScreen;
        }

        public override void LoadView()
        {
            base.LoadView();

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

            if (!__layoutInitialized)
            {
                this.UpdateLayout();
                __layoutInitialized = true;
            }

            this.NavigationController.ToolbarHidden = false;

            __presenter.LoadData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController.ToolbarHidden = true;
        }

        public void UpdateLayout()
        {
        }

        public void DisplayCard()
        {
            this.ClearSubViews();

            CardDisplayView displayView = new CardDisplayView();
            displayView.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.View.Frame.Width - LEFT_MARGIN - RIGHT_MARGIN, this.View.Frame.Height - TOP_MARGIN - BOTTOM_MARGIN);

            displayView.CardBackground = this.DeskBackground;
            displayView.FrontCardElements = this.CurrentCardFrontElements;
            displayView.BackCardElements = this.CurrentCardBackElements;

            this.Add(displayView);
        }

        public void DisplayCardLikeNext()
        {
        }

        public void DisplayCardLikePrevious()
        {
        }

        private void DrawCardsCounter()
        {
            if (this.TotalCards == 0)
                this.Title = "None cards";
            else
                this.Title = string.Format("{0} of {1}", this.CurrentIndex + 1, this.TotalCards);
        }

        public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (__requiresFullScreen)
            {
                this.NavigationController.ToolbarHidden = false;
                this.NavigationController.NavigationBarHidden = false;
                this.View.BackgroundColor = UIColor.White;
                __requiresFullScreen = false;
            }
            else
            {
                this.NavigationController.ToolbarHidden = true;
                this.NavigationController.NavigationBarHidden = true;
                this.View.BackgroundColor = UIColor.White;
                __requiresFullScreen = true;
            }

            this.SetNeedsStatusBarAppearanceUpdate();
        }

        private void ClearSubViews()
        {
            foreach (UIView subView in this.View.Subviews)
                subView.RemoveFromSuperview();
        }

        private void CreateUIControls()
        {
            __options = new UIBarButtonItem();
            __options.Style = UIBarButtonItemStyle.Plain;
            __options.Title = "Options";
            __options.Clicked += this.Options_Click;

            __add = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            __add.Clicked += this.Add_Click;

            __ciclic = new UIBarButtonItem();
            __ciclic.Style = UIBarButtonItemStyle.Plain;
            __ciclic.Title = "Ciclic";

            __previous = new UIBarButtonItem();
            __previous.Style = UIBarButtonItemStyle.Plain;
            __previous.Title = "Previous";

            __next = new UIBarButtonItem();
            __next.Style = UIBarButtonItemStyle.Plain;
            __next.Title = "Next";

            __shuffle = new UIBarButtonItem();
            __shuffle.Style = UIBarButtonItemStyle.Plain;
            __shuffle.Title = "Shuffle";
        }

        private void AddUIControls()
        {
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __add, __options };

            UIBarButtonItem __dummyButton1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            UIBarButtonItem __dummyButton2 = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
            __dummyButton2.Width = 30;
            UIBarButtonItem __dummyButton3 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            this.ToolbarItems = new UIBarButtonItem[] { __ciclic, __dummyButton1, __previous, __dummyButton2, __next, __dummyButton3, __shuffle };
        }

        private void Options_Click(object sender, EventArgs e)
        {
            UIActionSheet optionsSheet = new UIActionSheet();
            optionsSheet.AddButton("Desk settings");
            optionsSheet.AddButton("Search");
            optionsSheet.AddButton("Edit card");
            optionsSheet.AddButton("Delete card");
            optionsSheet.AddButton("Cancel");
            optionsSheet.CancelButtonIndex = 4;
            optionsSheet.DestructiveButtonIndex = 3;
            optionsSheet.Clicked += this.OptionsSheet_Clicked;

            optionsSheet.ShowInView(this.View);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            this.NavigationController.ToolbarHidden = true;
            CardEditorView editorView = new CardEditorView(__presenter.GetDesk(), __presenter.GetCurrentIndex());
            this.NavigationController.PushViewController(editorView, true);
        }

        private void OptionsSheet_Clicked(object sender, UIButtonEventArgs e)
        {
            switch (e.ButtonIndex)
            {
                case 0:
                    DeskEditorView deskEditor = new DeskEditorView(__presenter.GetDesk());
                    this.NavigationController.PushViewController(deskEditor, true);
                    break;
                default:
                    break;
            }
        }
    }
}

