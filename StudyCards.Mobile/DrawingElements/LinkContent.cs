using System;

namespace StudyCards.Mobile.DrawingElements
{
    public class LinkContent : DrawingContent
    {
        public float FontSize { get; set; }

        public TextAlignment Alignment { get; set; }

        public string Label { get; set; }

        public string WebLink { get; set; }

        internal LinkContent()
        {
            this.FontSize = 18;
            this.Alignment = TextAlignment.Center;
            this.Label = string.Empty;
            this.WebLink = string.Empty;
        }

        internal override string GenerateContentValue()
        {
            string valueStr = string.Format("{0}||{1}", this.Label, this.WebLink);

            return valueStr;
        }

        internal override void LoadContentValue(string element)
        {
            string[] content = element.Split(new string []{ "||" }, 2, StringSplitOptions.None);
            this.Label = content[0];
            this.WebLink = content[1];
        }

        internal override bool Search(string searchCriteria)
        {
            string labelLowerCase = this.Label.ToLowerInvariant();
            string search = searchCriteria.ToLowerInvariant();
            return labelLowerCase.Contains(search);
        }
    }
}

