using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace StudyCards.Iphone
{
    public partial class DeskViewerView
    {
        private void Search_Click(object sender, EventArgs e)
        {
            __presenter.BeginSearchMode();
        }

        private void CancelSearch_Click(object sender, EventArgs e)
        {
            __presenter.EndSearchMode();
        }

        private void SearchBar_TextChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            __presenter.Search(e.SearchText);
        }

        private void Ciclic_TouchDown(object sender, EventArgs e)
        {
            __isCiclicButton.Selected = !__isCiclicButton.Selected;
            __presenter.IsCiclic = __isCiclicButton.Selected;
        }

        private void Shuffle_TouchDown(object sender, EventArgs e)
        {
            __isShuffleButton.Selected = !__isShuffleButton.Selected;
            __presenter.IsShuffle = __isShuffleButton.Selected;
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            int previousIndex = __presenter.PeekPrevious();

            if (previousIndex == -1)
                return;

            float newXOffset = CURRENT_CARDS_OFFSET - CONTROLLER_WIDTH;
            __cardsContainer.SetContentOffset(new PointF(newXOffset, 0), true);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            int nextIndex = __presenter.PeekNext();

            if (nextIndex == -1)
                return;

            float newXOffset = CURRENT_CARDS_OFFSET + CONTROLLER_WIDTH;
            __cardsContainer.SetContentOffset(new PointF(newXOffset, 0), true);
        }

        private void Options_Click(object sender, EventArgs e)
        {
            UIActionSheet optionsSheet = new UIActionSheet();
            optionsSheet.AddButton("Desk settings");
            optionsSheet.AddButton("Add card here");
            optionsSheet.AddButton("Edit current card");
            optionsSheet.AddButton("Delete current card");
            optionsSheet.AddButton("Cancel");
            optionsSheet.CancelButtonIndex = 4;
            optionsSheet.DestructiveButtonIndex = 3;
            optionsSheet.Clicked += this.OptionsSheet_Clicked;

            optionsSheet.ShowInView(this.View);
        }

        private void OptionsSheet_Clicked(object sender, UIButtonEventArgs e)
        {
            switch (e.ButtonIndex)
            {
                case 0:
                    this.NavigationController.ToolbarHidden = true;
                    DeskEditorView deskEditor = new DeskEditorView(__presenter.GetDesk());
                    this.NavigationController.PushViewController(deskEditor, true);
                    break;
                case 1:
                    this.AddCard();
                    break;
                case 2:
                    this.NavigationController.ToolbarHidden = true;
                    CardEditorView editorView = new CardEditorView(__presenter.GetDesk(), __presenter.GetCurrentCard());
                    editorView.DeskBackground = __deskBackgroundImage;
                    this.NavigationController.PushViewController(editorView, true);
                    break;
                case 3:
                    __presenter.RemoveCurrentCard();
                    break;
                default:
                    break;
            }
        }

        private void AddCard_AddTouchDown(object semder, EventArgs e)
        {
            this.AddCard();
        }

        private void CardsContainer_DecelerationEnded(object sender, EventArgs e)
        {
            this.AdjustCardsPositions();
        }

        private void CardsContainer_ScrollAnimationEnded(object sender, EventArgs e)
        {
            this.AdjustCardsPositions();
        }
    }
}

