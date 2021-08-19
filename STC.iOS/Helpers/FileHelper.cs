using Foundation;
using STC.Interfaces;
using STC.iOS.Helpers;
using System;
using System.IO;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace STC.iOS.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetFilePath(string name)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename1 = Path.Combine(path.ToString(), name);
            return filename1;
        }

        public void OpenFile(string filepath, bool isRtl)
        {
            var PreviewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(filepath));
            PreviewController.Delegate = new UIDocumentInteractionControllerDelegateClass(UIApplication.SharedApplication.KeyWindow.RootViewController);
            Device.BeginInvokeOnMainThread(() =>
            {
                PreviewController.PresentPreview(true);
            });
        }

        public Stream OpenStream(string path, int bufferSize)
        {
            return new FileStream(path: path, mode: FileMode.Open);
            //throw new NotImplementedException();
        }

        public string SaveFile(byte[] buffer, string fileName)
        {
            string extention = fileName.Split('.')[1];

            bool isImage = extention.ToLower() == "png" || extention.ToLower() == "jpg" || extention.ToLower() == "jpeg";

            if (isImage)
            {
                var imageData = new UIImage(NSData.FromArray(buffer));
                imageData.SaveToPhotosAlbum((image, error) =>
                {
                    //you can retrieve the saved UI Image as well if needed using  
                    //var i = image as UIImage;  
                    if (error != null)
                    {
                        Console.WriteLine(error.ToString());
                    }
                });
            }
            else
            {
                string path = GetFilePath("Files");
                //string path = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filepath = Path.Combine(path, fileName);

                if (!File.Exists(filepath))
                {
                    File.WriteAllBytes(filepath, buffer);
                }
                
            }
            
            return fileName;
        }
    }
}