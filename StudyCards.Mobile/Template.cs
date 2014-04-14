using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using StudyCards.Mobile.Utils;

namespace StudyCards.Mobile
{
    public class Template : IParsableObject
    {
        public string Name{ get; set; }

        public bool IsDefault { get; internal set; }

        internal string Location { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public List<DrawingElement> Elements{ get; private set; }

        public Template()
        {
            this.Name = string.Empty;
            this.Width = 0;
            this.Height = 0;
            this.Elements = new List<DrawingElement>();
        }

        public void Parse(string data)
        {
            throw new NotImplementedException();
        }

        public string GenerateString()
        {
            throw new NotImplementedException();
        }

        internal void LoadName()
        {
            if (!string.IsNullOrEmpty(this.Name))
                return;

            XmlNode node = XmlUtils.CreateNode(this.Location);

            if (node == null)
                return;

            string templateName = XmlUtils.GetAttributeValue(node, "Name");
            this.Name = templateName;
        }

        public static int Compare(Template templateA, Template templateB)
        {
            return string.Compare(templateA.Name, templateB.Name);
        }
    }
}

