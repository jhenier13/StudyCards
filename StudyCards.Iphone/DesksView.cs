﻿using System;
using StudyCards.Mobile.Views;
using MonoTouch.UIKit;
using StudyCards.Mobile.Presenters;
using StudyCards.Mobile;
using System.Collections.Generic;
using System.Drawing;
using UIComponents.Events;

namespace StudyCards.Iphone
{
    public class DesksView : UIViewController, IDesksView
    {
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
                __desks = value;
                __desksSource = new DesksTableSource(__desks);
                __desksSource.RowDeleted += this.DesksSource_RowDeleted;
                __table.Source = __desksSource;
            }
        }

        public DesksView()
        {
            __presenter = new DesksPresenter(this);
            this.View.BackgroundColor = UIColor.Blue;
            this.EdgesForExtendedLayout = UIRectEdge.None;
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
            __presenter.LoadData();
        }

        private void CreateUIControls()
        {
            __table = new UITableView();
            __table.Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height);

            __addDesk = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            __addDesk.Clicked += this.AddDesk_Click;

            __templates = new UIBarButtonItem();
            __templates.Style = UIBarButtonItemStyle.Plain;
            __templates.Title = "Templates";
            __templates.Clicked += this.Templates_Click;

            __edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit);
            __edit.Clicked += this.Edit_Click;

            __editDone = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            __editDone.Clicked += this.EditDone_Click;
        }

        private void AddUIControls()
        {
            this.Add(__table);
            this.NavigationItem.LeftBarButtonItem = __addDesk;
            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[]{ __edit, __templates };
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
    }
}
