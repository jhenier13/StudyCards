using System;
using StudyCards.Mobile.Utils;
using System.Xml;
using System.Globalization;

namespace StudyCards.Mobile.DrawingElements
{
    public class ImageElement : TemplateElement
    {
        internal const string NODE_NAME = "Image";
        internal static readonly string STRETCH = "Stretch";
        internal static readonly string ROTATION = "Rotation";

        public ImageStretch Stretch { get; set; }

        public float Rotation { get; set; }

        public override string ElementName { get { return NODE_NAME; } }

        public ImageElement()
        {
            this.Stretch = ImageStretch.Uniform;
            this.Rotation = 0;
        }

        public override void Parse(string data)
        {
            base.Parse(data);
            XmlNode node = XmlUtils.CreateNodeFromData(data);

            string stretchValue = XmlUtils.GetAttributeValue(node, STRETCH);
            string rotationValue = XmlUtils.GetAttributeValue(node, ROTATION);

            this.Rotation = float.Parse(rotationValue);
            this.Stretch = ImageStretchUtils.FromString(stretchValue);
        }

        protected override XmlDocument GenerateNode()
        {
            XmlDocument document = base.GenerateNode();
            XmlNode node = document.FirstChild;

            XmlAttribute stretchAttribute = document.CreateAttribute(STRETCH);
            stretchAttribute.Value = this.Stretch.GetStringValue();
            node.Attributes.Append(stretchAttribute);

            XmlAttribute rotationAttribute = document.CreateAttribute(ROTATION);
            rotationAttribute.Value = this.Rotation.ToString();
            node.Attributes.Append(rotationAttribute);

            return document;
        }

        public override DrawingContent CreateDrawingContent()
        {
            ImageContent content = new ImageContent();
            content.Stretch = this.Stretch;
            content.Rotation = this.Rotation;

            return content;
        }

        public override TemplateElement CloneElement()
        {
            ImageElement clone = new ImageElement();
            clone.Id = this.Id;
            clone.Position = this.Position;
            clone.Size = this.Size;
            clone.Stretch = this.Stretch;
            clone.Rotation = this.Rotation;

            return clone;
        }
    }

    public enum ImageStretch
    {
        None,
        Uniform,
        Fill
    }

    public static class ImageStretchUtils
    {
        public static string GetStringValue(this ImageStretch stretch)
        {
            switch (stretch)
            {
                case ImageStretch.Fill:
                    return "Fill";
                case ImageStretch.Uniform:
                    return "Uniform";
                case ImageStretch.None:
                    return "None";
            }

            throw new ArgumentException("The stretch doesn't have string representation");
        }

        public static ImageStretch FromString(string stretchStr)
        {
            string lowerCaseStretch = stretchStr.ToLower(CultureInfo.InvariantCulture);

            switch (lowerCaseStretch)
            {
                case "fill":
                    return ImageStretch.Fill;
                case "uniform":
                    return ImageStretch.Uniform;
                case "none":
                    return ImageStretch.None;
            }

            throw new ArgumentException("The string doesn't represent an ImageStretch");
        }
    }
}

