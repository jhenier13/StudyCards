using System;
using StudyCards.Mobile.DrawingElements;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Globalization;

namespace StudyCards.Iphone
{
    public static class DrawingUtils
    {
        private static Dictionary<string, UIColor> __colors;

        static DrawingUtils()
        {
            __colors = new Dictionary<string, UIColor>();
            __colors.Add("Black", UIColor.Black);
            __colors.Add("Blue", UIColor.Blue);
            __colors.Add("Brown", UIColor.Brown);
            __colors.Add("Clear", UIColor.Clear);
            __colors.Add("Cyan", UIColor.Cyan);
            __colors.Add("Dark Gray", UIColor.DarkGray);
            __colors.Add("Gray", UIColor.Gray);
            __colors.Add("Green", UIColor.Green);
            __colors.Add("Light Gray", UIColor.LightGray);
            __colors.Add("Magenta", UIColor.Magenta);
            __colors.Add("Orange", UIColor.Orange);
            __colors.Add("Purple", UIColor.Purple);
            __colors.Add("Red", UIColor.Red);
            __colors.Add("White", UIColor.White);
            __colors.Add("Yellow", UIColor.Yellow);
        }

        public static UITextAlignment ConvertToUITextAlignment(TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Left:
                    return UITextAlignment.Left;
                case TextAlignment.Center:
                    return UITextAlignment.Center;
                case TextAlignment.Right:
                    return UITextAlignment.Right;
                default:
                    return UITextAlignment.Left;
            }
        }

        public static TextAlignment ConvertToTextAlignment(UITextAlignment alignment)
        {
            switch (alignment)
            {
                case UITextAlignment.Left:
                    return TextAlignment.Left;
                case UITextAlignment.Center:
                    return TextAlignment.Center;
                case UITextAlignment.Right:
                    return TextAlignment.Right;
                default:
                    return TextAlignment.Center;
            }
        }
        //This needs serius changes
        public static UIFont CreateFont(string fontFamily, float fontSize, bool isBold)
        {
            string fontName = fontFamily;

            if (isBold)
                fontName = string.Format("{0}-BoldMT", fontName);

            UIFont font = UIFont.FromName(fontName, fontSize);

            return font;
        }

        public static UIColor CreateColor(string colorName)
        {
            if (__colors.ContainsKey(colorName))
            {
                UIColor color = __colors[colorName];
                return color;
            }
            else
            {
                throw new ArgumentException("The color name doesn't exists in the application");
            }
        }
    }
}

