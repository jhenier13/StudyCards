using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.HelpViews
{
    public class BackgroundsViewSource : UITableViewSource
    {
        private List<Background> __backgrounds;
        private Dictionary<string, List<Background>> __backgroundsByType;
        private string __cellIdentifier = "BackgroundCell";
        private string[] __sections = new string[]{ "Defaults", "Customs" };

        public Background CurrentSelectedBackground { get; set; }

        public event EventHandler RowHasBeenSelected;

        public BackgroundsViewSource()
        {
            __backgrounds = new List<Background>(BackgroundsManager.Backgrounds);
            __backgrounds.Sort(Background.Compare);

            __backgroundsByType = new Dictionary<string, List<Background>>();
            __backgroundsByType.Add(__sections[0], new List<Background>());
            __backgroundsByType.Add(__sections[1], new List<Background>());

            foreach (Background background in __backgrounds)
            {
                if (background.IsDefault)
                    __backgroundsByType[__sections[0]].Add(background);
                else
                    __backgroundsByType[__sections[1]].Add(background);
            }
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (section == 0)
                return __backgroundsByType[__sections[0]].Count;
            else if (section == 1)
                return __backgroundsByType[__sections[1]].Count;
            else
                throw new ArgumentOutOfRangeException("This section doesn't exists");
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return __sections.Length;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            return __sections[section];
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 30;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(__cellIdentifier);

            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, __cellIdentifier);

            List<Background> sectionBackgrounds = __backgroundsByType[__sections[indexPath.Section]];
            Background background = sectionBackgrounds[indexPath.Row];
            cell.TextLabel.Text = background.Name;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            if (this.CurrentSelectedBackground != null)
            {
                if (string.Equals(background.Name, this.CurrentSelectedBackground.Name))
                {
                    __lastSelectedCell = cell;
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                }
            }

            return cell;
        }

        public UITableViewCell __lastSelectedCell;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.CellAt(indexPath);

            if (__lastSelectedCell != null)
                __lastSelectedCell.Accessory = UITableViewCellAccessory.None;

            cell.Accessory = UITableViewCellAccessory.Checkmark;
            __lastSelectedCell = cell;

            List<Background> sectionBackgrounds = __backgroundsByType[__sections[indexPath.Section]];
            this.CurrentSelectedBackground = sectionBackgrounds[indexPath.Row];

            var handler = this.RowHasBeenSelected;

            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

