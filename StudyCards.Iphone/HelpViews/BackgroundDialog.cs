using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Drawing;

namespace StudyCards.Iphone.HelpViews
{
    public class BackgroundDialog : UIViewController
    {
        //UIControls
        private UITableView __table;
        private BackgroundsViewSource __tableSource;

        public Background SelectedBackground { get; private set; }

        public event EventHandler SelectionChanged;

        public BackgroundDialog(Background currentBackground)
        {
            this.EdgesForExtendedLayout = UIRectEdge.None;
            this.SelectedBackground = currentBackground;
            this.Title = "Background Selection";
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
            __table.AllowsSelection = true;
            __table.AllowsMultipleSelection = false;

            __tableSource = new BackgroundsViewSource();
            __tableSource.RowHasBeenSelected += this.TableSource_RowHasBeenSelected;
            __tableSource.CurrentSelectedBackground = this.SelectedBackground;
            __table.Source = __tableSource;
        }

        private void AddUIControls()
        {
            this.Add(__table);
        }

        private void TableSource_RowHasBeenSelected(object sender, EventArgs e)
        {
            if (string.Equals(this.SelectedBackground.Name, __tableSource.CurrentSelectedBackground))
                return;

            this.SelectedBackground = __tableSource.CurrentSelectedBackground;
            var handler = this.SelectionChanged;

            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

