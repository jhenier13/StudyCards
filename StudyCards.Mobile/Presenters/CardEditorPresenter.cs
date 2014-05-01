using System;
using StudyCards.Mobile.Views;
using System.Collections.Generic;

namespace StudyCards.Mobile.Presenters
{
    public class CardEditorPresenter
    {
        private bool __isNewCard = false;
        private ICardEditorView __view;
        private Desk __desk;
        private Card __card;
        private int __index;
        private List<CardRelation> __frontElements;
        private List<CardRelation> __backElements;

        public CardEditorPresenter(ICardEditorView view, Desk desk, int index)
        {
            if (view == null)
                throw new ArgumentNullException("The view can't be null");

            if (desk == null)
                throw new ArgumentNullException("The Desk can't be null");

            __view = view;
            __desk = desk;
            __index = index;

            __card = __desk.CreateCard();
            __frontElements = new List<CardRelation>(__card.FrontElements);
            __backElements = new List<CardRelation>(__card.BackElements);

            __isNewCard = true;
        }

        public void LoadData()
        {
            Background background = __desk.GetBackground();
            __view.DeskBackground = background;
            __view.FrontElements = __frontElements;
            __view.BackElements = __backElements;
        }

        public void Save()
        {
            string deskImageFolder = string.Format("{0} ID_{1}", "Desk", __desk.Id);

            foreach (var singleRelation in __card.FrontElements)
                singleRelation.Content.CommitExtraData(deskImageFolder);//This is for the images

            if (__isNewCard)
                __desk.AddCard(__card, __index);
            else
                __card.Save();
        }
    }
}

