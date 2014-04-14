using System;

namespace StudyCards.Mobile
{
    public interface IParsableObject
    {
        void Parse(string data);
        string GenerateString();
    }
}

