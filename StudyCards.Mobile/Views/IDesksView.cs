using System;
using System.Collections.Generic;

namespace StudyCards.Mobile.Views
{
    public interface IDesksView
    {
        IList<Desk> Desks { get; set; }
    }
}

