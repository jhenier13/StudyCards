using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using StudyCards.Mobile.Utils;

namespace StudyCards.Mobile
{
    public class Template
    {
        public const float WIDTH = 480.0F;
        public const float HEIGHT = 320.0F;
        private bool __isLoaded = false;

        public string Name{ get; set; }

        public bool IsDefault { get; internal set; }

        internal string Location { get; set; }

        public List<TemplateElement> Elements{ get; private set; }

        public Template()
        {
            this.Name = string.Empty;
            this.Elements = new List<TemplateElement>();
        }

        public void LoadTemplate()
        {
            if (__isLoaded)
                return;

            XmlNode node = XmlUtils.CreateNode(this.Location);

            if (node == null)
                return;

            XmlNode contentNode = XmlUtils.GetChildNode(node, "Content");

            this.Elements.Clear();

            foreach (XmlNode element in  XmlUtils.GetNotCommentChildNodes(contentNode))
            {
                TemplateElement newDrawingElement = TemplateElement.CreateInstance(element.Name);
                newDrawingElement.Parse(element.OuterXml);

                this.Elements.Add(newDrawingElement);
            }

            __isLoaded = true;
        }

        internal void SaveTemplate()
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

