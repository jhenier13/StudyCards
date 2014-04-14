using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Collections.Generic;
using MonoTouch.Foundation;
using UIComponents.Events;

namespace StudyCards.Iphone
{
    public class DesksTableSource : UITableViewSource
    {
        private IList<Desk> __items;
        private string __cellIdentifier = "DeskCell";

        public event DeleteRowEventHandler RowDeleted;
        public event SelectRowEventHandler RowHasBeenSelected;

        public DesksTableSource(IList<Desk> items)
        {
            if (items == null)
                throw new ArgumentNullException("items can't be null");

            __items = items;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return __items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(__cellIdentifier);

            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, __cellIdentifier);

            Desk item = __items[indexPath.Row];
            cell.TextLabel.Text = item.Name;

            return cell;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var handler = this.RowDeleted;

                    if (handler != null)
                        handler(this, new DeleteRowEventArgs(){ DeleteIndex = indexPath.Row });

                    tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Bottom);
                    break;
                default:
                    break;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var handler = this.RowHasBeenSelected;

            if (handler != null)
                handler(this, new SelectRowEventArgs(){ SelectedIndex = indexPath.Row });
        }
    }
}

