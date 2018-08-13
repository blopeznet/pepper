using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pepper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// MainPage code behind
    /// </summary>
    public partial class ResultPage : TabbedPage
    {
        /// <summary>
        /// Constructor without wikia source
        /// </summary>
        public ResultPage ()
        {
            InitializeComponent();           
        }

        protected override void OnAppearing()
        {
            AssociateWebview();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            DesAssociateWebview();
            base.OnDisappearing();
        }

        private void AssociateWebview()
        {
          this.webViewWikia.Navigating += webViewWikia_Navigating;            
        }

        private void DesAssociateWebview()
        {
            this.webViewWikia.Navigating -= webViewWikia_Navigating;
        }

        /// <summary>
        /// Constructor with wikia source
        /// </summary>
        /// <param name="source"></param>
        public ResultPage(HtmlWebViewSource source)
        {
            InitializeComponent();
            tabWikia.IsVisible = true;
            this.webViewWikia.Source = source;
        }

        /// <summary>
        /// Event navigating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webViewWikia_Navigating(object sender, WebNavigatingEventArgs e)
        {            
            Device.OpenUri(ChangueLocalURL(e.Url));
            e.Cancel = true;           
        }
        
        /// <summary>
        /// Check if is URL
        /// </summary>
        /// <param name="uriName"></param>
        /// <returns></returns>
        private bool IsWebURL(String uriName)
        {            
            bool result = uriName.StartsWith("file://");
            return result;
        }

        /// <summary>
        /// Change local URL to web url
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private Uri ChangueLocalURL(String URL)
        {
            if (IsWebURL(URL)) { 
                String url = URL.Replace("file://", "http://marvel.wikia.com");
                return new Uri(url);
            }
            else            
                return new Uri(URL); 
            
        }
    }
}