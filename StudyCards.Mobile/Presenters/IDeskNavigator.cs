using System;
using System.Collections.Generic;
using LobaSoft.Common;
using System.Linq;

namespace StudyCards.Mobile.Presenters
{
    public interface IDeskNavigator
    {
        int CardsCount { get; set; }

        bool IsEmpty { get; }

        bool OnlyOneCard { get; }

        bool CanAdd { get; }

        int CurrentIndex { get; set; }

        int RealIndex { get; set; }

        int MaxIndex { get; }

        bool IsCiclic { get; set; }

        bool IsSearching { get; set; }

        bool CanGoNext();

        bool CanGoPrevious();

        void GoNext();

        void GoPrevious();

        int PeekNextTo(int index);

        int PeekPreviousTo(int index);

        int RealIndexAt(int index);

        void Reset();

        void RemoveIndex(int index);
    }

    public class SequencialNavigator : IDeskNavigator
    {
        private int _cardsCount = -1;
        private int _currentIndex = -1;
        private int _realIndex = -1;
        private bool _isCiclic = false;
        private bool _isSearching = false;

        public bool IsEmpty { get; private set; }

        public bool OnlyOneCard { get; private set; }

        public bool CanAdd { get; private set; }

        public int CardsCount
        { 
            get{ return _cardsCount; }
            set
            {
                if (_cardsCount == value)
                    return;

                _cardsCount = value;
                this.Reset();
            }
        }

        public int CurrentIndex
        { 
            get { return _currentIndex; }
            set
            { 
                if (value < -1 || value > this.MaxIndex)
                    throw new ArgumentException("The value is not valid");

                _currentIndex = value;

                if (_currentIndex == -1 || _currentIndex == this.CardsCount)
                {
                    _realIndex = -1;
                    return;
                }

                _realIndex = _currentIndex;
            }
        }

        public int RealIndex
        { 
            get { return _realIndex; }
            set
            {
                if (value < -1 || value >= this.CardsCount)
                    throw new ArgumentException("The value is not valid");

                _realIndex = value;
                _currentIndex = _realIndex;
            }
        }

        public int MaxIndex { get; private set; }

        public bool IsCiclic
        { 
            get { return _isCiclic; }
            set
            {
                _isCiclic = value;
                this.CanAdd = !_isCiclic && !this.IsSearching;
                this.MaxIndex = (this.IsSearching || _isCiclic) ? this.CardsCount - 1 : this.CardsCount;

                if (this.CurrentIndex > this.MaxIndex)
                {
                    this.CurrentIndex = this.MaxIndex;
                    return;
                }

                if (this.CanAdd && this.CurrentIndex == -1 && this.IsEmpty)
                    this.CurrentIndex = this.MaxIndex;
            }
        }

        public bool IsSearching
        { 
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                this.CanAdd = !this.IsCiclic && !_isSearching;
                this.MaxIndex = (_isSearching || this.IsCiclic) ? this.CardsCount - 1 : this.CardsCount;

                if (this.CurrentIndex > this.MaxIndex)
                {
                    this.CurrentIndex = this.MaxIndex;
                    return;
                }

                if (this.CanAdd && this.CurrentIndex == -1 && this.IsEmpty)
                    this.CurrentIndex = this.MaxIndex;
            }
        }

        public SequencialNavigator(int cardsCount)
        {
            this.CanAdd = true;
            this.CardsCount = cardsCount;
        }

