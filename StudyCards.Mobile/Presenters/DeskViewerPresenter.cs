using System;
using StudyCards.Mobile.Views;

namespace StudyCards.Mobile.Presenters
{
    public class DeskViewerPresenter
    {
        private IDeskViewerView __view;
        private Desk __desk;

        public DeskViewerPresenter(IDeskViewerView view, Desk desk)
        {
            if (view == null)
                throw new ArgumentNullException("View can't be null");

            __view = view;
            __desk = desk;
        }

        public void LoadData()
        {
            __view.DeskBackground = __desk.GetBackground();
        }

        public Desk GetDesk()
        {
            return __desk;
        }
    }
}

