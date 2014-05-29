using System;
using System.Collections.Generic;
using System.Drawing;
using LobaSoft.IOS.UIComponents;
using LobaSoft.IOS.UIComponents.Events;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile.Views;

namespace StudyCards.Iphone
{
    public class DesksView : UIViewController, IDesksView, IDisposableView
    {
        //Flags
        private bool __layoutInitialized = false;
        //Attributes
        private DesksPresenter __presenter;
        //UIControls
        private UITableView __table;
        private UIBarButtonItem __addDesk;
        private UIBarButtonItem __templates;
        private UIBarButtonItem __edit;
        private UIBarButtonItem __editDone;
        //UIControls Extras
        private DesksTableSource __desksSource;
        private IList<Desk> __desks;

        public IList<Desk> Desks
        {
            get
            {
                return __desks;
            }
            set
            {
                if (__desks == value)
                    return;

                if (__desksSource != null)
                    this.DetachDesksSourceEventHandlers();

                __desks = value;
                __desksSource = new DesksTableSource(__desks);
                this.AttachDesksSourceEventHandlers();
                __table.Source = __desksSource;
            }
        }

        public DesksView()
        {
            __presenter = new DesksPresenter(this);
            this.EdgesForExtendedLayout = UIRectEdge.None;
            this.Title = "All Desks";
        }

        public void AttachEventHandlers()
        {
            __addDesk.Clicked += this.AddDesk_Click;
            __templates.Clicked += this.Templates_Click;
            __edit.Clicked += this.Edit_Click;
            __editDone.Clicked += this.EditDone_Click;

            if (__desksSource != null)
                this.AttachDesksSourceEventHandlers();
        }

        public void DetachEventHandlers()
        {
            __addDesk.Clicked -= this.AddDesk_Click;
            __templates.Clicked -= this.Templates_Click;
            __edit.Clicked -= this.Edit_Click;
            __editDone.Clicked -= this.EditDone_Click;

            if (__desksSource != null)
                this.DetachDesksSourceEventHandlers();
        }

        public void CleanSubViews()
        {
        }

        public void AddSubViews()
        {
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
            this.NavigationController.ToolbarHidden = true;
            this.AttachEventHandlers();

            if (!__layoutInitialized)
                this.UpdateLayout();

            __layoutInitialized = true;
            __presenter.LoadData();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.DetachEventHandlers();
        }

        public void UpdateLayout()
        {
            __table.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
        }

        private void CreateUIControls()
        {
            __table = new UITableView();
            __table.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            __table.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            __addDesk = new UIBarButtonItem(UIBarButtonSystemItem.Add);

            __templates = new UIBarButtonItem();
            __templates.Style = UIBarButtonItemStyle.Plain;
            __templates.Title = "Templates";

            __edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit);

            __editDone = new UIBarButtonItem(UIBarButtonSystemItem.Done);
        }

        private void AddUIControls()
        {
            this.Add(__table);
            this.NavigationItem.LeftBarButtonItem = __addDesk;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __edit, __templates };
        }

        private void AttachDesksSourceEventHandlers()
        {
            __desksSource.RowDeleted += this.DesksSource_RowDeleted;
            __desksSource.RowHasBeenSelected += this.DesksSource_RowSelected;
        }

        private void DetachDesksSourceEventHandlers()
        {
            __desksSource.RowDeleted -= this.DesksSource_RowDeleted;
            __desksSource.RowHasBeenSelected -= this.DesksSource_RowSelected;
        }

        private void AddDesk_Click(object sender, EventArgs e)
        {
            DeskEditorView deskEditor = new DeskEditorView();
            this.NavigationController.PushViewController(deskEditor, true);
        }

        private void Templates_Click(object sender, EventArgs e)
        {
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            __table.SetEditing(true, true);

            this.NavigationItem.LeftBarButtonItem = null;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ };
            this.NavigationItem.RightBarButtonItem = __editDone;
        }

        private void EditDone_Click(object sender, EventArgs e)
        {
            __table.SetEditing(false, true);

            this.NavigationItem.LeftBarButtonItem = __addDesk;
            this.NavigationItem.RightBarButtonItem = null;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __edit, __templates };
        }

        private void DesksSource_RowDeleted(object sender, DeleteRowEventArgs e)
        {
            __presenter.DeleteDesk(e.DeleteIndex);
        }

        private void DesksSource_RowSelected(object sender, SelectRowEventArgs e)
        {
            Desk selectedDesk = __desks[e.SelectedIndex];
            DeskViewerView deskViewer = new DeskViewerView(selectedDesk);
            this.NavigationController.PushViewController(deskViewer, true);
        }
    }
}

