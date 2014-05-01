using System;
using System.Drawing;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile.Views;
using UIComponents.Frames;
using StudyCards.Iphone.HelpViews;

namespace StudyCards.Iphone
{
    public class DeskEditorView : UIViewController, IDeskEditorView
    {
        //Flags
        private bool __layoutInitialized = false;
        //Attributes
        private DeskEditorPresenter __presenter;
        private Template __frontTemplate;
        private Template __backTemplate;
        private Background __cardsBackground;
        //UIControls
        private GridView __innerFrame;
        private UILabel __nameLabel;
        private UILabel __cardFrontTemplateLabel;
        private UILabel __cardBackTemplateLabel;
        private UILabel __backgroundLabel;
        private UITextField __name;
        private UIButton __cardFrontTemplate;
        private UIButton __cardBackTemplate;
        private UIButton __background;
        private UIBarButtonItem __done;
        private UIBarButtonItem __cancel;

        public string Name
        {
            get
            {
                return __name.Text;
            }
            set
            {
                __name.Text = value;
            }
        }

        public Template CardFrontTemplate
        {
            get
            {
                return __frontTemplate;
            }
            set
            {
                __frontTemplate = value;
                __cardFrontTemplate.SetTitle(value.Name, UIControlState.Normal);
                __cardFrontTemplate.ImageEdgeInsets = new UIEdgeInsets(2, __cardFrontTemplate.Frame.Width - 20, 2, 0);
            }
        }

        public Template CardBackTemplate
        {
            get
            {
                return __backTemplate;
            }
            set
            {
                __backTemplate = value;
                __cardBackTemplate.SetTitle(value.Name, UIControlState.Normal);
                __cardBackTemplate.ImageEdgeInsets = new UIEdgeInsets(2, __cardBackTemplate.Frame.Width - 20, 2, 0);
            }
        }

        public Background CardsBackground
        {
            get
            {
                return __cardsBackground;
            }
            set
            {
                __cardsBackground = value;
                __background.SetTitle(value.Name, UIControlState.Normal);
                __background.ImageEdgeInsets = new UIEdgeInsets(2, __background.Frame.Width - 20, 2, 0);
            }
        }

        public DeskEditorView()
        {
            __presenter = new DeskEditorPresenter(this);
            this.View.BackgroundColor = UIColor.White;
            this.NavigationItem.HidesBackButton = true;
            this.EdgesForExtendedLayout = UIRectEdge.None;

            this.Title = "Create desk";
        }

        public DeskEditorView(Desk desk)
        {
            __presenter = new DeskEditorPresenter(this, desk);
            this.View.BackgroundColor = UIColor.White;
            this.NavigationItem.HidesBackButton = true;
            this.EdgesForExtendedLayout = UIRectEdge.None;

            this.Title = string.Format("Edit \"{0}\"", desk.Name);
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

            if (!__layoutInitialized)
            {
                this.UpdateLayout();
                __layoutInitialized = true;
            }

            __presenter.LoadData();
        }

        public void UpdateLayout()
        {
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.UpdateChildrenLayout();
        }

        public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            __name.ResignFirstResponder();
            __cardFrontTemplate.ResignFirstResponder();
            __cardBackTemplate.ResignFirstResponder();
            __background.ResignFirstResponder();
        }

        private void CreateGrid()
        {
            __innerFrame = new GridView();
            __innerFrame.AddRowsAndColumns("15;30;30;30;30;1.0*", "15;0.35*;0.65*;15");
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
        }

