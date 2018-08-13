using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pepper.Common.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// View generic for loading
    /// </summary>
    public partial class NetErrorView : ContentView
	{        
		public NetErrorView()
		{
			InitializeComponent ();
		}
	}
}