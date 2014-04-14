using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using StudyCards.Mobile.Persistence;
using StudyCards.Mobile;

namespace StudyCards.Iphone
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;
        CustomNavigationController __viewController;
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            DataBaseUpdater.TryCreateDataBase();

            IphoneResourcesManager iphoneResources = new IphoneResourcesManager();
            BackgroundsManager.ResourcesManager = iphoneResources;
            TemplatesManager.ResourcesManager = iphoneResources;

            BackgroundsManager.CopyDefaultBackgroundsToLibrary();
            TemplatesManager.CopyDefaultTemplatesToLibrary();

            BackgroundsManager.LoadBackgrounds();
            TemplatesManager.LoadTemplates();

            window = new UIWindow(UIScreen.MainScreen.Bounds);
			
            __viewController = new CustomNavigationController();
            window.RootViewController = __viewController;
            window.MakeKeyAndVisible();
			
            return true;
        }
    }
}

