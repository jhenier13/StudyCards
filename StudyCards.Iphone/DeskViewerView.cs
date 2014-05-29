using System;
using System.Collections.Generic;
using System.Drawing;
using LobaSoft.IOS.UIComponents;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile.Views;
using StudyCards.Iphone.SubViews;

namespace StudyCards.Iphone
{
    public partial class DeskViewerView : UIViewController, IDeskViewerView, IDisposableView
    {
        private const float TOP_MARGIN = 15;
        private const float BOTTOM_MARGIN = 15;
        private const float LEFT_MARGIN = 15;
        private const float RIGHT_MARGIN = 15;
        private const float SEARCHBAR_HEIGHT = 40;
        private const int CARDS_CONTAINER_CAPACITY = 3;
        private float CONTROLLER_WIDTH;
        private float CONTROLLER_HEIGHT;
        private float CURRENT_CARDS_OFFSET = 0.0F;
        //Attributes
        private bool __requiresFullScreen;
        private DeskViewerPresenter __presenter;
        private int _currentIndex;
        private int _totalCards;
        private bool _isSearching;
        private Background _deskBackground;
        //UIControls
        private UIBarButtonItem __options;
        private UIBarButtonItem __search;
        private UIBarButtonItem __cancelSearch;
        private UIButton __isCiclicButton;
        private UIBarButtonItem __ciclic;
        private UIBarButtonItem __previous;
        private UIBarButtonItem __next;
        private UIButton __isShuffleButton;
        private UIBarButtonItem __shuffle;
        private UISearchBar __searchBar;
        //UIControls for cards
        private UIScrollView __cardsContainer;
        private UIView __previousCard;
        private UIView __currentCard;
        private UIView __nextCard;
        //Auxiliars
        private UIImage __deskBackgroundImage;
        //UIGesturesRecognizers
        private UITapGestureRecognizer __tapGesture;

        public Background DeskBackground
        { 
            get { return _deskBackground; }
            set
            {
                if (_deskBackground == value)
                    return;

                if (__deskBackgroundImage != null)
                    __deskBackgroundImage.Dispose();

                _deskBackground = value;
                __deskBackgroundImage = UIImage.FromFile(_deskBackground.Location);
            }
        }

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

        public bool IsSearching
        {
            get{ return _isSearching; }
            set
            {
                _isSearching = value;

                if (_isSearching)
                    this.EnterSearchMode();
                else
                    this.EnterNormalMode();
            }
        }

        public DeskViewerView(Desk desk)
        {
            __presenter = new DeskViewerPresenter(this, desk);
            this.View.BackgroundColor = UIColor.White;
            this.AutomaticallyAdjustsScrollViewInsets = false;
        }

        public void AttachEventHandlers()
        {
            __options.Clicked += this.Options_Click;
            __search.Clicked += this.Search_Click;
            __cancelSearch.Clicked += this.CancelSearch_Click;
            __searchBar.TextChanged += this.SearchBar_TextChanged;
            __isCiclicButton.TouchDown += this.Ciclic_TouchDown;
            __previous.Clicked += this.Previous_Click;
            __next.Clicked += this.Next_Click;
            __isShuffleButton.TouchDown += Shuffle_TouchDown;

            __cardsContainer.DecelerationEnded += this.CardsContainer_DecelerationEnded;
            __cardsContainer.ScrollAnimationEnded += this.CardsContainer_ScrollAnimationEnded;

            this.AttachGesturesRecognizers();
            this.AttachAllCardsEventHandlers();
        }

        public void DetachEventHandlers()
        {
            __options.Clicked -= this.Options_Click;
            __search.Clicked -= this.Search_Click;
            __cancelSearch.Clicked -= this.CancelSearch_Click;
            __searchBar.TextChanged -= this.SearchBar_TextChanged;
            __isCiclicButton.TouchDown -= this.Ciclic_TouchDown;
            __previous.Clicked -= this.Previous_Click;
            __next.Clicked -= this.Next_Click;
            __isShuffleButton.TouchDown -= Shuffle_TouchDown;

            __cardsContainer.DecelerationEnded -= this.CardsContainer_DecelerationEnded;
            __cardsContainer.ScrollAnimationEnded -= this.CardsContainer_ScrollAnimationEnded;

            this.DetachGesturesRecognizers();
            this.DetachAllCardsEventHandlers();
        }

        public void CleanSubViews()
        {
        }

        public void AddSubViews()
        {
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
            this.CreateGesturesRecognizers();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.ToolbarHidden = false;
            //This is a hack, because for some reason the toolbar buttons turns gray after coming back from another ViewController
            //example: without this line if you go to DeskSettings and come back the toolbar buttons are gray
            this.NavigationController.View.TintColor = UIColor.FromRGB(0, 122, 255);

            this.AttachEventHandlers();
            this.UpdateLayout();

            __presenter.LoadData();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.DetachEventHandlers();
        }

