using System;
using StudyCards.Mobile.Views;
using System.Collections.Generic;

namespace StudyCards.Mobile.Presenters
{
    public class DeskViewerPresenter
    {
        private IDeskViewerView __view;
        private Desk __desk;
        private int __currentIndex;

        public DeskViewerPresenter(IDeskViewerView view, Desk desk)
        {
            if (view == null)
                throw new ArgumentNullException("View can't be null");

            __view = view;
            __desk = desk;
            __currentIndex = 0;

            __desk.LoadCards();
        }

        public void LoadData()
        {
            __view.DeskBackground = __desk.GetBackground();
            this.LoadCurrentCard();
            __view.CurrentIndex = __currentIndex;
            __view.TotalCards = __desk.Cards.Count;
        }

        public Desk GetDesk()
        {
            return __desk;
        }

        public int GetCurrentIndex()
        {
            return __currentIndex;
        }

        private void LoadCurrentCard()
        {
            if (__desk.Cards.Count <= __currentIndex)
                return;

            Card currentCard = __desk.Cards[__currentIndex];

            __view.CurrentCardFrontElements = new List<CardRelation>(currentCard.FrontElements);
            __view.CurrentCardBackElements = new List<CardRelation>(currentCard.BackElements);

            __view.DisplayCard();
        }
    }
}