        public bool CanGoNext()
        {
            return (!this.IsCiclic && !this.IsEmpty && this.CurrentIndex < this.MaxIndex) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        public bool CanGoPrevious()
        {
            return (!this.IsCiclic && !this.IsEmpty && this.CurrentIndex > 0) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        public void GoNext()
        {
            if (!this.CanGoNext())
                return;

            int topLimit = this.MaxIndex + 1;
            this.CurrentIndex = (this.CurrentIndex + 1) % topLimit;
        }

        public void GoPrevious()
        {
            if (!this.CanGoPrevious())
                return;

            int previousIndex = this.CurrentIndex - 1;
            this.CurrentIndex = (previousIndex < 0) ? this.MaxIndex : previousIndex;
        }

        public int PeekNextTo(int index)
        {
            if (!this.CanGoNextFrom(index))
                return -1;

            int topLimit = this.MaxIndex + 1;
            int nextIndex = (index + 1) % topLimit;
            return nextIndex;
        }

        public int PeekPreviousTo(int index)
        {
            if (!this.CanGoPreviousFrom(index))
                return -1;

            int previousIndex = index - 1;
            previousIndex = (previousIndex < 0) ? this.MaxIndex : previousIndex;
            return previousIndex;
        }

        public int RealIndexAt(int index)
        {
            if (index < 0 || index >= this.CardsCount)
                throw new ArgumentOutOfRangeException("This index is out of range");

            return index;
        }

        public void Reset()
        {
            this.OnlyOneCard = (this.CardsCount == 1);
            this.IsEmpty = (this.CardsCount == 0);

            if (this.IsEmpty)
                this.CurrentIndex = this.CanAdd ? 0 : -1;
            else
                this.CurrentIndex = 0;

            this.MaxIndex = (this.IsSearching || this.IsCiclic) ? this.CardsCount - 1 : this.CardsCount;
        }

        public void RemoveIndex(int index)
        {
            if (index < 0 || index >= this.CardsCount)
                return;

            int currentIndex = this.CurrentIndex;
            this.CardsCount = this.CardsCount - 1;

            if (currentIndex > this.MaxIndex)
                this.CurrentIndex = this.MaxIndex;
            else
                this.CurrentIndex = this.CurrentIndex;
        }

        private bool CanGoNextFrom(int index)
        {
            return (!this.IsCiclic && !this.IsEmpty && index < this.MaxIndex) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        private bool CanGoPreviousFrom(int index)
        {
            return (!this.IsCiclic && !this.IsEmpty && index > 0) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }
    }

    public class RandomNavigator : IDeskNavigator
    {
        private int[] __randomOrder;
        private int _cardsCount = -1;
        private int _currentIndex = -1;
        private int _realIndex = -1;

        public bool IsEmpty { get; private set; }

        public bool OnlyOneCard { get; private set; }

        public bool CanAdd { get; private set; }

        public int CardsCount
        { 
            get{ return _cardsCount; } 
            set
            {
                if (_cardsCount == value)
                    return;

                _cardsCount = value;
                this.Reset();
            }
        }

        public int CurrentIndex
        { 
            get { return _currentIndex; }
            set
            {
                if (value < -1 || value >= this.CardsCount)
                    throw new ArgumentException("The value is not valid");

                _currentIndex = value;

                if (_currentIndex == -1)
                {
                    this.RealIndex = -1;
                    return;
                }

                this.RealIndex = this.RealIndexAt(_currentIndex);
            }
        }

        public int RealIndex
        { 
            get { return _realIndex; }
            set
            {
                if (_realIndex < -1 || _realIndex >= this.CardsCount)
                    throw new ArgumentException("The value is not valid");

                _realIndex = value;
                int newIndex = this.FindIndex(_realIndex);
                _currentIndex = newIndex;
            }
        }

        public int MaxIndex { get; private set; }

        public bool IsCiclic { get; set; }

        public bool IsSearching { get; set; }

        public RandomNavigator(int cardsCount)
        {
            this.CanAdd = false;
            this.CardsCount = cardsCount;
        }

        public bool CanGoNext()
        {
            return (!this.IsCiclic && !this.IsEmpty && this.CurrentIndex < this.MaxIndex) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        public bool CanGoPrevious()
        {
            return (!this.IsCiclic && !this.IsEmpty && this.CurrentIndex > 0) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        public void GoNext()
        {
            if (!this.CanGoNext())
                return;

            this.CurrentIndex = (this.CurrentIndex + 1) % this.CardsCount;
        }

        public void GoPrevious()
        {
            if (!this.CanGoPrevious())
                return;

            int previousIndex = this.CurrentIndex - 1;
            this.CurrentIndex = (previousIndex < 0) ? this.CardsCount - 1 : previousIndex;
        }

        public int RealIndexAt(int index)
        {
            if (index < 0 || index >= __randomOrder.Length)
                throw new ArgumentOutOfRangeException("This index is out of range");

            return __randomOrder[index];
        }

        public int PeekNextTo(int index)
        {
            if (!this.CanGoNextFrom(index))
                return -1;

            int nextIndex = (index + 1) % this.CardsCount;
            return nextIndex;
        }

        public int PeekPreviousTo(int index)
        {
            if (!this.CanGoPreviousFrom(index))
                return -1;

            int previousIndex = index - 1;
            previousIndex = (previousIndex < 0) ? this.CardsCount - 1 : previousIndex;
            return previousIndex;
        }

        public void Reset()
        {
            this.OnlyOneCard = (this.CardsCount == 1);
            this.IsEmpty = (this.CardsCount == 0);
            __randomOrder = RandomGenerator.GenerateRandomicOrder(this.CardsCount);
            this.CurrentIndex = this.IsEmpty ? -1 : 0;
            this.MaxIndex = this.CardsCount - 1;
        }

        public void ResetWithFirstIndex(int firstIndex)
        {
            this.OnlyOneCard = (this.CardsCount == 1);
            this.IsEmpty = (this.CardsCount == 0);
            __randomOrder = RandomGenerator.GenerateRandomicOrder(this.CardsCount);
            List<int> randomOrderList = new List<int>(__randomOrder);
            int searchIndex = this.FindIndex(firstIndex);

            if (searchIndex > 0)
            {
                randomOrderList.Move(searchIndex, 0);
                __randomOrder = randomOrderList.ToArray();
            }

            this.CurrentIndex = this.IsEmpty ? -1 : 0;
            this.MaxIndex = this.CardsCount - 1;
        }

        public void RemoveIndex(int index)
        {
            if (index < 0 || index >= this.CardsCount)
                return;

            _cardsCount--;
            this.OnlyOneCard = (this.CardsCount == 1);
            this.IsEmpty = (this.CardsCount == 0);
            this.MaxIndex = this.CardsCount - 1;

            int searchIndex = this.FindIndex(index);
            List<int> randomOrderList = new List<int>(__randomOrder);
            randomOrderList.RemoveAt(searchIndex);
            __randomOrder = randomOrderList.ToArray();

            if (this.CurrentIndex > this.MaxIndex)
                this.CurrentIndex = this.CurrentIndex - 1;
            else
                this.CurrentIndex = this.CurrentIndex;
        }

        private int FindIndex(int value)
        {
            for (int i = 0; i < __randomOrder.Length; i++)
            {
                if (__randomOrder[i] == value)
                    return i;
            }

            return -1;
        }

        private bool CanGoNextFrom(int index)
        {
            return (!this.IsCiclic && !this.IsEmpty && index < this.MaxIndex) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }

        private bool CanGoPreviousFrom(int index)
        {
            return (!this.IsCiclic && !this.IsEmpty && index > 0) || (this.IsCiclic && !this.OnlyOneCard && !this.IsEmpty);
        }
    }
}

