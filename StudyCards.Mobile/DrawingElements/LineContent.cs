using System;

namespace StudyCards.Mobile.DrawingElements
{
    public class LineContent : DrawingContent
    {
        public string FontFamily { get; set; }

        public string Color { get; set; }

        public float FontSize { get; set; }

        public bool IsBold { get; set; }

        public TextAlignment Alignment { get; set; }

        public string Content{ get; set; }

        internal LineContent()
        {
            this.FontFamily = "Arial";
            this.Color = "Black";
            this.FontSize = 20.0F;
            this.IsBold = false;
            this.Alignment = TextAlignment.Center;
            this.Content = string.Empty;
        }

        internal override string GenerateContentValue()
        {
            string valueStr = string.Format("{0}", this.Content);

            return valueStr;
        }

        internal override void LoadContentValue(string element)
        {
            this.Content = element;
        }

        internal override bool Search(string searchCriteria)
        {
            string contentLowerCase = this.Content.ToLowerInvariant();
            string search = searchCriteria.ToLowerInvariant();
            return contentLowerCase.Contains(search);
        }
    }
}

