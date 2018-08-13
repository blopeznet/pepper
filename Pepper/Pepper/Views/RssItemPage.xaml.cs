using System;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pepper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// ImageCoverView Marvel comics code behind
    /// </summary>
	public partial class RssItemPage : ContentPage
	{

        private Xam.Rss.FeedItem item { get; set; }

		public RssItemPage(Xam.Rss.FeedItem i)
		{
			InitializeComponent ();
            item = i;
            UpdateWebView();
		}        

        private void UpdateWebView()
        {
            var html = new HtmlWebViewSource();
            html.BaseUrl = item.Link;
            html.Html = Xam.Wikia.Helper.WikiReadingHelper.ParseHtmlCodeWithCss(item.Description);            
            this.webViewFeed.Source = html;
        }

        /// <summary>
        /// Button tap action share feed item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShareFeed(object sender, EventArgs e)
        {
            ViewModels.MainViewModel.Instance.IsBusy = true;
            await Xamarin.Essentials.DataTransfer.RequestAsync(new Xamarin.Essentials.ShareTextRequest
            {
                Uri = item.Link,
                Title = "Share Feed Link"
            });
            ViewModels.MainViewModel.Instance.IsBusy = false;
        }

        /// <summary>
        /// Button tap action view feed item in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private async void BrowserFeed(object sender, EventArgs ev)
        {

            ViewModels.MainViewModel.Instance.IsBusy = true;
            Device.OpenUri(new Uri(item.Link));
            ViewModels.MainViewModel.Instance.IsBusy = false;
        }
    }
}