using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace StudyCards.Mobile.Utils
{
    public static class XmlUtils
    {
        public static XmlNode CreateNode(string file)
        {
            XmlDocument document = new XmlDocument();
            document.Load(file);

            foreach (XmlNode child in document.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.XmlDeclaration)
                    continue;

                return child;
            }

            return null;
        }

        public static XmlNode CreateNodeFromData(string data)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(data);

            return document.DocumentElement;
        }

        public static XmlNode CreateNode(XmlNodeType type, string name)
        {
            XmlDocument document = new XmlDocument();
            XmlNode node = document.CreateNode(type, name, null);

            return node;
        }

        public static XmlAttribute CreateAttribute(string name)
        {
            XmlDocument document = new XmlDocument();
            XmlAttribute attribute = document.CreateAttribute(name);

            return attribute;
        }

        public static IEnumerable<XmlNode> GetNotCommentChildNodes(XmlNode xmlNode)
        {
            return xmlNode.ChildNodes.OfType<XmlElement>();
        }

        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            foreach (XmlAttribute singleAttribute in node.Attributes)
            {
                if (string.Equals(singleAttribute.Name, attributeName, StringComparison.InvariantCultureIgnoreCase))
                    return singleAttribute.Value;
            }

            return null;
        }

        public static XmlNode GetChildNode(XmlNode node, string childName)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment)
                    continue;

                if (string.Equals(childName, child.Name, StringComparison.InvariantCultureIgnoreCase))
                    return child;
            }

            return null;
        }
    }
}

