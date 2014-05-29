using System;

namespace StudyCards.Mobile.DrawingElements
{
    public class TextContent : DrawingContent
    {
        public string FontFamily { get; set; }

        public string Color { get; set; }

        public float FontSize { get; set; }

        public string Text { get; set; }

        internal TextContent()
        {
            this.FontFamily = "Arial";
            this.Color = "Black";
            this.FontSize = 15.0F;
            this.Text = string.Empty;
        }

        internal override string GenerateContentValue()
        {
            string valueStr = string.Format("{0}", this.Text);

            return valueStr;
        }

        internal override void LoadContentValue(string element)
        {
            this.Text = element;
        }

        internal override bool Search(string searchCriteria)
        {
            string textLowerCase = this.Text.ToLowerInvariant();
            string search = searchCriteria.ToLowerInvariant();
            return textLowerCase.Contains(search);
        }
    }
}

