using System;

namespace StudyCards.Mobile.DrawingElements
{
    public struct DrawPoint
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public static DrawPoint FromString(string point)
        {
            DrawPoint newPoint = new DrawPoint();

            string[] pointParts = point.Split(new char[]{ ',' }, 2);
            newPoint.X = float.Parse(pointParts[0]);
            newPoint.Y = float.Parse(pointParts[1]);

            return newPoint;
        }
    }
}

