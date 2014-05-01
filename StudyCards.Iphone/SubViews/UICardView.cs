using System;
using MonoTouch.UIKit;
using StudyCards.Mobile;
using System.Collections.Generic;
using System.Drawing;
using StudyCards.Iphone.DisplayViews;

namespace StudyCards.Iphone.SubViews
{
    public class UICardView : UIView
    {
        //Flags
        private bool __isLoaded = false;
        //Attributes
        private Background __cardBackground;
        private List<CardRelation> _elements;
        //UIControls
        private UIImageView __backgroundView;

        public Background CardBackground
        {
            get{ return __cardBackground; }
            set
            {
                __cardBackground = value; 

                if (!__isLoaded)
                    return;

                this.SetBackgroundImage();
            }
        }

        public List<CardRelation> Elements
        {
            get{ return _elements; }
            set
            {
                if (_elements == value)
                    return;

                _elements = value;

                if (!__isLoaded)
                    return;

                this.DrawElements();
            }
        }

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;

                if (!__isLoaded)
                    return;

                this.UpdateLayout();
            }
        }

        public UICardView()
        {
            this.Layer.CornerRadius = 10.0F;
            this.ClipsToBounds = true;

            this.CreateUIControls();
            this.AddUIControls();
        }

        public UICardView(Background cardBackground)
        {
            this.CardBackground = cardBackground;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!__isLoaded)
                this.DrawElements();

            this.UpdateLayout();
            this.SetBackgroundImage();
            __isLoaded = true;
        }

        private void CreateUIControls()
        {
            __backgroundView = new UIImageView();
            __backgroundView.ContentMode = UIViewContentMode.ScaleAspectFill;
        }

        private void AddUIControls()
        {
            this.Add(__backgroundView);
        }

        private void UpdateLayout()
        {
            __backgroundView.Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height);
        }

        private void SetBackgroundImage()
        {
            UIImage image = UIImage.FromFile(this.CardBackground.Location);
            __backgroundView.Image = image;
        }

        private void DrawElements()
        {
            if (this.Elements == null)
                return;

            this.CleanSubViews();

            foreach (CardRelation singleElement in this.Elements)
            {
                TemplateElement element = singleElement.Element;
                DrawingContent content = singleElement.Content;

                PointF position = this.ScalePosition(element.Position.X, element.Position.Y);
                SizeF size = this.ScaleSize(element.Size.Width, element.Size.Height);

                IDisplayView displayView = DisplayViewFactory.CreateDisplayView(content);
                displayView.Content = content;
                UIView subView = displayView as UIView;
                subView.Frame = new RectangleF(position, size);
                this.Add(subView);
            }
        }

        private void CleanSubViews()
        {
            for (int i = 0; i < this.Subviews.Length; i++)
            {
                if (this.Subviews[i] == __backgroundView)
                    continue;

                this.Subviews[i].RemoveFromSuperview();
            }
        }

        private PointF ScalePosition(float xPosition, float yPosition)
        {
            float scaledXPosition = xPosition * this.Frame.Width / Template.WIDTH;
            float scaledYPosition = yPosition * this.Frame.Height / Template.HEIGHT;

            return new PointF(scaledXPosition, scaledYPosition);
        }

        private SizeF ScaleSize(float width, float height)
        {
            float scaledWidth = width * this.Frame.Width / Template.WIDTH;
            float scaledHeight = height * this.Frame.Height / Template.HEIGHT;

            return new SizeF(scaledWidth, scaledHeight);
        }
    }
}

