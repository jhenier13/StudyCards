using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace StudyCards.Mobile
{
    public static class BackgroundsManager
    {
        private static readonly string DEFAULT_BACKGROUND_NAME = "Dark Blue.png";
        private static List<Background> __backgrounds;

        public static ReadOnlyCollection<Background> Backgrounds { get; private set; }

        public static Background DefaultBackground { get; private set; }

        public static IDeviceResourcesManager ResourcesManager { get; set; }

        static BackgroundsManager()
        {
            __backgrounds = new List<Background>();
            Backgrounds = __backgrounds.AsReadOnly();
        }

        public static void CopyDefaultBackgroundsToLibrary()
        {
            bool defaultBackgroundsExists = Directory.Exists(ApplicationEnviroment.DEFAULT_BACKGROUNDS_DIRECTORY);

            if (defaultBackgroundsExists)
                return;

            if (ResourcesManager == null)
                throw new NullReferenceException("A ResourcesManager is needed for this task");

            Directory.CreateDirectory(ApplicationEnviroment.DEFAULT_BACKGROUNDS_DIRECTORY);
            Directory.CreateDirectory(ApplicationEnviroment.CUSTOM_BACKGROUNDS_DIRECTORY);

            string defaultBackgroundsSource = Path.Combine(ResourcesManager.ResourcesDirectory, "Backgrounds");

            DirectoryInfo sourceDirectory = new DirectoryInfo(defaultBackgroundsSource);
            DirectoryInfo destinyDirectory = new DirectoryInfo(ApplicationEnviroment.DEFAULT_BACKGROUNDS_DIRECTORY);

            foreach (FileInfo singleBackground in sourceDirectory.GetFiles("*.png"))
            {
                string destinyPath = Path.Combine(destinyDirectory.FullName, singleBackground.Name);
                singleBackground.CopyTo(destinyPath);
            }
        }

        public static void LoadBackgrounds()
        {
            DirectoryInfo defaultBackgrounds = new DirectoryInfo(ApplicationEnviroment.DEFAULT_BACKGROUNDS_DIRECTORY);

            foreach (FileInfo singleBackground in defaultBackgrounds.GetFiles("*.png"))
            {
                Background newDefaultBackground = new Background();
                newDefaultBackground.Name = singleBackground.Name.Substring(0, singleBackground.Name.Length - singleBackground.Extension.Length);
                newDefaultBackground.IsDefault = true;
                newDefaultBackground.Location = singleBackground.FullName;

                if (string.Equals(singleBackground.Name, DEFAULT_BACKGROUND_NAME))
                    DefaultBackground = newDefaultBackground;

                __backgrounds.Add(newDefaultBackground);
            }

            DirectoryInfo customBackgrounds = new DirectoryInfo(ApplicationEnviroment.CUSTOM_BACKGROUNDS_DIRECTORY);

            foreach (FileInfo singleBackground in customBackgrounds.GetFiles("*.png"))
            {
                Background newCustomBackground = new Background();
                newCustomBackground.Name = singleBackground.Name.Substring(0, singleBackground.Name.Length - singleBackground.Extension.Length);
                newCustomBackground.IsDefault = false;
                newCustomBackground.Location = singleBackground.FullName;

                __backgrounds.Add(newCustomBackground);
            }
        }

        public static Background BackgroundByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return DefaultBackground;

            Background foundedBackground = __backgrounds.Find((back) => string.Equals(back.Name, name));

            if (foundedBackground == null)
                return DefaultBackground;

            return foundedBackground;
        }
    }
}

