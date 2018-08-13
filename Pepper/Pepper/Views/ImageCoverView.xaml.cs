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
	public partial class ImageCoverView : ContentView
	{
		public ImageCoverView ()
		{
			InitializeComponent ();
		}        

        /// <summary>
        /// Button tap action share image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShareImg(object sender, EventArgs e)
        {
            ViewModels.MainViewModel.Instance.IsBusy = true;
            Xam.Marvelous.Model.Base.Image img = ((Xam.Marvelous.Model.Base.Comic)this.carrouserControl.BindingContext).Images[this.carrouserControl.Position];
            await Xamarin.Essentials.DataTransfer.RequestAsync(new Xamarin.Essentials.ShareTextRequest
            {
                Uri = img.DisplayPath,
                Title = "Share Cover Link"
            });
            ViewModels.MainViewModel.Instance.IsBusy = false;
        }

        /// <summary>
        /// Button tap action view image in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private async void BrowserImg(object sender, EventArgs ev)
        {

            ViewModels.MainViewModel.Instance.IsBusy = true;
            Xam.Marvelous.Model.Base.Image img = ((Xam.Marvelous.Model.Base.Comic)this.carrouserControl.BindingContext).Images[this.carrouserControl.Position];
            Device.OpenUri(new Uri(img.DisplayPath));
            ViewModels.MainViewModel.Instance.IsBusy = false;
        }
    }
}