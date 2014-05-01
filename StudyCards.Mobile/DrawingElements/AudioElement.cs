using System;
using System.Xml;

namespace StudyCards.Mobile.DrawingElements
{
    public class AudioElement : TemplateElement
    {
        internal static readonly string NODE_NAME = "Audio";

        public override string ElementName { get { return NODE_NAME; } }

        public AudioElement()
        {
        }

        public override DrawingContent CreateDrawingContent()
        {
            return new AudioContent();
        }

        protected override XmlDocument GenerateNode()
        {
            XmlDocument document = base.GenerateNode();
            return document;
        }

        public override TemplateElement CloneElement()
        {
            AudioElement clone = new AudioElement();
            return clone;
        }
    }
}

