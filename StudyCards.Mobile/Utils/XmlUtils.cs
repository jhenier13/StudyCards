using System;
using System.Xml;

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

        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            foreach (XmlAttribute singleAttribute in node.Attributes)
            {
                if (string.Equals(singleAttribute.Name, attributeName, StringComparison.InvariantCultureIgnoreCase))
                    return singleAttribute.Value;
            }

            return null;
        }
    }
}

