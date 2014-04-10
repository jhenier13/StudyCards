using System;
using MonoTouch.UIKit;

namespace StudyCards.Iphone
{
    public class CustomNavigationController : UINavigationController
    {
        public CustomNavigationController()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            DesksView desks = new DesksView();
            this.PushViewController(desks, true);
        }
    }
}

