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
    /// MarvelDetailPage code behind
    /// </summary>
	public partial class MarvelDetailPage : ContentPage
    {

        #region private vars

        private const int ParallaxSpeed = 5;

        private double _lastScroll;

        #endregion


        public MarvelDetailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event appear to suscribe scroll event
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ParallaxScroll.Scrolled += OnParallaxScrollScrolled;
        }

        /// <summary>
        /// Event appear to unsuscribe scroll event
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ParallaxScroll.Scrolled -= OnParallaxScrollScrolled;
        }

        /// <summary>
        /// Effect parallax when scroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnParallaxScrollScrolled(object sender, ScrolledEventArgs e)
        {
            double translation = 0;

            if (_lastScroll < e.ScrollY)
            {
                translation = 0 - ((e.ScrollY / 2));
                if (translation > 0) translation = 0;
            }
            else
            {
                translation = 0 + ((e.ScrollY / 2));
                if (translation > 0) translation = 0;
            }

            HeaderPanel.TranslateTo(HeaderPanel.TranslationX, translation);
            _lastScroll = e.ScrollY;
        }
    }
}