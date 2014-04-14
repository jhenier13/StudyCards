using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Drawing;

namespace StudyCards.Iphone.HelpViews
{
    public class TemplateDialog : UIViewController
    {
        //UIControls
        private UITableView __table;
        private TemplatesViewSource __tableSource;

        public Template SelectedTemplate { get; private set; }

        public event EventHandler SelectionChanged;

        public TemplateDialog(Template currentTemplate)
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;
            this.SelectedTemplate = currentTemplate;
            this.Title = "Template Selection";
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

            __table.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
        }

        private void CreateUIControls()
        {
            __table = new UITableView(new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height), UITableViewStyle.Grouped);
            __table.AllowsMultipleSelection = false;
            __table.AllowsSelection = true;

            __tableSource = new TemplatesViewSource();
            __tableSource.RowHasBeenSelected += this.TableSource_RowHasBeenSelected;
            __tableSource.CurrentSelectedTemplate = this.SelectedTemplate;
            __table.Source = __tableSource;
        }

        private void AddUIControls()
        {
            this.Add(__table);
        }

        private void TableSource_RowHasBeenSelected(object sender, EventArgs e)
        {
            if (string.Equals(this.SelectedTemplate.Name, __tableSource.CurrentSelectedTemplate.Name))
                return;

            this.SelectedTemplate = __tableSource.CurrentSelectedTemplate;
            var handler = this.SelectionChanged;

            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

