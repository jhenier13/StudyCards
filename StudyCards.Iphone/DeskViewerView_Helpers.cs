using System;
using StudyCards.Iphone.SubViews;
using System.Collections.Generic;
using StudyCards.Mobile;
using MonoTouch.UIKit;

namespace StudyCards.Iphone
{
    public partial class DeskViewerView
    {
        private void CreateAllCards(int index)
        {
            __previousCard = this.CreatePreviousCardTo(index);
            __currentCard = this.CreateCardFor(index);
            __nextCard = this.CreateNextCardTo(index);
        }

        private UIView CreateCardFor(int index)
        {
            if (index == -1)
                return null;

            UIView cardViewer = null;

            if (index == this.TotalCards)
                cardViewer = this.CreateAddCard();
            else
                cardViewer = this.CreateDisplayCard(index);

            return cardViewer;
        }

        private UIView CreatePreviousCardTo(int index)
        {
            int previousIndex = __presenter.PeekPrevious(index);

            if (previousIndex == -1 || __presenter.HasAddCardBothSides(index) || __presenter.HasSameCardBothSides(index))
                return null;

            UIView cardViewer = null;

            if (previousIndex == this.TotalCards)
                cardViewer = this.CreateAddCard();
            else
                cardViewer = this.CreateDisplayCard(previousIndex);

            return cardViewer;
        }

        private UIView CreateNextCardTo(int index)
        {
            int nextIndex = __presenter.PeekNext(index);

            if (nextIndex == -1)
                return null;

            UIView cardViewer = null;

            if (nextIndex == this.TotalCards)
                cardViewer = this.CreateAddCard();
            else
                cardViewer = this.CreateDisplayCard(nextIndex);

            return cardViewer;
        }

        private UIAddCardView CreateAddCard()
        {
            UIAddCardView addCardView = new UIAddCardView();
            addCardView.CardBackground = __deskBackgroundImage;
            addCardView.AddTouchDown += this.AddCard_AddTouchDown;

            return addCardView;
        }

        private CardDisplayView CreateDisplayCard(int index)
        {
            Card previousCard = __presenter.GetCardAt(index);
            CardDisplayView cardViewer = new CardDisplayView();
            cardViewer.CardBackground = __deskBackgroundImage;
            cardViewer.FrontCardElements = new List<CardRelation>(previousCard.FrontElements);
            cardViewer.BackCardElements = new List<CardRelation>(previousCard.BackElements);

            return cardViewer;
        }

        private void CleanCardsContainer()
        {
            this.DetachCardHandlers(__previousCard);
            this.DetachCardHandlers(__currentCard);
            this.DetachCardHandlers(__nextCard);

            foreach (var card in __cardsContainer.Subviews)
                card.RemoveFromSuperview();

            __previousCard = null;
            __currentCard = null;
            __nextCard = null;
        }

        private void DrawCardsCounter()
        {
            if (this.TotalCards == 0 && this.CurrentIndex == -1)
                this.Title = "None cards";
            else if (this.CurrentIndex == this.TotalCards)
                this.Title = "Add card";
            else
                this.Title = string.Format("{0} of {1}", this.CurrentIndex + 1, this.TotalCards);
        }

        private void AddCard()
        {
            this.ChangeToNormalScreen();
            this.NavigationController.ToolbarHidden = true;
            CardEditorView editorView = new CardEditorView(__presenter.GetDesk(), __presenter.GetRealIndex());
            editorView.DeskBackground = __deskBackgroundImage;
            this.NavigationController.PushViewController(editorView, true);
        }

        private void ViewTapped()
        {
            if (__requiresFullScreen)
                this.ChangeToNormalScreen();
            else
                this.ChangeToFullScreen();
        }
    }
}

