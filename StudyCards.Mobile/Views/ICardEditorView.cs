using System;
using System.Collections.Generic;

namespace StudyCards.Mobile.Views
{
    public interface ICardEditorView
    {
        Background DeskBackground { get; set; }

        IList<CardRelation> FrontElements { get; set; }

        IList<CardRelation> BackElements{ get; set; }
    }
}

