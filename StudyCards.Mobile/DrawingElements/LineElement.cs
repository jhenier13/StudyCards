using System;
using System.Xml;
using StudyCards.Mobile.Utils;
using System.Globalization;

namespace StudyCards.Mobile.DrawingElements
{
    public class LineElement : TemplateElement
    {
        internal const string NODE_NAME = "Line";
        internal static readonly string FONT_FAMILY = "FontFamily";
        internal static readonly string FONT_SIZE = "FontSize";
        internal static readonly string COLOR = "Color";
        internal static readonly string IS_BOLD = "IsBold";
        internal static readonly string ALIGNMENT = "Alignment";

        public string FontFamily{ get; set; }

        public float FontSize { get; set; }

        public string Color { get; set; }

        public bool IsBold { get; set; }

        public TextAlignment Alignment { get; set; }

        public override string ElementName { get { return NODE_NAME; } }

        public LineElement()
        {
            this.FontFamily = "Arial";
            this.FontSize = 20;
            this.Color = "Black";
            this.IsBold = false;
            this.Alignment = TextAlignment.Center;
        }

        public override void Parse(string data)
        {
            base.Parse(data);
            XmlNode node = XmlUtils.CreateNodeFromData(data);

            string fontFamilyValue = XmlUtils.GetAttributeValue(node, FONT_FAMILY);
            string fontSizeValue = XmlUtils.GetAttributeValue(node, FONT_SIZE);
            string colorValue = XmlUtils.GetAttributeValue(node, COLOR);
            string isBoldValue = XmlUtils.GetAttributeValue(node, IS_BOLD);
            string alignmentValue = XmlUtils.GetAttributeValue(node, ALIGNMENT);

            this.FontFamily = fontFamilyValue;
            this.FontSize = float.Parse(fontSizeValue);
            this.Color = colorValue;
            this.IsBold = bool.Parse(isBoldValue);
            this.Alignment = TextAlignmentUtils.FromString(alignmentValue);
        }

        protected override XmlDocument GenerateNode()
        {
            XmlDocument document = base.GenerateNode();
            XmlNode node = document.FirstChild;

            XmlAttribute fontFamilyAttribute = document.CreateAttribute(FONT_FAMILY);
            fontFamilyAttribute.Value = this.FontFamily;
            node.Attributes.Append(fontFamilyAttribute);

            XmlAttribute fontSizeAttribute = document.CreateAttribute(FONT_SIZE);
            fontSizeAttribute.Value = this.FontSize.ToString();
            node.Attributes.Append(fontSizeAttribute);

            XmlAttribute colorAttribute = document.CreateAttribute(COLOR);
            colorAttribute.Value = this.Color;
            node.Attributes.Append(colorAttribute);

            XmlAttribute isBoldAttribute = document.CreateAttribute(IS_BOLD);
            isBoldAttribute.Value = (this.IsBold) ? "true" : "false";
            node.Attributes.Append(isBoldAttribute);

            XmlAttribute alignmentAttribute = document.CreateAttribute(ALIGNMENT);
            alignmentAttribute.Value = this.Alignment.GetStringValue();
            node.Attributes.Append(alignmentAttribute);

            return document;
        }

        public override DrawingContent CreateDrawingContent()
        {
            LineContent content = new LineContent();
            content.FontFamily = this.FontFamily;
            content.FontSize = this.FontSize;
            content.Color = this.Color;
            content.IsBold = this.IsBold;
            content.Alignment = this.Alignment;

            return content;
        }

        public override TemplateElement CloneElement()
        {
            LineElement clone = new LineElement();
            clone.Id = this.Id;
            clone.Position = this.Position;
            clone.Size = this.Size;
            clone.Alignment = this.Alignment;
            clone.Color = this.Color;
            clone.FontFamily = this.FontFamily;
            clone.FontSize = this.FontSize;
            clone.IsBold = this.IsBold;

            return clone;
        }
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }

    public static class TextAlignmentUtils
    {
        public static string GetStringValue(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Left:
                    return "Left";
                case TextAlignment.Center:
                    return "Center";
                case TextAlignment.Right:
                    return "Right";
            }

            throw new ArgumentException("This alignment doesn't have string representation");
        }

        public static TextAlignment FromString(string alignmentStr)
        {
            string lowerCaseAlignment = alignmentStr.ToLower(CultureInfo.InvariantCulture);

            switch (lowerCaseAlignment)
            {
                case "left":
                    return TextAlignment.Left;
                case "center":
                    return TextAlignment.Center;
                case "right":
                    return TextAlignment.Right;
            }

            throw new ArgumentException("The alignment string is not a representation of a TextAlignment");
        }
    }
}

