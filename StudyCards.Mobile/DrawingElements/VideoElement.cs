using System;
using System.Xml;

namespace StudyCards.Mobile.DrawingElements
{
    public class VideoElement : TemplateElement
    {
        internal static readonly string NODE_NAME = "Video";

        public override string ElementName { get { return NODE_NAME; } }

        public VideoElement()
        {
        }

        public override DrawingContent CreateDrawingContent()
        {
            VideoContent content = new VideoContent();

            return content;
        }

        protected override XmlDocument GenerateNode()
        {
            XmlDocument document = base.GenerateNode();
            return document;
        }

        public override TemplateElement CloneElement()
        {
            VideoElement clone = new VideoElement();

            return clone;
        }
    }
}

