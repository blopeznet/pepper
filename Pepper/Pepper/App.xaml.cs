using System;
using Xamarin.Forms;
using Pepper.Views;
using Xamarin.Forms.Xaml;
using Pepper.ViewModels;
using DLToolkit.Forms.Controls;
using Xamarin.Essentials;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Pepper
{
    /// <summary>
    /// App code behind
    /// </summary>
    public partial class App : Application
	{
            
        /// <summary>
        /// Init app and main page
        /// </summary>
        public App ()
		{
			InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var current = Connectivity.NetworkAccess;
            MainViewModel.Instance.CheckConnectionAndUpdate();
            FlowListView.Init();            
            MainPage p = new MainPage();
            p.BindingContext = MainViewModel.Instance;
            p.Title = "P.E.P.P.E.R";
            NavigationPage navigationPage = new NavigationPage(p);
            MainPage = navigationPage;
            MainViewModel.Instance.LoadRss();

        }

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            var current = e.NetworkAccess;
            var profiles = e.Profiles;            
            if (current == NetworkAccess.Internet)
                MainViewModel.Instance.CheckConnectionAndUpdate(); 
            else
                MainViewModel.Instance.IsNetworkOk = false;
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
