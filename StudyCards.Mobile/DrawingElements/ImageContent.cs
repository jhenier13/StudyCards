using System;
using System.Text;
using System.IO;

namespace StudyCards.Mobile.DrawingElements
{
    public class ImageContent : DrawingContent
    {
        private const string IMAGE_FILE_NAME = "CardImage";
        //When the image was already loaded and is going to be modified
        private string __sourceToken;

        public ImageStretch Stretch { get; set; }

        public float Rotation { get; set; }

        public string Source { get; set; }

        public string Tag { get; set; }

        internal ImageContent()
        {
            this.Stretch = ImageStretch.Fill;
            this.Rotation = 0.0F;
            this.Source = string.Empty;
            this.Tag = string.Empty;
        }

        internal override string GenerateContentValue()
        {
            string valueStr = string.Format("{0}||{1}", this.Source, this.Rotation);

            return valueStr;
        }

        internal override void LoadContentValue(string element)
        {
            string[] content = element.Split(new string []{ "||" }, 2, StringSplitOptions.None);
            this.Source = content[0];
            this.Rotation = float.Parse(content[1]);
        }

        internal override void CommitExtraData(object parameter)
        {
            if (string.IsNullOrEmpty(this.Source))
                return;

            bool isTempImage = this.Source.StartsWith(EnviromentDirectories.IOS_TEMP_DIRECTORY);

            if (!isTempImage)
                return;

            string deskFolder = parameter as string;
            string deskImageDirectoryPath = Path.Combine(ApplicationEnviroment.IMAGES_DIRECTORY, deskFolder);
            DirectoryInfo deskImagesDirectory = new DirectoryInfo(deskImageDirectoryPath);
            deskImagesDirectory.Create();

            int imagesQuantity = deskImagesDirectory.GetFiles("*.png").Length;
            int newImageNumber = imagesQuantity + 1;
            string newImageFileName = string.Format("{0}_{1}.png", IMAGE_FILE_NAME, newImageNumber);
            string destinyFilePath = Path.Combine(deskImageDirectoryPath, newImageFileName);

            File.Move(this.Source, destinyFilePath);
            this.Source = destinyFilePath;
        }
    }
}