        private void CreateUIControls()
        {
            __nameLabel = new UILabel();
            __nameLabel.Text = "Name";

            __cardBackTemplateLabel = new UILabel();
            __cardBackTemplateLabel.AdjustsFontSizeToFitWidth = true;
            __cardBackTemplateLabel.Text = "Cards back template";

            __cardFrontTemplateLabel = new UILabel();
            __cardFrontTemplateLabel.AdjustsFontSizeToFitWidth = true;
            __cardFrontTemplateLabel.Text = "Cards front template";

            __backgroundLabel = new UILabel();
            __backgroundLabel.AdjustsFontSizeToFitWidth = true;
            __backgroundLabel.Text = "Background";

            __name = new UITextField();
            __name.Placeholder = "Desk name";
            __name.BorderStyle = UITextBorderStyle.RoundedRect;

            __cardBackTemplate = new UIButton(UIButtonType.DetailDisclosure);
            __cardBackTemplate.SetImage(UIImage.FromFile("DetailDisclosureIcon.png"), UIControlState.Normal);
            __cardBackTemplate.TouchDown += this.CardBackTemplate_TouchDown;

            __cardFrontTemplate = new UIButton(UIButtonType.DetailDisclosure);
            __cardFrontTemplate.SetImage(UIImage.FromFile("DetailDisclosureIcon.png"), UIControlState.Normal);
            __cardFrontTemplate.TouchDown += this.CardFrontTemplate_TouchDown;

            __background = new UIButton(UIButtonType.DetailDisclosure);
            __background.SetImage(UIImage.FromFile("DetailDisclosureIcon.png"), UIControlState.Normal);
            __background.TouchDown += this.Background_TouchDown;

            __done = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            __done.Clicked += this.Done_Click;
            __cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            __cancel.Clicked += this.Cancel_Click;
        }

        private void AddUIControls()
        {
//            __innerFrame.AddChild(__nameLabel, 0, 1);
            __innerFrame.AddChild(__cardFrontTemplateLabel, 2, 1);
            __innerFrame.AddChild(__cardBackTemplateLabel, 3, 1);
            __innerFrame.AddChild(__backgroundLabel, 4, 1);
            __innerFrame.AddChild(__name, 1, 1, 1, 2, true, true, GridHorizontalAlignment.Stretch, GridVerticalAlignment.Stretch, new SubViewThickness(3));
            __innerFrame.AddChild(__cardFrontTemplate, 2, 2, new SubViewThickness(3));
            __innerFrame.AddChild(__cardBackTemplate, 3, 2, new SubViewThickness(3));
            __innerFrame.AddChild(__background, 4, 2, new SubViewThickness(3));

            this.NavigationItem.RightBarButtonItem = __cancel;
            this.NavigationItem.LeftBarButtonItem = __done;
            this.Add(__innerFrame);
        }

        private void Done_Click(object sender, EventArgs e)
        {
            __presenter.Save();
            this.NavigationController.PopViewControllerAnimated(true);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.NavigationController.PopViewControllerAnimated(true);
        }

        private void CardFrontTemplate_TouchDown(object sender, EventArgs e)
        {
            __presenter.CommitName();
            TemplateDialog templateSelection = new TemplateDialog(this.CardFrontTemplate);
            templateSelection.SelectionChanged += this.CardFrontTemplateSelectorDialog_SelectionChanged;
            this.NavigationController.PushViewController(templateSelection, true);
        }

        private void CardBackTemplate_TouchDown(object sender, EventArgs e)
        {
            __presenter.CommitName();
            TemplateDialog templateSelection = new TemplateDialog(this.CardBackTemplate);
            templateSelection.SelectionChanged += this.CardBackTemplateSelectorDialog_SelectionChanged;
            this.NavigationController.PushViewController(templateSelection, true);
        }

        private void Background_TouchDown(object sender, EventArgs e)
        {
            __presenter.CommitName();
            BackgroundDialog backgroundSelection = new BackgroundDialog(this.CardsBackground);
            backgroundSelection.SelectionChanged += this.BackgroundSelectorDialog_SelectionChanged;
            this.NavigationController.PushViewController(backgroundSelection, true);
        }

        private void CardBackTemplateSelectorDialog_SelectionChanged(object sender, EventArgs e)
        {
            TemplateDialog dialog = sender as TemplateDialog;
            __presenter.SetCardBackTemplate(dialog.SelectedTemplate);
        }

        private void CardFrontTemplateSelectorDialog_SelectionChanged(object sender, EventArgs e)
        {
            TemplateDialog dialog = sender as TemplateDialog;
            __presenter.SetCardFrontTemplate(dialog.SelectedTemplate);
        }

        private void BackgroundSelectorDialog_SelectionChanged(object sender, EventArgs e)
        {
            BackgroundDialog dialog = sender as BackgroundDialog;
            __presenter.SetBackground(dialog.SelectedBackground);
        }
    }
}

