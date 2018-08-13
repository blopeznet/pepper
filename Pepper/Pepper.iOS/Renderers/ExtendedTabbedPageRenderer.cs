using System;
using Pepper.iOS.Renderers;
using Pepper.iOS.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace Pepper.iOS.Renderers
{
    /// <summary>
    /// This subclass of <see cref="TabbedRenderer"/> changes the appearance of the pages attached to a <see cref="TabbedPage"/> instance.
    /// </summary>
    public class ExtendedTabbedPageRenderer : TabbedRenderer
{
    /// <summary>
    /// This method is overridden to allow the tab bar items to have their fonts updated appropriately.
    /// </summary>
    /// <param name="animated">Whether it's animated or not.</param>
    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        UpdateAllTabBarItems();
    }

    /// <summary>
    /// Updates the tab bar item for each page included in the TabbedPage.
    /// </summary>
    private void UpdateAllTabBarItems()
    {
        foreach (var controller in ViewControllers)
        {
            controller.TabBarItem.SetTitleTextAttributes(StandardAttributes, UIControlState.Normal);
            controller.TabBarItem.TitlePositionAdjustment = new UIOffset(0, 0f);
        }
    }

    /// <summary>
    /// Stores the UITextAttributes for this class.
    /// </summary>
    /// <value>The standard attributes.</value>
    private static UITextAttributes StandardAttributes { get; } = new UITextAttributes { Font = FontManager.GetFont(10.0f) };
}
}