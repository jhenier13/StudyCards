using System;

namespace StudyCards.Mobile.Views
{
    public interface IDeskEditorView
    {
        string Name { get; set; }

        Template CardFrontTemplate{ get; set; }

        Template CardBackTemplate{ get; set; }

        Background CardsBackground { get; set; }
    }
}

