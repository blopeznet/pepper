using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using Pepper.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(MyCellRenderer))]

namespace Pepper.iOS.Renderers
{
    /// <summary>
    /// This class make transparent tap into cell iOS
    /// </summary>
    public class MyCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);            
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = UIColor.Clear,
            };

            return cell;
        }

    }
}