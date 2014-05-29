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
        private Template __currentFrontTemplate;
        private Template __currentBackTemplate;

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
            __currentFrontTemplate = __desk.GetCardFrontTemplate();
            __currentBackTemplate = __desk.GetCardBackTemplate();
            __isNewCard = true;
        }

        public CardEditorPresenter(ICardEditorView view, Desk desk, Card card)
        {
            if (view == null)
                throw new ArgumentNullException("The view can't be null");

            if (desk == null)
                throw new ArgumentNullException("The desk can't be null");

            __view = view;
            __desk = desk;
            __card = card;

            __frontElements = new List<CardRelation>(__card.FrontElements);
            __backElements = new List<CardRelation>(__card.BackElements);
            __currentFrontTemplate = __desk.GetCardFrontTemplate();
            __currentBackTemplate = __desk.GetCardBackTemplate();
            __isNewCard = false;
        }

        public void LoadData()
        {
            __view.FrontElements = __frontElements;
            __view.BackElements = __backElements;
            __view.CurrentFrontTemplate = __currentFrontTemplate;
            __view.CurrentBackTemplate = __currentBackTemplate;
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

        public void ChangeFrontTemplate(Template newTemplate)
        {
            __currentFrontTemplate = newTemplate;
            __currentFrontTemplate.LoadTemplate();
            __view.CurrentFrontTemplate = __currentFrontTemplate;
            __card.LoadTemplateInFront(newTemplate);
            __frontElements = new List<CardRelation>(__card.FrontElements);
        }

        public void ChangeBackTemplate(Template newTemplate)
        {
            __currentBackTemplate = newTemplate;
            __currentBackTemplate.LoadTemplate();
            __view.CurrentBackTemplate = __currentBackTemplate;
            __card.LoadTemplateInBack(newTemplate);
            __backElements = new List<CardRelation>(__card.BackElements);
        }
    }
}

