using System;
using System.Collections.Generic;

namespace StudyCards.Mobile
{
    public partial class Desk
    {
        private int _id;
        private string _name;
        private string _cardFrontTemplate;
        private string _cardBackTemplate;
        private string _background;

        public int Id { get { return _id; } internal set { _id = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public string CardFrontTemplate { get { return _cardFrontTemplate; } set { _cardFrontTemplate = value; } }

        public string CardBackTemplate { get { return _cardBackTemplate; } set { _cardBackTemplate = value; } }

        public string Background { get { return _background; } set { _background = value; } }

        public Desk()
        {
            this.Id = PersistenceDefaultValues.NO_IDENTIFIED;
            this.Name = string.Empty;
            this.CardBackTemplate = string.Empty;
            this.CardFrontTemplate = string.Empty;
            this.Background = string.Empty;
        }
    }
}

