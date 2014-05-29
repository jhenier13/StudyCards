using System;
using System.Data;
using StudyCards.Mobile.Persistence;
using System.Text;
using System.Collections.Generic;

namespace StudyCards.Mobile
{
    public partial class Card
    {
        #region DB Fields

        private static readonly string TABLE_NAME = "Cards";
        private static readonly string ID = "CardID";
        private static readonly string MAX_ID = "MaxID";
        private static readonly string DESK_ID = "DeskID";
        private static readonly string INDEX = "CardIndex";
        private static readonly string FRONT_TEMPLATE = "FrontTemplate";
        private static readonly string FRONT_VALUES = "FrontValues";
        private static readonly string BACK_TEMPLATE = "BackTemplate";
        private static readonly string BACK_VALUES = "BackValues";

        #endregion

        public void Save()
        {
            if (this.Id == PersistenceDefaultValues.NO_IDENTIFIED)
                this.Insert();
            else
                this.Update();
        }

        public void SaveIndex()
        {
            string updateIndexQuery = string.Format("UPDATE {0} SET {3}={4} WHERE {1}={2}", TABLE_NAME, ID, this.Id, INDEX, this.Index);
            SQLiteLinker.ExecuteQuery(updateIndexQuery);
        }

        public void Delete()
        {
            if (this.Id == PersistenceDefaultValues.NO_IDENTIFIED)
                return;

            string deleteQuery = string.Format("DELETE FROM {0} WHERE {1}={2}", TABLE_NAME, ID, this.Id);
            SQLiteLinker.ExecuteQuery(deleteQuery);
        }

        internal static List<Card> GetCards(int deskId)
        {
            List<Card> deskCards = new List<Card>();

            string selectQuery = string.Format("SELECT * FROM {0} WHERE {1}={2} ORDER BY {3}", TABLE_NAME, DESK_ID, deskId, INDEX);
            DataTable table = SQLiteLinker.GetDataTable(selectQuery);

            foreach (DataRow row in table.Rows)
            {
                Card newCard = new Card();
                newCard.LoadData(row);
                deskCards.Add(newCard);
            }

            return deskCards;
        }

        private void Insert()
        {
            this.Id = this.GetNewId();
            string frontValuesStr = this.FrontValuesToString();
            string backValuesStr = this.BackValuesToString();
            string frontTemplateElements = this.FrontTemplateElementsToString();
            string backTemplateElements = this.BackTemplateElementsToString();

            string insertQuery = string.Format("INSERT INTO {0}({1},{3},{5},{7},{9},{11},{13}) VALUES({2},{4},{6},'{8}','{10}','{12}','{14}')",
                                     TABLE_NAME, ID, this.Id, DESK_ID, this.DeskId, INDEX, this.Index, FRONT_TEMPLATE, frontTemplateElements,
                                     BACK_TEMPLATE, backTemplateElements, FRONT_VALUES, SQLiteLinker.FixSQLInjection(frontValuesStr), BACK_VALUES, SQLiteLinker.FixSQLInjection(backValuesStr));

            SQLiteLinker.ExecuteQuery(insertQuery);
        }

        private void Update()
        {
            string frontValuesStr = this.FrontValuesToString();
            string backValuesStr = this.BackValuesToString();
            string frontTemplateElements = this.FrontTemplateElementsToString();
            string backTemplateElements = this.BackTemplateElementsToString();

            string updateQuery = string.Format("UPDATE {0} SET {3}={4}, {5}={6}, {7}='{8}', {9}='{10}', {11}='{12}', {13}='{14}' WHERE {1}={2}",
                                     TABLE_NAME, ID, this.Id, DESK_ID, this.DeskId, INDEX, this.Index, FRONT_TEMPLATE, frontTemplateElements,
                                     BACK_TEMPLATE, backTemplateElements, FRONT_VALUES, SQLiteLinker.FixSQLInjection(frontValuesStr), BACK_VALUES, SQLiteLinker.FixSQLInjection(backValuesStr));

            SQLiteLinker.ExecuteQuery(updateQuery);
        }

        private void LoadData(DataRow row)
        {
            this.Id = Convert.ToInt32(row[ID]);
            this.DeskId = Convert.ToInt32(row[DESK_ID]);
            this.Index = Convert.ToInt32(row[INDEX]);

            string frontValuesStr = row[FRONT_VALUES].ToString();
            string backValuesStr = row[BACK_VALUES].ToString();
            string frontTemplateElements = row[FRONT_TEMPLATE].ToString();
            string backTemplateElements = row[BACK_TEMPLATE].ToString();

            this.FromStringToFrontTemplateElements(frontTemplateElements);
            this.FromStringToBackTemplateElements(backTemplateElements);
            this.FromStringToFrontValues(frontValuesStr);
            this.FromStringToBackValues(backValuesStr);
        }

