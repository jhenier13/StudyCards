using System;
using System.Collections.Generic;

namespace StudyCards.Mobile.Views
{
    public interface IDeskViewerView
    {
        Background DeskBackground { get; set; }

        List<CardRelation> CurrentCardFrontElements { get; set; }

        List<CardRelation> CurrentCardBackElements{ get; set; }

        int CurrentIndex { get; set; }

        int TotalCards { get; set; }

        void DisplayCard();

        void DisplayCardLikeNext();

        void DisplayCardLikePrevious();
    }
}

