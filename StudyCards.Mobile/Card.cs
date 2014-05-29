using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace StudyCards.Mobile
{
    public partial class Card
    {
        private List<CardRelation> __frontElements;
        private List<CardRelation> __backElements;

        internal int Id { get; set; }

        internal int DeskId { get; set; }

        internal int Index { get; set; }

        public ReadOnlyCollection<CardRelation> FrontElements { get; private set; }

        public ReadOnlyCollection<CardRelation> BackElements { get; private set; }

        public Card()
        {
            __frontElements = new List<CardRelation>();
            __backElements = new List<CardRelation>();

            this.FrontElements = __frontElements.AsReadOnly();
            this.BackElements = __backElements.AsReadOnly();
        }

        public void LoadTemplateInFront(Template template)
        {
            __frontElements.Clear();

            foreach (TemplateElement singleElement in template.Elements)
            {
                TemplateElement newTemplateElement = singleElement.CloneElement();
                DrawingContent content = newTemplateElement.CreateDrawingContent();

                __frontElements.Add(new CardRelation(){ Element = newTemplateElement, Content = content });
            }
        }

        public void LoadTemplateInBack(Template template)
        {
            __backElements.Clear();

            foreach (TemplateElement singleElement in template.Elements)
            {
                TemplateElement newTemplateElement = singleElement.CloneElement();
                DrawingContent content = newTemplateElement.CreateDrawingContent();

                __backElements.Add(new CardRelation(){ Element = newTemplateElement, Content = content });
            }
        }

        public bool Search(string searchCriteria)
        {
            foreach (CardRelation frontRelation in this.FrontElements)
            {
                if (frontRelation.Content.Search(searchCriteria))
                    return true;
            }

            foreach (CardRelation backRelation in this.BackElements)
            {
                if (backRelation.Content.Search(searchCriteria))
                    return true;
            }

            return false;
        }
    }

    internal sealed class CardElement
    {
        internal const string ELEMENTS_SEPARATOR = "<|&&|>";
        internal const string ATTRIBUTES_SEPARATOR = "<[$$]>";

        internal string TemplateElementID { get; set; }

        internal string Value { get; set; }

        internal string RepresentLikeString()
        {
            string stringRepresentation = string.Format("{0}{1}{2}", this.TemplateElementID, ATTRIBUTES_SEPARATOR, this.Value);
            return stringRepresentation;
        }

        internal void FromStringRepresentation(string representation)
        {
            string[] attributes = representation.Split(new string[]{ ATTRIBUTES_SEPARATOR }, StringSplitOptions.None);
            this.TemplateElementID = attributes[0];
            this.Value = attributes[1];
        }

        internal static string RepresentToString(IEnumerable<CardElement> elements)
        {
            StringBuilder elementsValueBuilder = new StringBuilder();

            foreach (CardElement singleElement in elements)
                elementsValueBuilder.AppendFormat("{0}{1}", singleElement.RepresentLikeString(), ELEMENTS_SEPARATOR);

            return elementsValueBuilder.ToString();
        }

        internal static IList<CardElement> ParseStringRepresentation(string cardElements)
        {
            List<CardElement> elements = new List<CardElement>();
            string[] parsableElements = cardElements.Split(new string[]{ ELEMENTS_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parsableElements.Length; i++)
            {
                CardElement newCardElement = new CardElement();
                newCardElement.FromStringRepresentation(parsableElements[i]);
                elements.Add(newCardElement);
            }

            return elements;
        }
    }

    public sealed class CardRelation
    {
        public TemplateElement Element { get; set; }

        public DrawingContent Content { get; set; }
    }
}

