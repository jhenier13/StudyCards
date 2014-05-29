using System;
using System.Collections.Generic;

namespace StudyCards.Mobile.Views
{
    public interface IDeskViewerView
    {
        Background DeskBackground { get; set; }

        int CurrentIndex { get; set; }

        int TotalCards { get; set; }

        bool IsSearching { get; set; }

        void DisplayEmptyDesk();

        void LoadCardAt(int index);
    }
}

