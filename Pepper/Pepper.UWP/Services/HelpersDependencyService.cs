using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pepper;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

namespace Pepper.UWP
{
    public class HelpersDependencyService : IHelpersDependencyService
    {
        public void HideKeyboard()
        {
            try { 
                var key = InputPane.GetForCurrentView();
                if (key != null)
                    key.TryHide();
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error hide keyboard UWP: "+ex.Message);
            }
        }

        public async Task<bool> SaveBitmap(byte[] bitmapData, string filename)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("PNG", new List<string>() { ".png" });
            savePicker.FileTypeChoices.Add("JPEG", new List<string>() { ".jpeg" });
            savePicker.FileTypeChoices.Add("GIF", new List<string>() { ".gif" });
            savePicker.SuggestedFileName = filename;
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                IRandomAccessStream filestream = await file.OpenAsync(FileAccessMode.Read);                                
                await FileIO.WriteBytesAsync(file,bitmapData);
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                return true;
            }
            return false;
        }
    }
}
