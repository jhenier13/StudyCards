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

        public string CardFrontTemplateName { get { return _cardFrontTemplate; } internal set { _cardFrontTemplate = value; } }

        public string CardBackTemplateName { get { return _cardBackTemplate; } internal set { _cardBackTemplate = value; } }

        public string BackgroundName { get { return _background; } internal set { _background = value; } }

        public Desk()
        {
            this.Id = PersistenceDefaultValues.NO_IDENTIFIED;
            this.Name = string.Empty;
            this.CardBackTemplateName = string.Empty;
            this.CardFrontTemplateName = string.Empty;
            this.BackgroundName = string.Empty;
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
    }
}

