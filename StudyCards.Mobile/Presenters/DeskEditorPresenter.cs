using System;
using StudyCards.Mobile.Views;

namespace StudyCards.Mobile.Presenters
{
    public class DeskEditorPresenter
    {
        private IDeskEditorView __view;
        private Desk __desk;

        public DeskEditorPresenter(IDeskEditorView view)
        {
            if (view == null)
                throw new ArgumentNullException("View can't be null");

            __view = view;
            __desk = new Desk();
        }

        public void LoadData()
        {
            __view.Name = __desk.Name;
            __view.CardBackTemplate = __desk.CardBackTemplate;
            __view.CardFrontTemplate = __desk.CardFrontTemplate;
            __view.Background = __desk.Background;
        }

        public bool Save()
        {
            __desk.Name = __view.Name;
            __desk.CardBackTemplate = __view.CardBackTemplate;
            __desk.CardFrontTemplate = __view.CardFrontTemplate;
            __desk.Background = __view.Background;

            __desk.Save();

            return true;
        }
    }
}

