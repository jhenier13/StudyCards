using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using StudyCards.Mobile.DrawingElements;
using System.Drawing;
using System.IO;
using MonoTouch.Foundation;

namespace StudyCards.Iphone.DrawingViews
{
    public class ImageDrawingView : UIView, IDrawingView
    {
        private const float TOP_MARGIN = 3.0F;
        private const float LEFT_MARGIN = 3.0F;
        private const float OPEN_DIALOG_WIDTH = 35.0F;
        private const float OPEN_DIALOG_HEIGHT = 35.0F;
        private const string TEMP_IMAGE_NAME = "Card_Image";
        private string IMAGE_FILE_NAME = string.Empty;
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private string __tag;
        private string __source;
        private ImageContent __image;
        private DrawingContent _content;
        //UIControls
        private UIImageView __imageView;
        private UIButton __openDialog;

        public DrawingContent Content
        {
            get{ return _content; }
            set
            {
                _content = value; 
                __image = (ImageContent)_content;
                __tag = __image.Tag;
                __source = __image.Source;

                if (!__isLoaded)
                    return;

                this.DrawContent();
            }
        }

        public RectangleF TotalFrame
        {
            get{ return this.Frame; }
        }

        public RectangleF UsedFrame
        {
            //The ImageView has the same size that the container, so we return the container frame
            get{ return this.Frame; }
        }

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
                this.UpdateLayout();
            }
        }

        public event RequiresModalControllerEventHandler RequiresModalController;

        public ImageDrawingView()
        {
            this.CreateUIControls();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!__isLoaded)
            {
                this.AddUIControls();
                this.DrawContent();
            }

            __isLoaded = true;
        }

        public void AdjustToKeyboard(RectangleF keyboardOverlapingFrame)
        {
            //Do nothing
        }

        public void RestoreToHideKeyboard()
        {
            //Do nothing
        }

        public void CommitData()
        {
            __image.Tag = __tag;
            __image.Source = __source;
        }

        public void DrawingModalControllerAccepted(IDrawingViewModalController modalController)
        {
            ImageDrawingViewModalController imageModalController = modalController as ImageDrawingViewModalController;

            __tag = imageModalController.Tag;

            UIImage selectedImage = imageModalController.SelectedImage;
            UIImage resizedImage = this.ResizeImage(selectedImage);
            string imageSource = this.SaveTemporaly(resizedImage);
            __source = imageSource;

            UIImage newImage = UIImage.FromFile(imageSource);
            __imageView.Image = newImage;
        }

        public void DrawingModalControllerCanceled(IDrawingViewModalController modalController)
        {
            //Do nothing
        }

        private UIImage ResizeImage(UIImage image)
        {
            //We don't resize for now
            return image;
        }

        private string SaveTemporaly(UIImage image)
        {
            if (string.IsNullOrEmpty(IMAGE_FILE_NAME))
            {
                DirectoryInfo tempDir = new DirectoryInfo(EnviromentDirectories.IOS_TEMP_DIRECTORY);
                FileInfo[] tempFiles = tempDir.GetFiles();
                IMAGE_FILE_NAME = string.Format("{0}({1}).png", TEMP_IMAGE_NAME, tempFiles.Length);
            }

            string tempImagePath = Path.Combine(EnviromentDirectories.IOS_TEMP_DIRECTORY, IMAGE_FILE_NAME);

            NSData imageData = image.AsPNG();
            NSError error = null;
            imageData.Save(tempImagePath, false, out error);

            return tempImagePath;
        }

        private void DrawContent()
        {
            if (__image == null)
                return;

            if (string.IsNullOrEmpty(__image.Source))
                return;

            UIImage imageData = UIImage.FromFile(__image.Source);
            __imageView.Image = imageData;
        }

        private void CreateUIControls()
        {
            __imageView = new UIImageView();
            __imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            __imageView.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);
            __imageView.Layer.BorderWidth = 0.5F;
            __imageView.Layer.BorderColor = UIColor.Blue.CGColor;
            __imageView.Layer.ShadowRadius = 4.0F;
            __imageView.Layer.ShadowColor = UIColor.LightGray.CGColor;

            __openDialog = new UIButton(UIButtonType.Custom);
            UIImage photoIcon = UIImage.FromFile("ImageIcon.png");
            __openDialog.SetImage(photoIcon, UIControlState.Normal);
            __openDialog.TouchDown += this.OpenDialog_TouchDown;
        }

        private void AddUIControls()
        {
            if (__isLoaded)
                return;

            __imageView.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);
            this.Add(__imageView);

            float xPosition = (this.Frame.Width - OPEN_DIALOG_WIDTH) / 2;
            float yPosition = this.Frame.Height - OPEN_DIALOG_HEIGHT - 6;
            __openDialog.Frame = new RectangleF(xPosition, yPosition, OPEN_DIALOG_WIDTH, OPEN_DIALOG_HEIGHT);
            this.Add(__openDialog);
        }

        private void UpdateLayout()
        {
            if (!__isLoaded)
                return;

            __imageView.Frame = new RectangleF(LEFT_MARGIN, TOP_MARGIN, this.Frame.Width - 2 * LEFT_MARGIN, this.Frame.Height - 2 * TOP_MARGIN);

            float xPosition = (this.Frame.Width - OPEN_DIALOG_WIDTH) / 2;
            float yPosition = this.Frame.Height - OPEN_DIALOG_HEIGHT - TOP_MARGIN - 2;
            __openDialog.Frame = new RectangleF(xPosition, yPosition, OPEN_DIALOG_WIDTH, OPEN_DIALOG_HEIGHT);
        }

        private void OpenDialog_TouchDown(object sender, EventArgs e)
        {
            UIActionSheet actions = new UIActionSheet();
            actions.AddButton("Choose existing photo");
            actions.AddButton("Take photo");
            actions.AddButton("Cancel");
            actions.CancelButtonIndex = 2;
            actions.Clicked += this.Actions_Clicked;

            actions.ShowInView(this);
        }

        private void Actions_Clicked(object sender, UIButtonEventArgs e)
        {
            switch (e.ButtonIndex)
            {
                case 0:
                    ImageDrawingChoosePhotoController choosePhoto = new ImageDrawingChoosePhotoController();
                    this.RequiresImageModalController(choosePhoto);
                    break;
                case 1:
                    ImageDrawingChoosePhotoController camera = new ImageDrawingChoosePhotoController();
                    this.RequiresImageModalController(camera);
                    break;
            }
        }

        private void RequiresImageModalController(IDrawingViewModalController controller)
        {
            RequiresModalControllerEventArgs args = new RequiresModalControllerEventArgs();
            args.ModalController = controller;

            this.RequiresModalView(args);
        }

        private void RequiresModalView(RequiresModalControllerEventArgs e)
        {
            var handler = this.RequiresModalController;

            if (handler != null)
                handler(this, e);
        }
    }
}

