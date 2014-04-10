using System;
using StudyCards.Mobile.Views;
using System.Collections.Generic;

namespace StudyCards.Mobile.Presenters
{
    public class DesksPresenter
    {
        private IDesksView __view;
        private List<Desk> __allDesks;

        public DesksPresenter(IDesksView view)
        {
            if (view == null)
                throw new ArgumentNullException("The view can't be null");

            __view = view;
        }

        public void LoadData()
        {
            __allDesks = Desk.GetAllDesks();
            __view.Desks = __allDesks;
        }

        public void DeleteDesk(int index)
        {
            Desk deleteDesk = __allDesks[index];
            deleteDesk.Delete();

            __allDesks.RemoveAt(index);
        }
    }
}

