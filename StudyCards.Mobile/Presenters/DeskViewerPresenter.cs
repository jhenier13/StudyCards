using System;
using StudyCards.Mobile.Views;
using System.Collections.Generic;

namespace StudyCards.Mobile.Presenters
{
    public class DeskViewerPresenter
    {
        //Flags
        private bool __isSearching;
        private bool __initialized = false;
        //Navigation
        private SequencialNavigator __sequencialNavigator;
        private RandomNavigator __randomNavigator;
        private IDeskNavigator __navigator;
        //Properties
        private IDeskViewerView __view;
        private Desk __desk;
        private bool __isShuffle = false;
        private string __currentSearchCriteria = string.Empty;

        public bool IsCiclic
        { 
            get { return __navigator.IsCiclic; }
            set
            {
                __sequencialNavigator.IsCiclic = value;
                __randomNavigator.IsCiclic = value;

                if (!__initialized)
                    return;

                this.CurrentIndex = __navigator.CurrentIndex;
                __view.CurrentIndex = this.CurrentIndex;
                __view.LoadCardAt(this.CurrentIndex);
            }
        }

        public bool IsShuffle
        { 
            get { return __isShuffle; }
            set
            {
                __isShuffle = value;
                __navigator = (__isShuffle) ? (IDeskNavigator)__randomNavigator : (IDeskNavigator)__sequencialNavigator;

                if (!__initialized)
                    return;

                if (!__navigator.IsEmpty)
                {
                    if (__isShuffle)
                        __randomNavigator.ResetWithFirstIndex(__sequencialNavigator.RealIndex);
                    else
                        __sequencialNavigator.RealIndex = __randomNavigator.RealIndex;
                }

                this.CurrentIndex = __navigator.CurrentIndex;
                __view.CurrentIndex = this.CurrentIndex;
                __view.TotalCards = __desk.Cards.Count;
                __view.LoadCardAt(this.CurrentIndex);
            }
        }

        public bool IsSearching
        {
            get{ return __isSearching; }
        }

        public int CurrentIndex { get; private set; }

        public DeskViewerPresenter(IDeskViewerView view, Desk desk)
        {
            if (view == null)
                throw new ArgumentNullException("View can't be null");

            __view = view;
            __desk = desk;

            __desk.LoadCards();

            __sequencialNavigator = new SequencialNavigator(__desk.Cards.Count);
            __randomNavigator = new RandomNavigator(__desk.Cards.Count);
            this.IsShuffle = false;
            this.IsCiclic = false;

            this.CurrentIndex = __navigator.CurrentIndex;
            __initialized = true;
        }

        public void LoadData()
        {
            __view.DeskBackground = __desk.GetBackground();
            __view.CurrentIndex = this.CurrentIndex;
            __view.TotalCards = __desk.Cards.Count;
            __sequencialNavigator.CardsCount = __desk.Cards.Count;
            __randomNavigator.CardsCount = __desk.Cards.Count;

            __view.LoadCardAt(this.CurrentIndex);
        }

        public void RemoveCurrentCard()
        {
            if (this.CurrentIndex == __desk.Cards.Count)
                return;

            __desk.RemoveCard(__navigator.RealIndex);
            __sequencialNavigator.RemoveIndex(__navigator.RealIndex);
            __randomNavigator.RemoveIndex(__navigator.RealIndex);

            this.CurrentIndex = __navigator.CurrentIndex;
            __view.LoadCardAt(this.CurrentIndex);
            __view.CurrentIndex = this.CurrentIndex;
            __view.TotalCards = __desk.Cards.Count;
        }

        public bool GoNext(bool needsLoading = true)
        {
            if (!__navigator.CanGoNext())
                return false;

            __navigator.GoNext();
            this.CurrentIndex = __navigator.CurrentIndex;
            __view.CurrentIndex = this.CurrentIndex;

            if (needsLoading)
                __view.LoadCardAt(this.CurrentIndex);

            return true;
        }

        public bool GoPrevious(bool needsLoading = true)
        {
            if (!__navigator.CanGoPrevious())
                return false;

            __navigator.GoPrevious();
            this.CurrentIndex = __navigator.CurrentIndex;
            __view.CurrentIndex = this.CurrentIndex;

            if (needsLoading)
                __view.LoadCardAt(this.CurrentIndex);

            return true;
        }

        public void BeginSearchMode()
        {
            __isSearching = true;
            __sequencialNavigator.IsSearching = true;
            __randomNavigator.IsSearching = true;
            __view.IsSearching = true;
        }

        public void Search(string searchCriteria)
        {
            if (!__isSearching)
                return;

            if (string.Equals(searchCriteria, __currentSearchCriteria, StringComparison.InvariantCultureIgnoreCase))
                return;

            __currentSearchCriteria = searchCriteria;
            __desk.Search(searchCriteria);

            __sequencialNavigator.CardsCount = __desk.FilteredCards.Count;
            __randomNavigator.CardsCount = __desk.FilteredCards.Count;
            this.CurrentIndex = __navigator.CurrentIndex;
            __view.CurrentIndex = this.CurrentIndex;
            __view.TotalCards = __desk.FilteredCards.Count;

            if (__desk.FilteredCards.Count == 0)
                __view.DisplayEmptyDesk();
            else
                __view.LoadCardAt(this.CurrentIndex);
        }

        public void EndSearchMode()
        {
            __isSearching = false;
            __sequencialNavigator.IsSearching = true;
            __randomNavigator.IsSearching = true;
            __view.IsSearching = false;

            __sequencialNavigator.CardsCount = __desk.Cards.Count;
            __randomNavigator.CardsCount = __desk.Cards.Count;
            this.CurrentIndex = __navigator.CurrentIndex;
            __view.CurrentIndex = this.CurrentIndex;
            __view.TotalCards = __desk.Cards.Count;

            __view.LoadCardAt(this.CurrentIndex);
        }

        public Card GetCardAt(int index)
        {
            IList<Card> deskCards = (__isSearching) ? __desk.FilteredCards : __desk.Cards;
            int realIndex = __navigator.RealIndexAt(index);
            return deskCards[realIndex];
        }

        public int PeekPrevious(int index)
        {
            return __navigator.PeekPreviousTo(index);
        }

        public int PeekPrevious()
        {
            return __navigator.PeekPreviousTo(this.CurrentIndex);
        }

        public int PeekNext(int index)
        {
            return __navigator.PeekNextTo(index);
        }

        public int PeekNext()
        {
            return __navigator.PeekNextTo(this.CurrentIndex);
        }

        public bool HasAddCardBothSides()
        {
            return this.HasAddCardBothSides(this.CurrentIndex);
        }

        public bool HasAddCardBothSides(int index)
        {
            int previousIndex = __navigator.PeekPreviousTo(index);
            int nextIndex = __navigator.PeekNextTo(index);

            return (previousIndex == __navigator.CardsCount && nextIndex == __navigator.CardsCount);
        }

        public bool HasSameCardBothSides()
        {
            return this.HasSameCardBothSides(this.CurrentIndex);
        }

        public bool HasSameCardBothSides(int index)
        {
            int previousIndex = __navigator.PeekPreviousTo(index);
            int nextIndex = __navigator.PeekNextTo(index);

            return (previousIndex == nextIndex);
        }

        public Desk GetDesk()
        {
            return __desk;
        }

        public int GetRealIndex()
        {
            return __navigator.RealIndex;
        }

        public Card GetCurrentCard()
        {
            return __desk.Cards[__navigator.RealIndex];
        }
    }
}

