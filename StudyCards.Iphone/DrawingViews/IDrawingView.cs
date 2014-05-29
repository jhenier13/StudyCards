using System;
using StudyCards.Mobile;
using System.Drawing;
using MonoTouch.UIKit;

namespace StudyCards.Iphone.DrawingViews
{
    public interface IDrawingView
    {
        DrawingContent Content { get; set; }
        //The total space of the view
        RectangleF TotalFrame { get; }
        //Only the used space of the view
        RectangleF UsedFrame { get; }

        event RequiresModalControllerEventHandler RequiresModalController;

        void AdjustToKeyboard(RectangleF keyboardOverlapingFrame);

        void RestoreToHideKeyboard();

        void CommitData();

        void DrawingModalControllerAccepted(IDrawingViewModalController modalController);

        void DrawingModalControllerCanceled(IDrawingViewModalController modalController);
    }
    public delegate void RequiresModalControllerEventHandler(object sender,RequiresModalControllerEventArgs e);
    public sealed class RequiresModalControllerEventArgs : EventArgs
    {
        public IDrawingViewModalController ModalController { get; set; }
    }

    public static class DrawingViewConstants
    {
        public static readonly double KEYBOARD_ADJUST_ANIMATIONS_DURATION = 0.2;
        public static readonly double CARD_FLIP_ANIMATION_DURATION = 0.5;
    }
}

