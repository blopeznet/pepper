using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pepper.Views
{
    /// <summary>
    /// MainPage code behind
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Show Wikia Web on Browser button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Wikia(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://marvel.wikia.com/wiki/Marvel_Database"));
        }

        /// <summary>
        /// Show Marvel Web on Marvel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Marvel(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.marvel.com/explore"));

        }

        /// <summary>
        /// Show Tumblr Web on Tumblr button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Tumblr(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://marvelentertainment.tumblr.com/"));

        }

        private void SearchEntry_Focused(object sender, FocusEventArgs e)
        {
            CancelSearch.IsVisible = true;
        }

        private void SearchEntry_Unfocused(object sender, FocusEventArgs e)
        {
            CancelSearch.IsVisible = false;
        }
    }
}