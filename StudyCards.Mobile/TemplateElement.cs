using System;
using System.Xml;
using StudyCards.Mobile.DrawingElements;
using StudyCards.Mobile.Utils;
using System.Collections.Generic;
using System.Text;

namespace StudyCards.Mobile
{
    public abstract class TemplateElement : IParsableObject
    {
        internal static string POSITION = "Position";
        internal static string SIZE = "Size";
        internal static string ID = "ID";

        public string Id { get; set; }

        public DrawPoint Position { get; set; }

        public DrawSize Size { get; set; }

        public abstract string ElementName { get; }

        public virtual void Parse(string data)
        {
            XmlNode node = XmlUtils.CreateNodeFromData(data);

            string positionValue = XmlUtils.GetAttributeValue(node, POSITION);
            string sizeValue = XmlUtils.GetAttributeValue(node, SIZE);
            string idValue = XmlUtils.GetAttributeValue(node, ID);

            this.Position = DrawPoint.FromString(positionValue);
            this.Size = DrawSize.FromString(sizeValue);
            this.Id = idValue;
        }

        protected virtual XmlDocument GenerateNode()
        {
            XmlDocument document = new XmlDocument();
            XmlNode node = document.CreateNode(XmlNodeType.Element, this.ElementName, null);
            document.AppendChild(node);

            XmlAttribute idAttribute = document.CreateAttribute(ID);
            idAttribute.Value = this.Id;
            node.Attributes.Append(idAttribute);

            XmlAttribute positionAttribute = document.CreateAttribute(POSITION);
            positionAttribute.Value = this.Position.ToString();
            node.Attributes.Append(positionAttribute);

            XmlAttribute sizeAttribute = document.CreateAttribute(SIZE);
            sizeAttribute.Value = this.Size.ToString();
            node.Attributes.Append(sizeAttribute);

            return document;
        }

        public string GenerateString()
        {
            XmlDocument document = this.GenerateNode();
            XmlNode node = document.FirstChild;

            return node.OuterXml;
        }

        public abstract DrawingContent CreateDrawingContent();

        public abstract TemplateElement CloneElement();

        public static TemplateElement CreateInstance(string elementName)
        {
            switch (elementName)
            {
                case LineElement.NODE_NAME:
                    return new LineElement();

                case TextElement.NODE_NAME:
                    return new TextElement();

                case LinkElement.NODE_NAME:
                    return new LinkElement();

                case ImageElement.NODE_NAME:
                    return new ImageElement();
                
                default:
                    break;
            }

            throw new ArgumentException("An instance can't be created for this elementName");
        }

        public static string RepresentToString(IList<TemplateElement> elements)
        {
            StringBuilder completeRepresentation = new StringBuilder();

            foreach (TemplateElement singleElement in elements)
            {
                string elementStr = singleElement.GenerateString();
                completeRepresentation.Append(elementStr);
            }

            return completeRepresentation.ToString();
        }

        public static List<TemplateElement> ParseFromStringRepresentation(string representationStr)
        {
            //This is a hack because the XmlDocument.LoadXml only works with one element
            representationStr = string.Format("<TemplateElements> {0} </TemplateElements>", representationStr);

            List<TemplateElement> elements = new List<TemplateElement>();
            XmlNode rootNode = XmlUtils.CreateNodeFromData(representationStr);

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                TemplateElement newElement = TemplateElement.CreateInstance(node.LocalName);
                newElement.Parse(node.OuterXml);
                elements.Add(newElement);
            }

            return elements;
        }
    }
}

