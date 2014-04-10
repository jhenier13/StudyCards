using System;
using System.IO;

namespace StudyCards.Mobile
{
    public static class ApplicationEnviroment
    {
        private static string TEMPLATES = "Templates";
        private static string IMAGES = "Images";
        private static string BACKGROUNDS = "Backgrounds";
        private static string CUSTOM = "Custom";
        private static string DEFAULT = "Default";
        public static readonly string TEMPLATES_DIRECTORY;
        public static readonly string DEFAULT_TEMPLATES_DIRECTORY;
        public static readonly string CUSTOM_TEMPLATES_DIRECTORY;
        public static readonly string IMAGES_DIRECTORY;
        public static readonly string BACKGROUNDS_DIRECTORY;
        public static readonly string DEFAULT_BACKGROUNDS_DIRECTORY;
        public static readonly string CUSTOM_BACKGROUNDS_DIRECTORY;

        static ApplicationEnviroment()
        {
            TEMPLATES_DIRECTORY = Path.Combine(EnviromentDirectories.IOS_LIBRARY_DIRECTORY, TEMPLATES);
            DEFAULT_TEMPLATES_DIRECTORY = Path.Combine(TEMPLATES_DIRECTORY, DEFAULT);
            CUSTOM_TEMPLATES_DIRECTORY = Path.Combine(TEMPLATES_DIRECTORY, CUSTOM);
            IMAGES = Path.Combine(EnviromentDirectories.IOS_LIBRARY_DIRECTORY, IMAGES);
            BACKGROUNDS_DIRECTORY = Path.Combine(EnviromentDirectories.IOS_LIBRARY_DIRECTORY, BACKGROUNDS);
            DEFAULT_BACKGROUNDS_DIRECTORY = Path.Combine(BACKGROUNDS_DIRECTORY, DEFAULT);
            CUSTOM_BACKGROUNDS_DIRECTORY = Path.Combine(BACKGROUNDS_DIRECTORY, CUSTOM);
        }
    }
}

