using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StudyCards.Mobile
{
    public partial class Desk
    {
        private bool __cardsLoaded = false;
        private int _id;
        private string _name;
        private string _cardFrontTemplate;
        private string _cardBackTemplate;
        private string _background;
        private List<Card> _cards;
        private List<Card> _filteredCards;

        public int Id { get { return _id; } internal set { _id = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public string CardFrontTemplateName { get { return _cardFrontTemplate; } internal set { _cardFrontTemplate = value; } }

        public string CardBackTemplateName { get { return _cardBackTemplate; } internal set { _cardBackTemplate = value; } }

        public string BackgroundName { get { return _background; } internal set { _background = value; } }

        public ReadOnlyCollection<Card> Cards { get; private set; }

        public ReadOnlyCollection<Card> FilteredCards { get; private set; }

        public Desk()
        {
            this.Id = PersistenceDefaultValues.NO_IDENTIFIED;
            this.Name = string.Empty;
            this.CardBackTemplateName = string.Empty;
            this.CardFrontTemplateName = string.Empty;
            this.BackgroundName = string.Empty;

            _cards = new List<Card>();
            this.Cards = _cards.AsReadOnly();

            _filteredCards = new List<Card>();
            this.FilteredCards = _filteredCards.AsReadOnly();
        }

        public Template GetCardFrontTemplate()
        {
            Template frontTemplate = TemplatesManager.TemplateByName(this.CardFrontTemplateName);
            return (frontTemplate == null) ? TemplatesManager.DefaultFrontTemplate : frontTemplate;
        }

        public void SetCardFrontTemplate(Template template)
        {
            this.CardFrontTemplateName = template.Name;
        }

        public Template GetCardBackTemplate()
        {
            Template backTemplate = TemplatesManager.TemplateByName(this.CardBackTemplateName);
            return (backTemplate == null) ? TemplatesManager.DefaultBackTemplate : backTemplate;
        }

        public void SetCardBackTemplate(Template template)
        {
            this.CardBackTemplateName = template.Name;
        }

        public Background GetBackground()
        {
            Background deskBackground = BackgroundsManager.BackgroundByName(this.BackgroundName);
            return (deskBackground == null) ? BackgroundsManager.DefaultBackground : deskBackground;
        }

        public void SetBackground(Background background)
        {
            this.BackgroundName = background.Name;
        }

        public void LoadCards()
        {
            if (__cardsLoaded)
                return;

            _cards.Clear();
            List<Card> deskCards = Card.GetCards(this.Id);
            _cards.AddRange(deskCards);
            __cardsLoaded = true;
        }

        public void Search(string searchCriteria)
        {
            _filteredCards.Clear();

            if (string.IsNullOrEmpty(searchCriteria))
            {
                _filteredCards.AddRange(_cards);
                return;
            }

            foreach (Card singleCard in this.Cards)
            {
                if (singleCard.Search(searchCriteria))
                    _filteredCards.Add(singleCard);
            }
        }

        public Card CreateCard()
        {
            Card newDeskCard = new Card();

            Template backTemplate = this.GetCardBackTemplate();
            backTemplate.LoadTemplate();
            newDeskCard.LoadTemplateInBack(backTemplate);

            Template frontTemplate = this.GetCardFrontTemplate();
            frontTemplate.LoadTemplate();
            newDeskCard.LoadTemplateInFront(frontTemplate);

            return newDeskCard;
        }

        public void AddCard(Card newCard, int index)
        {
            if (newCard == null || index < 0)
                return;

            //In case the card is already registered, we change the id so is considered like a new one (will be inserted)
            newCard.Id = PersistenceDefaultValues.NO_IDENTIFIED;
            newCard.DeskId = this.Id;
            int realIndex = (index > _cards.Count) ? _cards.Count : index;
            newCard.Index = realIndex;

            newCard.Save();
            _cards.Insert(realIndex, newCard);

            this.RefreshCardsIndexes();
        }

        public void RemoveCard(int index)
        {
            if (index < 0 || index > this.Cards.Count)
                return;

            Card cardToRemove = _cards[index];
            cardToRemove.Delete();
            _cards.RemoveAt(index);
            this.RefreshCardsIndexes();
        }

        private void RefreshCardsIndexes()
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                this.Cards[i].Index = i;
                this.Cards[i].SaveIndex();
            }
        }
    }
}

