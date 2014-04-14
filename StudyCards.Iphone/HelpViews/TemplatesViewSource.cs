using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.HelpViews
{
    public class TemplatesViewSource : UITableViewSource
    {
        private List<Template> __templates;
        private Dictionary<string,List<Template>> __templatesByType;
        private string[] __sections = new string[]{ "Defaults", "Customs" };
        private string __cellIdentifier = "TemplateCell";

        public Template CurrentSelectedTemplate { get; set; }

        public event EventHandler RowHasBeenSelected;

        public TemplatesViewSource()
        {
            __templates = new List<Template>(TemplatesManager.Templates);
            __templates.Sort(Template.Compare);

            __templatesByType = new Dictionary<string, List<Template>>();
            __templatesByType.Add(__sections[0], new List<Template>());
            __templatesByType.Add(__sections[1], new List<Template>());

            foreach (var item in __templates)
            {
                if (item.IsDefault)
                    __templatesByType[__sections[0]].Add(item);
                else
                    __templatesByType[__sections[1]].Add(item);
            }
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (section == 0)
                return __templatesByType[__sections[0]].Count;
            else if (section == 1)
                return __templatesByType[__sections[1]].Count;
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

            List<Template> sectionTemplates = __templatesByType[__sections[indexPath.Section]];
            Template template = sectionTemplates[indexPath.Row];
            cell.TextLabel.Text = template.Name;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            if (this.CurrentSelectedTemplate != null)
            {
                if (string.Equals(this.CurrentSelectedTemplate.Name, template.Name))
                {
                    __lastSelectedCell = cell;
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                }
            }

            return cell;
        }

        private UITableViewCell __lastSelectedCell;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.CellAt(indexPath);

            if (__lastSelectedCell != null)
                __lastSelectedCell.Accessory = UITableViewCellAccessory.None;

            cell.Accessory = UITableViewCellAccessory.Checkmark;
            __lastSelectedCell = cell;

            List<Template> sectionTemplates = __templatesByType[__sections[indexPath.Section]];
            this.CurrentSelectedTemplate = sectionTemplates[indexPath.Row];

            var handler = this.RowHasBeenSelected;

            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

