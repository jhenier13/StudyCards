using System;
using System.Xml;
using StudyCards.Mobile.Utils;

namespace StudyCards.Mobile.DrawingElements
{
    public class LinkElement : TemplateElement
    {
        internal const string NODE_NAME = "Link";
        internal static readonly string FONT_SIZE = "FontSize";
        internal static readonly string ALIGNMENT = "Alignment";

        public float FontSize { get; set; }

        public TextAlignment Alignment { get; set; }

        public override string ElementName { get { return NODE_NAME; } }

        public LinkElement()
        {
            this.FontSize = 18;
            this.Alignment = TextAlignment.Center;
        }

        public override void Parse(string data)
        {
            base.Parse(data);
            XmlNode node = XmlUtils.CreateNodeFromData(data);

            string fontSizeValue = XmlUtils.GetAttributeValue(node, FONT_SIZE);
            string alignmentValue = XmlUtils.GetAttributeValue(node, ALIGNMENT);

            this.FontSize = float.Parse(fontSizeValue);
            this.Alignment = TextAlignmentUtils.FromString(alignmentValue);
        }

        protected override XmlDocument GenerateNode()
        {
            XmlDocument document = base.GenerateNode();
            XmlNode node = document.FirstChild;

            XmlAttribute fontSizeAttribute = document.CreateAttribute(FONT_SIZE);
            fontSizeAttribute.Value = this.FontSize.ToString();
            node.Attributes.Append(fontSizeAttribute);

            XmlAttribute alignmentAttribute = document.CreateAttribute(ALIGNMENT);
            alignmentAttribute.Value = this.Alignment.GetStringValue();
            node.Attributes.Append(alignmentAttribute);

            return document;
        }

        public override DrawingContent CreateDrawingContent()
        {
            LinkContent content = new LinkContent();
            content.Alignment = this.Alignment;
            content.FontSize = this.FontSize;

            return content;
        }

        public override TemplateElement CloneElement()
        {
            LinkElement clone = new LinkElement();
            clone.Id = this.Id;
            clone.Position = this.Position;
            clone.Size = this.Size;
            clone.FontSize = this.FontSize;
            clone.Alignment = this.Alignment;

            return clone;
        }
    }
}

