using System;
using System.Xml;
using StudyCards.Mobile.Utils;

namespace StudyCards.Mobile.DrawingElements
{
    public class TextElement : TemplateElement
    {
        internal const string NODE_NAME = "Text";
        internal static readonly string FONT_FAMILY = "FontFamily";
        internal static readonly string COLOR = "Color";
        internal static readonly string FONT_SIZE = "FontSize";

        public string FontFamily { get; set; }

        public string Color { get; set; }

        public float FontSize { get; set; }

        public override string ElementName { get { return NODE_NAME; } }

        public TextElement()
        {
            this.FontFamily = "Arial";
            this.Color = "Black";
            this.FontSize = 14;
        }

        public override void Parse(string data)
        {
            base.Parse(data);
            XmlNode node = XmlUtils.CreateNodeFromData(data);

            string fontFamilyValue = XmlUtils.GetAttributeValue(node, FONT_FAMILY);
            string colorValue = XmlUtils.GetAttributeValue(node, COLOR);
            string fontSizeValue = XmlUtils.GetAttributeValue(node, FONT_SIZE);

            this.FontFamily = fontFamilyValue;
            this.Color = colorValue;
            this.FontSize = float.Parse(fontSizeValue);
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

            return document;
        }

        public override DrawingContent CreateDrawingContent()
        {
            TextContent content = new TextContent();
            content.FontFamily = this.FontFamily;
            content.Color = this.Color;
            content.FontSize = this.FontSize;

            return content;
        }

        public override TemplateElement CloneElement()
        {
            TextElement clone = new TextElement();
            clone.Id = this.Id;
            clone.Position = this.Position;
            clone.Size = this.Size;
            clone.FontFamily = this.FontFamily;
            clone.Color = this.Color;
            clone.FontSize = this.FontSize;

            return clone;
        }
    }
}

