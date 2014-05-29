using System;
using System.Collections.Generic;

namespace StudyCards.Mobile.Views
{
    public interface ICardEditorView
    {
        IList<CardRelation> FrontElements { get; set; }

        IList<CardRelation> BackElements{ get; set; }

        Template CurrentFrontTemplate { get; set; }

        Template CurrentBackTemplate { get; set; }
    }
}

