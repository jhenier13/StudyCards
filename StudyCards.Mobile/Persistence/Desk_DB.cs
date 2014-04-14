using System;
using System.Data;
using StudyCards.Mobile.Persistence;
using System.Collections.Generic;

namespace StudyCards.Mobile
{
    public partial class Desk
    {
        #region DB_Fields

        private static readonly string TABLE_NAME = "Desks";
        private static readonly string MAX_ID = "MaxID";
        private static readonly string ID = "DeskID";
        private static readonly string NAME = "Name";
        private static readonly string CARD_BACK_TEMPLATE = "CardBackTemplate";
        private static readonly string CARD_FRONT_TEMPLATE = "CardFrontTemplate";
        private static readonly string BACKGROUND = "Background";

        #endregion

        public static List<Desk> GetAllDesks()
        {
            List<Desk> allDesks = new List<Desk>();

            string selectDesks = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable data = SQLiteLinker.GetDataTable(selectDesks);

            foreach (DataRow singleRow in data.Rows)
            {
                Desk newDesk = new Desk();
                newDesk.LoadData(singleRow);
                allDesks.Add(newDesk);
            }

            return allDesks;
        }

        public void Save()
        {
            if (this.Id == PersistenceDefaultValues.NO_IDENTIFIED)
                this.Insert();
            else
                this.Update();
        }

        public void Delete()
        {
            if (this.Id == PersistenceDefaultValues.NO_IDENTIFIED)
                return;

            string deleteQuery = string.Format("DELETE FROM {0} WHERE {1}={2}", TABLE_NAME, ID, this.Id);
            SQLiteLinker.ExecuteQuery(deleteQuery);
        }

        private void Update()
        {
            string updateQuery = string.Format("UPDATE {0} SET {3}='{4}',{5}='{6}',{7}='{8}',{9}='{10}' WHERE {1}={2}",
                                     TABLE_NAME, ID, this.Id, NAME, SQLiteLinker.FixSQLInjection(this.Name), CARD_FRONT_TEMPLATE, this.CardFrontTemplateName,
                                     CARD_BACK_TEMPLATE, this.CardBackTemplateName, BACKGROUND, this.BackgroundName);

            SQLiteLinker.ExecuteQuery(updateQuery);
        }

        private void Insert()
        {
            this.Id = this.GetNewId();
            string insertQuery = string.Format("INSERT INTO {0}({1},{3},{5},{7},{9}) VALUES ({2},'{4}','{6}','{8}','{10}')",
                                     TABLE_NAME, ID, this.Id, NAME, SQLiteLinker.FixSQLInjection(this.Name), CARD_BACK_TEMPLATE, this.CardBackTemplateName,
                                     CARD_FRONT_TEMPLATE, this.CardFrontTemplateName, BACKGROUND, this.BackgroundName);

            SQLiteLinker.ExecuteQuery(insertQuery);
        }

        private void LoadData(DataRow data)
        {
            this.Id = Convert.ToInt32(data[ID]);
            this.Name = Convert.ToString(data[NAME]);
            this.CardFrontTemplateName = Convert.ToString(data[CARD_FRONT_TEMPLATE]);
            this.CardBackTemplateName = Convert.ToString(data[CARD_BACK_TEMPLATE]);
            this.BackgroundName = Convert.ToString(data[BACKGROUND]);
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

