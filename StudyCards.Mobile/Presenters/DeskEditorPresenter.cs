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

        public DeskEditorPresenter(IDeskEditorView view, Desk desk)
        {
            if (view == null)
                throw new ArgumentNullException("View can't be null");

            if (desk == null)
                throw new ArgumentNullException("Desk can't be null");

            __view = view;
            __desk = desk;
        }

        public void LoadData()
        {
            __view.Name = __desk.Name;
            __view.CardBackTemplate = __desk.GetCardBackTemplate();
            __view.CardFrontTemplate = __desk.GetCardFrontTemplate();
            __view.CardsBackground = __desk.GetBackground();
        }

        public bool Save()
        {
            __desk.Name = __view.Name;
            __desk.SetCardBackTemplate(__view.CardBackTemplate);
            __desk.SetCardFrontTemplate(__view.CardFrontTemplate);
            __desk.SetBackground(__view.CardsBackground);

            __desk.Save();

            return true;
        }

        public void SetCardBackTemplate(Template template)
        {
            __desk.SetCardBackTemplate(template);
            __view.CardBackTemplate = template;
        }

        public void SetCardFrontTemplate(Template template)
        {
            __desk.SetCardFrontTemplate(template);
            __view.CardFrontTemplate = template;
        }

        public void SetBackground(Background background)
        {
            __desk.SetBackground(background);
            __view.CardsBackground = background;
        }

        public void CommitName()
        {
            __desk.Name = __view.Name;
        }
    }
}

