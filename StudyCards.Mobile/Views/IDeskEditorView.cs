using System;

namespace StudyCards.Mobile.Views
{
    public interface IDeskEditorView
    {
        string Name { get; set; }

        string CardFrontTemplate{ get; set; }

        string CardBackTemplate{ get; set; }

        string Background { get; set; }
    }
}

