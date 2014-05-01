using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.DrawingViews
{
    public class ImageDrawingChoosePhotoController : UIImagePickerController, IDrawingViewModalController
    {
        public IDrawingView DrawingView { get; set; }

        public UIImage SelectedImage { get; private set; }

        public bool ImagePicked { get; private set; }

        public string PhotoType { get; private set; }

        public NSUrl Url { get; private set; }

        public event EventHandler ViewModalAccepted;
        public event EventHandler ViewModalCanceled;

        public ImageDrawingChoosePhotoController()
        {
            this.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            this.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            this.FinishedPickingMedia += (object sender, UIImagePickerMediaPickedEventArgs e) =>
            {
                this.ImagePicked = true;

                this.SelectedImage = e.OriginalImage;
                this.PhotoType = e.MediaType;
                this.Url = e.MediaUrl;

                this.Accept();
            };

            this.Canceled += (object sender, EventArgs e) =>
            {
                this.ImagePicked = false;
                this.Cancel();
            };
        }

        public bool HasNextModalController()
        {
            return true;
        }

        public IDrawingViewModalController NextModalController()
        {
            ImageDrawingViewModalController drawingModalController = new ImageDrawingViewModalController();
            drawingModalController.SelectedImage = this.SelectedImage;
            drawingModalController.DrawingView = this.DrawingView;

            return drawingModalController;
        }

        private void Accept()
        {
            var handler = this.ViewModalAccepted;

            if (handler != null)
                handler(this, new EventArgs());
        }

        private void Cancel()
        {
            var handler = this.ViewModalCanceled;

            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

