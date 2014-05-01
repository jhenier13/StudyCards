using System;

namespace StudyCards.Mobile.DrawingElements
{
    public struct DrawSize
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", Width, Height);
        }

        public static DrawSize FromString(string size)
        {
            DrawSize newSize = new DrawSize();

            string [] sizeParts = size.Split(new char[]{ ',' }, 2);
            newSize.Width = float.Parse(sizeParts[0]);
            newSize.Height = float.Parse(sizeParts[1]);

            return newSize;
        }
    }
}

