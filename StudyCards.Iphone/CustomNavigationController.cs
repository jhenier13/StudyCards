using System;
using MonoTouch.UIKit;

namespace StudyCards.Iphone
{
    public class CustomNavigationController : UINavigationController
    {
        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return UIInterfaceOrientation.LandscapeRight;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Landscape;
        }

        public CustomNavigationController()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
        }

        public override void LoadView()
        {
            base.LoadView();

            DesksView desks = new DesksView();
            this.PushViewController(desks, true);
        }
    }
}

