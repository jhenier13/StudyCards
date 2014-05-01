using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;

namespace StudyCards.Mobile
{
    public static class TemplatesManager
    {
        private static readonly string DEFAULT_FRONT_TEMPLATE = "Template1.xml";
        private static readonly string DEFAULT_BACK_TEMPLATE = "Template2.xml";
        private static List<Template> __templates;
        private static bool __templatesNameLoaded;

        public static ReadOnlyCollection<Template> Templates { get; private set; }

        public static Template DefaultFrontTemplate { get; private set; }

        public static Template DefaultBackTemplate { get; private set; }

        public static IDeviceResourcesManager ResourcesManager { get; set; }

        static TemplatesManager()
        {
            __templates = new List<Template>();
            Templates = __templates.AsReadOnly();
            __templatesNameLoaded = false;
        }

        public static void CopyDefaultTemplatesToLibrary()
        {
            bool defaultTemplatesExists = Directory.Exists(ApplicationEnviroment.DEFAULT_TEMPLATES_DIRECTORY);

            if (defaultTemplatesExists)
                return;

            if (ResourcesManager == null)
                throw new NullReferenceException("A ResourcesManager is needed for this task");

            Directory.CreateDirectory(ApplicationEnviroment.DEFAULT_TEMPLATES_DIRECTORY);
            Directory.CreateDirectory(ApplicationEnviroment.CUSTOM_TEMPLATES_DIRECTORY);

            string defaultBackgroundsSource = Path.Combine(ResourcesManager.ResourcesDirectory, "Templates");

            DirectoryInfo sourceDirectory = new DirectoryInfo(defaultBackgroundsSource);
            DirectoryInfo destinyDirectory = new DirectoryInfo(ApplicationEnviroment.DEFAULT_TEMPLATES_DIRECTORY);

            foreach (FileInfo singleBackground in sourceDirectory.GetFiles("*.xml"))
            {
                string destinyPath = Path.Combine(destinyDirectory.FullName, singleBackground.Name);
                singleBackground.CopyTo(destinyPath);
            }
        }

        public static void LoadTemplates()
        {
            DirectoryInfo defaultTemplates = new DirectoryInfo(ApplicationEnviroment.DEFAULT_TEMPLATES_DIRECTORY);

            foreach (FileInfo singleTemplate in defaultTemplates.GetFiles("*.xml"))
            {
                Template newDefaultTemplate = new Template();
                newDefaultTemplate.IsDefault = true;
                newDefaultTemplate.Location = singleTemplate.FullName;

                if (string.Equals(singleTemplate.Name, DEFAULT_FRONT_TEMPLATE))
                    DefaultFrontTemplate = newDefaultTemplate;

                if (string.Equals(singleTemplate.Name, DEFAULT_BACK_TEMPLATE))
                    DefaultBackTemplate = newDefaultTemplate;

                __templates.Add(newDefaultTemplate);
            }

            DirectoryInfo customTemplates = new DirectoryInfo(ApplicationEnviroment.CUSTOM_TEMPLATES_DIRECTORY);

            foreach (FileInfo singleTemplate in customTemplates.GetFiles("*.xml"))
            {
                Template newCustomTemplate = new Template();
                newCustomTemplate.IsDefault = false;
                newCustomTemplate.Location = singleTemplate.FullName;

                __templates.Add(newCustomTemplate);
            }
        }

        public static void SaveTemplate(Template template)
        {
            throw new NotImplementedException("Check that the template is already in the list");
        }

        public static Template TemplateByName(string name)
        {
            LoadTemplatesNames();

            Template foundedTemplate = __templates.Find((temp) => temp.Name == name);
            return foundedTemplate;
        }

        public static void LoadTemplatesNames()
        {
            if (__templatesNameLoaded)
                return;

            foreach (Template template in __templates)
                template.LoadName();

            __templatesNameLoaded = true;
        }
    }
}

