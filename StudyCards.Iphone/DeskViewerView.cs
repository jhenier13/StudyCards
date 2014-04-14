using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile.Views;
using StudyCards.Iphone.SubViews;
using System.Drawing;

namespace StudyCards.Iphone
{
    public class DeskViewerView : UIViewController, IDeskViewerView
    {
        //Attributes
        private DeskViewerPresenter __presenter;
        private Background __deskBackground;
        private bool __requiresFullScreen;
        //UIControls
        private UIBarButtonItem __options;
        private UIBarButtonItem __add;
        private UIBarButtonItem __ciclic;
        private UIBarButtonItem __previous;
        private UIBarButtonItem __next;
        private UIBarButtonItem __shuffle;
        private UICardView __cardViewer;

        public Background DeskBackground
        {
            get{ return __deskBackground; }
            set
            {
                __deskBackground = value;
                __cardViewer.CardBackground = __deskBackground;
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

            __cardViewer.Frame = new RectangleF(10, 10, this.View.Frame.Width - 20, this.View.Frame.Height - 20);

            __presenter.LoadData();
            this.NavigationController.ToolbarHidden = false;
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

        private void CreateUIControls()
        {
            __options = new UIBarButtonItem();
            __options.Style = UIBarButtonItemStyle.Plain;
            __options.Title = "Options";
            __options.Clicked += this.Options_Click;

            __add = new UIBarButtonItem(UIBarButtonSystemItem.Add);

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

            __cardViewer = new UICardView();
        }

        private void AddUIControls()
        {
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __add, __options };

            UIBarButtonItem __dummyButton1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            UIBarButtonItem __dummyButton2 = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
            __dummyButton2.Width = 30;
            UIBarButtonItem __dummyButton3 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            this.ToolbarItems = new UIBarButtonItem[] { __ciclic, __dummyButton1, __previous, __dummyButton2, __next, __dummyButton3, __shuffle };

            this.Add(__cardViewer);
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

