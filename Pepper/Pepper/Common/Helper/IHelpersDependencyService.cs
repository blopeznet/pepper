using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pepper
{    
    public interface IHelpersDependencyService
    {
        
        Task<bool> SaveBitmap(byte[] bitmapData, string filename);

        void HideKeyboard();

    }    

    public static class Helpers
    {
        public static void HideHeyboardFromScreen()
        {
            try
            {
                DependencyService.Get<IHelpersDependencyService>().HideKeyboard();
            }catch(Exception ex) { }
        }
    }
}
