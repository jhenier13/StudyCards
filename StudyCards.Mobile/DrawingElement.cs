using System;
using System.Xml;

namespace StudyCards.Mobile
{
    public abstract class DrawingElement : IParsableObject
    {
        public float AxisXPosition{ get; set; }

        public float AxisYPosition{ get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public virtual void Parse(string data)
        {

        }

        protected virtual XmlNode GenerateNode()
        {
            throw new NotImplementedException();
        }

        public string GenerateString()
        {
            XmlNode node = this.GenerateNode();
            return node.OuterXml;
        }
    }
}

