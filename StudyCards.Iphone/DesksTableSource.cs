using System;
using System.Collections.Generic;
using LobaSoft.IOS.UIComponents.Events;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using StudyCards.Mobile;

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

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 100.0F;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            DeskTableViewCell cell = tableView.DequeueReusableCell(__cellIdentifier) as DeskTableViewCell;

            if (cell == null)
                cell = new DeskTableViewCell(__cellIdentifier);

            Desk item = __items[indexPath.Row];
            cell.NameLabel.Text = item.Name;

            Background deskBackground = item.GetBackground();
            UIImage deskBackgroundImage = UIImage.FromFile(deskBackground.Location);
            cell.BackgroundContainer.Image = deskBackgroundImage;
            deskBackgroundImage.Dispose();

            return cell;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
                return UITableViewCellEditingStyle.Delete;

            return UITableViewCellEditingStyle.None;
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

