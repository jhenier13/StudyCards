using System;
using System.IO;

namespace StudyCards.Mobile
{
    public static class EnviromentDirectories
    {
        private static readonly string IOS_LIBRARY = "Library";
        private static readonly string IOS_TEMP = "tmp";
        public static readonly string IOS_LIBRARY_DIRECTORY;
        public static readonly string IOS_DOCUMENTS_DIRECTORY;
        public static readonly string IOS_TEMP_DIRECTORY;

        static EnviromentDirectories()
        {
            IOS_DOCUMENTS_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            IOS_LIBRARY_DIRECTORY = Path.Combine(IOS_DOCUMENTS_DIRECTORY, "..", IOS_LIBRARY);
            IOS_TEMP_DIRECTORY = Path.Combine(IOS_DOCUMENTS_DIRECTORY, "..", IOS_TEMP);
        }
    }
}

