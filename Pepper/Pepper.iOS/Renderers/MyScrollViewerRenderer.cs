using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Pepper.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ScrollView), typeof(MyScrollViewerRenderer))]
namespace Pepper.iOS.Renderers
{
    /// <summary>
    /// This class clear bounds on iOS to scrollviewers
    /// </summary>
    public class MyScrollViewerRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            this.BouncesZoom = false;
            this.Bounces = false;            
        }
    }
}