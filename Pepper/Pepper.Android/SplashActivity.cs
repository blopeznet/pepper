
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Threading;

namespace Pepper.Droid
{

    /// <summary>
    /// Activity Splash page
    /// </summary>
    [Activity(Label = "Pepper", Icon = "@mipmap/icon", Theme = "@style/SplashTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]    
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            // Wait for 2 seconds
            Thread.Sleep(2000);
            StartActivity(intent);
            Finish();
        }
    }
}

