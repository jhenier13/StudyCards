using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace StudyCards.Iphone
{
    public partial class DeskViewerView
    {
        private void UpdateLayout()
        {
            CONTROLLER_WIDTH = this.View.Frame.Width;
            CONTROLLER_HEIGHT = this.View.Frame.Height;
            __searchBar.Frame = new RectangleF(0, 0, CONTROLLER_WIDTH, SEARCHBAR_HEIGHT);

            __cardsContainer.Frame = new RectangleF(0, 0, CONTROLLER_WIDTH, CONTROLLER_HEIGHT);
            __cardsContainer.Bounds = new RectangleF(0, 0, CONTROLLER_WIDTH, CONTROLLER_HEIGHT);
            __cardsContainer.ContentSize = new SizeF(CARDS_CONTAINER_CAPACITY * CONTROLLER_WIDTH, CONTROLLER_HEIGHT);
        }

        private void LayoutCardsContainer(int index)
        {
            int previousIndex = __presenter.PeekPrevious(index);
            int nextIndex = __presenter.PeekNext(index);
            bool hasPrevious = (previousIndex != -1);
            bool hasNext = (nextIndex != -1);

            int currentCoeficient = 1;
            int previousCoeficient = (hasPrevious && !__presenter.HasAddCardBothSides()) ? 1 : 0;
            int nextCoeficient = (hasNext) ? 1 : 0;
            float scrollWidth = CONTROLLER_WIDTH * (currentCoeficient + previousCoeficient + nextCoeficient);

            __cardsContainer.ContentSize = new SizeF(scrollWidth, CONTROLLER_HEIGHT);
        }

        private void LayoutCards()
        {
            if (__previousCard != null)
                __previousCard.Frame = new RectangleF(RIGHT_MARGIN, TOP_MARGIN, CONTROLLER_WIDTH - RIGHT_MARGIN - LEFT_MARGIN, CONTROLLER_HEIGHT - TOP_MARGIN - BOTTOM_MARGIN);

            if (__currentCard != null)
            {
                float currentCardXPosition = (__previousCard == null) ? 0 : CONTROLLER_WIDTH;
                __currentCard.Frame = new RectangleF(currentCardXPosition + RIGHT_MARGIN, TOP_MARGIN, CONTROLLER_WIDTH - RIGHT_MARGIN - LEFT_MARGIN, CONTROLLER_HEIGHT - TOP_MARGIN - BOTTOM_MARGIN);
                __cardsContainer.SetContentOffset(new PointF(currentCardXPosition, 0), false);
            }

            if (__nextCard != null)
            {
                float nextCardXPosition = __cardsContainer.ContentSize.Width - CONTROLLER_WIDTH;
                __nextCard.Frame = new RectangleF(nextCardXPosition + RIGHT_MARGIN, TOP_MARGIN, CONTROLLER_WIDTH - RIGHT_MARGIN - LEFT_MARGIN, CONTROLLER_HEIGHT - TOP_MARGIN - BOTTOM_MARGIN);
            }
        }

        private void AdjustCardsPositions()
        {
            float cardContainerOffset = __cardsContainer.ContentOffset.X;
            int adjustDirection = 0;

            if (cardContainerOffset < CURRENT_CARDS_OFFSET)
                adjustDirection = -1;
            else if (cardContainerOffset > CURRENT_CARDS_OFFSET)
                adjustDirection = 1;

            switch (adjustDirection)
            { 
                case -1:
                    this.AdjustCardsToLeft();
                    break;
                case 1:
                    this.AdjustCardsToRight();
                    break;
            }

            float currentCardXPosition = (__previousCard == null) ? 0 : CONTROLLER_WIDTH;
            __cardsContainer.SetContentOffset(new PointF(currentCardXPosition, 0), false);
            CURRENT_CARDS_OFFSET = currentCardXPosition;
        }

        private void AdjustCardsToLeft()
        {
            //Change PreviousCard to CurrentCard, CurrentCard to NextCard and get a new PreviousCard
            UIView cardToDiscard = __nextCard;
            __nextCard = __currentCard;
            __currentCard = __previousCard;

            this.DetachCardHandlers(cardToDiscard);

            if (cardToDiscard != null)
                cardToDiscard.RemoveFromSuperview();

            __previousCard = null;
            __presenter.GoPrevious(false);
            __previousCard = this.CreatePreviousCardTo(__presenter.CurrentIndex);

            if (__previousCard != null)
                __cardsContainer.Add(__previousCard);

            this.LayoutCardsContainer(__presenter.CurrentIndex);
            this.LayoutCards();
        }

        private void AdjustCardsToRight()
        {
            //Change NextCard to CurrentCard, CurrentCard to PreviousCard and get a newNextCard
            UIView cardToDiscard = __previousCard;
            __previousCard = __currentCard;
            __currentCard = __nextCard;

            this.DetachCardHandlers(cardToDiscard);

            if (cardToDiscard != null)
                cardToDiscard.RemoveFromSuperview();

            __nextCard = null;
            __presenter.GoNext(false);
            __nextCard = this.CreateNextCardTo(__presenter.CurrentIndex);

            if (__nextCard != null)
                __cardsContainer.Add(__nextCard);

            this.LayoutCardsContainer(__presenter.CurrentIndex);
            this.LayoutCards();
        }
    }
}

