using System;
using MonoTouch.UIKit;
using StudyCards.Mobile.Views;
using UIComponents.Frames;
using StudyCards.Mobile.Presenters;
using System.Drawing;

namespace StudyCards.Iphone
{
    public class DeskEditorView : UIViewController, IDeskEditorView
    {
        //Attributes
        private DeskEditorPresenter __presenter;
        //UIControls
        private GridView __innerFrame;
        private UILabel __nameLabel;
        private UILabel __cardFrontTemplateLabel;
        private UILabel __cardBackTemplateLabel;
        private UILabel __backgroundLabel;
        private UITextField __name;
        private UITextField __cardFrontTemplate;
        private UITextField __cardBackTemplate;
        private UITextField __background;
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

        public string CardFrontTemplate
        {
            get
            {
                return __cardFrontTemplate.Text;
            }
            set
            {
                __cardFrontTemplate.Text = value;
            }
        }

        public string CardBackTemplate
        {
            get
            {
                return __cardBackTemplate.Text;
            }
            set
            {
                __cardBackTemplate.Text = value;
            }
        }

        public string Background
        {
            get
            {
                return __background.Text;
            }
            set
            {
                __background.Text = value;
            }
        }

        public DeskEditorView()
        {
            __presenter = new DeskEditorPresenter(this);
            this.View.BackgroundColor = UIColor.White;
            this.NavigationItem.HidesBackButton = true;
            this.EdgesForExtendedLayout = UIRectEdge.None;
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

            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __innerFrame.UpdateChildrenLayout();

            __presenter.LoadData();
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
            __innerFrame.AddRowsAndColumns("30;30;30;30;1.0*", "15;0.35*;0.65*;15");
            __innerFrame.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
        }

        private void CreateUIControls()
        {
            __nameLabel = new UILabel();
            __nameLabel.Text = "Name";

            __cardBackTemplateLabel = new UILabel();
            __cardBackTemplateLabel.Text = "Cards back template";

            __cardFrontTemplateLabel = new UILabel();
            __cardFrontTemplateLabel.Text = "Cards front template";

            __backgroundLabel = new UILabel();
            __backgroundLabel.Text = "Background";

            __name = new UITextField();
            __name.BorderStyle = UITextBorderStyle.RoundedRect;

            __cardBackTemplate = new UITextField();
            __cardBackTemplate.BorderStyle = UITextBorderStyle.RoundedRect;

            __cardFrontTemplate = new UITextField();
            __cardFrontTemplate.BorderStyle = UITextBorderStyle.RoundedRect;

            __background = new UITextField();
            __background.BorderStyle = UITextBorderStyle.RoundedRect;

            __done = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            __done.Clicked += this.Done_Click;
            __cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            __cancel.Clicked += this.Cancel_Click;
        }

        private void AddUIControls()
        {
            __innerFrame.AddChild(__nameLabel, 0, 1);
            __innerFrame.AddChild(__cardFrontTemplateLabel, 1, 1);
            __innerFrame.AddChild(__cardBackTemplateLabel, 2, 1);
            __innerFrame.AddChild(__backgroundLabel, 3, 1);
            __innerFrame.AddChild(__name, 0, 2, new SubViewThickness(3));
            __innerFrame.AddChild(__cardFrontTemplate, 1, 2, new SubViewThickness(3));
            __innerFrame.AddChild(__cardBackTemplate, 2, 2, new SubViewThickness(3));
            __innerFrame.AddChild(__background, 3, 2, new SubViewThickness(3));

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
    }
}