        public void LoadCardAt(int index)
        {
            this.CleanCardsContainer();

            if (index == -1)
                return;

            this.LayoutCardsContainer(index);
            this.CreateAllCards(index);

            if (__previousCard != null)
                __cardsContainer.Add(__previousCard);

            if (__currentCard != null)
                __cardsContainer.Add(__currentCard);

            if (__nextCard != null)
                __cardsContainer.Add(__nextCard);

            this.LayoutCards();

            float currentCardXPosition = (__previousCard == null) ? 0 : CONTROLLER_WIDTH;
            __cardsContainer.SetContentOffset(new PointF(currentCardXPosition, 0), false);
            CURRENT_CARDS_OFFSET = currentCardXPosition;
        }

        public void DisplayEmptyDesk()
        {
            __cardsContainer.Hidden = true;
        }

        private void CreateUIControls()
        {
            __cardsContainer = new UIScrollView();
            __cardsContainer.BackgroundColor = UIColor.White;
            __cardsContainer.PagingEnabled = true;
            __cardsContainer.ClipsToBounds = true;
            __cardsContainer.ScrollEnabled = true;
            __cardsContainer.CanCancelContentTouches = true;
            __cardsContainer.ShowsHorizontalScrollIndicator = false;
            __cardsContainer.ShowsVerticalScrollIndicator = false;

            __options = new UIBarButtonItem(UIBarButtonSystemItem.Action);

            __search = new UIBarButtonItem(UIBarButtonSystemItem.Search);

            __cancelSearch = new UIBarButtonItem();
            __cancelSearch.Style = UIBarButtonItemStyle.Plain;
            __cancelSearch.Title = "Cancel search";

            __searchBar = new UISearchBar();
            __searchBar.Hidden = true;
            __searchBar.Frame = new RectangleF(0, 0, 300, 30);
            __searchBar.Placeholder = "Search in desk";

            __isCiclicButton = new UIButton(UIButtonType.System);
            __isCiclicButton.Frame = new RectangleF(0, 0, 80, 30);
            __isCiclicButton.Layer.CornerRadius = 5.0F;
            __isCiclicButton.SetTitle("  Repeat All  ", UIControlState.Normal);
            __isCiclicButton.SizeToFit();
            __ciclic = new UIBarButtonItem(__isCiclicButton);

            __previous = new UIBarButtonItem();
            __previous.Style = UIBarButtonItemStyle.Plain;
            __previous.Title = "Previous";

            __next = new UIBarButtonItem();
            __next.Style = UIBarButtonItemStyle.Plain;
            __next.Title = "Next";

            __isShuffleButton = new UIButton(UIButtonType.System);
            __isShuffleButton.Frame = new RectangleF(0, 0, 90, 30);
            __isShuffleButton.Layer.CornerRadius = 5.0F;
            __isShuffleButton.SetTitle("  Shuffle All  ", UIControlState.Normal);
            __isShuffleButton.SizeToFit();
            __shuffle = new UIBarButtonItem(__isShuffleButton);
        }

        private void AddUIControls()
        {
            this.Add(__cardsContainer);
            this.Add(__searchBar);

            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __options, __search };
            this.AddToolBarItems();
        }

        private UIBarButtonItem [] CreateToolBarItems()
        {
            UIBarButtonItem __dummyButton1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            UIBarButtonItem __dummyButton2 = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
            __dummyButton2.Width = 30;
            UIBarButtonItem __dummyButton3 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            UIBarButtonItem[] items = new UIBarButtonItem[] { __ciclic, __dummyButton1, __previous, __dummyButton2, __next, __dummyButton3, __shuffle };
            return items;
        }

        private void AddToolBarItems()
        {
            this.ToolbarItems = this.CreateToolBarItems();
        }

        private void CreateGesturesRecognizers()
        {
            __tapGesture = new UITapGestureRecognizer(new NSAction(this.ViewTapped));
        }

        private void EnterSearchMode()
        {
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __cancelSearch };
            __searchBar.Hidden = false;
            this.View.BringSubviewToFront(__searchBar);

            UIView.Animate(0.3, new NSAction(() =>
            {
                __searchBar.Frame = new RectangleF(0, IphoneEnviroment.StatusBarHeight + this.NavigationController.NavigationBar.Frame.Height, this.View.Frame.Width, SEARCHBAR_HEIGHT);
            }));
        }

        private void EnterNormalMode()
        {
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __options, __search };

            UIView.Animate(0.3, new NSAction(() =>
            {
                __searchBar.Frame = new RectangleF(0, 0, this.View.Frame.Width, SEARCHBAR_HEIGHT);
            }), new NSAction(() =>
            {
                __searchBar.Hidden = true;
            }));
        }

        private void ChangeToFullScreen()
        {
            __requiresFullScreen = true;
            this.SetNeedsStatusBarAppearanceUpdate();
            this.NavigationController.SetToolbarHidden(true, true);
            this.NavigationController.SetNavigationBarHidden(true, true);

            if (this.IsSearching)
                __searchBar.Hidden = true;
        }

        private void ChangeToNormalScreen()
        {
            __requiresFullScreen = false;
            this.SetNeedsStatusBarAppearanceUpdate();
            this.NavigationController.SetToolbarHidden(false, true);
            this.NavigationController.SetNavigationBarHidden(false, true);

            if (this.IsSearching)
                __searchBar.Hidden = false;
        }
    }
}

