using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views.InputMethods;
using Java.IO;
using Pepper;
using Xamarin.Forms;

// Requires android.permission.WRITE_EXTERNAL_STORAGE in AndroidManifest.xml

[assembly: Dependency(typeof(Pepper.Droid.HelpersDependencyService))]

namespace Pepper.Droid
{
    
    public class HelpersDependencyService : IHelpersDependencyService
    {
        public async Task<bool> SaveBitmap(byte[] buffer, string filename)
        {
            try
            {
                File picturesDirectory = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures);
                File PaintDirectory = new File(picturesDirectory, "Pepper");
                PaintDirectory.Mkdirs();

                using (File bitmapFile = new File(PaintDirectory, filename))
                {
                    bitmapFile.CreateNewFile();

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(buffer);
                    }

                    // Make sure it shows up in the Photos gallery promptly.
                    MediaScannerConnection.ScanFile(Forms.Context,
                                                    new string[] { bitmapFile.Path },
                                                    new string[] { "image/png", "image/jpeg" }, null);
                }
            }
            catch
            {
                return true;
            }

            return true;
        }

        public void HideKeyboard()
            {
                var context = Forms.Context;
                var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (inputMethodManager != null && context is Activity)
                {
                    var activity = context as Activity;
                    var token = activity.CurrentFocus?.WindowToken;
                    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                    activity.Window.DecorView.ClearFocus();
                }
        }
        
    }
}