        private string FrontTemplateElementsToString()
        {
            List<TemplateElement> elementsInFrontTemplate = new List<TemplateElement>();

            foreach (CardRelation singleRelation in this.FrontElements)
                elementsInFrontTemplate.Add(singleRelation.Element);

            string elementsInTemplateStr = TemplateElement.RepresentToString(elementsInFrontTemplate);
            return elementsInTemplateStr;
        }

        private void FromStringToFrontTemplateElements(string templateElementsStr)
        {
            List<TemplateElement> frontTemplateElements = TemplateElement.ParseFromStringRepresentation(templateElementsStr);

            foreach (TemplateElement element in frontTemplateElements)
            {
                CardRelation newRelation = new CardRelation();
                DrawingContent content = element.CreateDrawingContent();
                newRelation.Element = element;
                newRelation.Content = content;
                __frontElements.Add(newRelation);
            }
        }

        private string FrontValuesToString()
        {
            List<CardElement> frontElements = new List<CardElement>();

            foreach (CardRelation singleRelation in this.FrontElements)
            {
                CardElement newCardElement = new CardElement();

                TemplateElement element = singleRelation.Element;
                DrawingContent content = singleRelation.Content;

                newCardElement.TemplateElementID = element.Id;
                newCardElement.Value = content.GenerateContentValue();

                frontElements.Add(newCardElement);
            }

            string frontValuesStr = CardElement.RepresentToString(frontElements);
            return frontValuesStr;
        }

        private void FromStringToFrontValues(string frontContentStr)
        {
            IList<CardElement> frontCardElements = CardElement.ParseStringRepresentation(frontContentStr);

            foreach (CardElement singleElement in frontCardElements)
            {
                CardRelation templateRelation = __frontElements.Find((rel) => string.Equals(rel.Element.Id, singleElement.TemplateElementID, StringComparison.InvariantCultureIgnoreCase));

                if (templateRelation == null)
                    return;

                templateRelation.Content.LoadContentValue(singleElement.Value);
            }
        }

        private string BackTemplateElementsToString()
        {
            List<TemplateElement> elementsInBackTemplate = new List<TemplateElement>();

            foreach (CardRelation singleRelation in this.BackElements)
                elementsInBackTemplate.Add(singleRelation.Element);

            string elementsInTemplateStr = TemplateElement.RepresentToString(elementsInBackTemplate);
            return elementsInTemplateStr;
        }

        private void FromStringToBackTemplateElements(string backElementsStr)
        {
            List<TemplateElement> backTemplateElements = TemplateElement.ParseFromStringRepresentation(backElementsStr);

            foreach (TemplateElement element in backTemplateElements)
            {
                CardRelation newRelation = new CardRelation();
                newRelation.Content = element.CreateDrawingContent();
                newRelation.Element = element;
                __backElements.Add(newRelation);
            }
        }

        private string BackValuesToString()
        {
            List<CardElement> backElements = new List<CardElement>();

            foreach (CardRelation singleRelation in this.BackElements)
            {
                CardElement newCardElement = new CardElement();

                TemplateElement element = singleRelation.Element;
                DrawingContent content = singleRelation.Content;

                newCardElement.TemplateElementID = element.Id;
                newCardElement.Value = content.GenerateContentValue();

                backElements.Add(newCardElement);
            }

            string backValuesStr = CardElement.RepresentToString(backElements);
            return backValuesStr;
        }

        private void FromStringToBackValues(string backContentStr)
        {
            IList<CardElement> backCardElements = CardElement.ParseStringRepresentation(backContentStr);

            foreach (CardElement singleElement in backCardElements)
            {
                CardRelation templateRelation = __backElements.Find((rel) => string.Equals(rel.Element.Id, singleElement.TemplateElementID, StringComparison.InvariantCultureIgnoreCase));

                if (templateRelation == null)
                    continue;

                templateRelation.Content.LoadContentValue(singleElement.Value);
            }
        }

        private int GetNewId()
        {
            string selectMaxQuery = string.Format("SELECT MAX ({1}) AS {2}  FROM {0}", TABLE_NAME, ID, MAX_ID);
            DataTable table = SQLiteLinker.GetDataTable(selectMaxQuery);

            if (table.Rows.Count == 0)
                throw new InvalidOperationException("This should return always 1 row!!");

            DataRow firstRow = table.Rows[0];

            if (firstRow[MAX_ID] == DBNull.Value)
                return PersistenceDefaultValues.FIRST_IDENTIFIER;

            int maxIdentifier = Convert.ToInt32(firstRow[MAX_ID]);
            maxIdentifier++;

            return maxIdentifier;
        }
    }
}

