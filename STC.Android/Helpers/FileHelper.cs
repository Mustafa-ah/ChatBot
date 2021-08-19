using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Xamarin.Forms;
using STC.Droid.Helpers;
using STC.Interfaces;
using Android.Provider;
using Plugin.CurrentActivity;

[assembly: Dependency(typeof(FileHelper))]
namespace STC.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public FileHelper()
        {

        }

        public string GetFilePath(string name)
        {
            //string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string libFolder = Path.Combine(docFolder, "..", name);
            //return libFolder;

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            var libFolder = Path.Combine(path, name);
            return libFolder;
        }
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        public string SaveFile(byte[] buffer,string fileName)
        {
            try
            {
                string extention = fileName.Split('.')[1];

                bool isImage = extention.ToLower() == "png" || extention.ToLower() == "jpg" || extention.ToLower() == "jpeg";

                Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
              
                if (!isImage)
                {
                     storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                }
                
               
                string path = System.IO.Path.Combine(storagePath.ToString(), fileName);
                System.IO.File.WriteAllBytes(path, buffer);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                CurrentContext.SendBroadcast(mediaScanIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fileName;
        }
        public async void CopyFile(byte[] buffer, System.IO.Stream outstream)
        {
            //var instream = new MemoryStream(buffer);
            //var buffer_ = new byte[1024];
            //int read;
            //while ((read = instream.Read(buffer)) != -1)
            //{

            //    outstream.Write(buffer, 0, read);
            //}
            try
            {
                if (buffer != null)
                    await outstream.WriteAsync(buffer, 0, buffer.Length);
                outstream.Close();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public Stream OpenStream(string path, int bufferSize)
        {
            Java.IO.File filePath = new Java.IO.File(path);
            // Check whether the file is exist in the download directory 
            if (filePath.Exists())
            {
                // Convert the file path to stream to display it into SfPdfViewer 
                return new FileStream(filePath.AbsolutePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                // throw exception if the file is not found in the appropriate directory 
                throw new FileNotFoundException("File not found" + filePath.AbsolutePath.ToString());
            }
            //return new FileStream(path.AbsolutePath, FileMode.Open, FileAccess.Read);
           // return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
        }

        public void OpenFile(string filePath,bool isRtl)
        {
            //var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //var filename1 = Path.Combine(path.ToString(), filename);
            //return filename1;
          // // var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            // string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            // var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads + "/" + filename;
        ////    File.WriteAllBytes(filePath, bytes);

            Java.IO.File file = new Java.IO.File(filePath);
            file.SetReadable(true);

            string application = "";
            string extension = Path.GetExtension(filePath);

            // get mimeTye
            switch (extension.ToLower())
            {
                case ".txt":
                    application = "text/plain";
                    break;
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }

            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);

            var ac = intent.ResolveActivity(Forms.Context.PackageManager);
            if (ac != null)
            {
                //Android.App.Application.Context
                Forms.Context.StartActivity(intent);
            }
            else
            {
                string ok = isRtl ? "موافق" : "OK";
                string msg = isRtl ? "لا يوجد تطبيق يمكنه فتح هذا الملف" : "No application available to handle this file";


                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(Android.App.Application.Context);
                //alert.SetTitle(dialogTitle);
                alert.SetMessage(msg);
                alert.SetPositiveButton(ok, (senderAlert, args) => {

                });

                //alert.SetNegativeButton(dialogNegativeBtnLabel, (senderAlert, args) => {
                //    tcs.SetResult(dialogNegativeBtnLabel);
                //});

                Dialog dialog = alert.Create();
                dialog.Show();


            }
            //Forms.Context.StartActivity(Intent.CreateChooser(intent, "Your title"));
        }


        private Stream ReadPdfStreamFromExternalStorage()
        {
            //Get the path of external storage directory. Here, we used download directory to read the PDF document 
            String path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            //Read the specific PDF document from the download directory 
            Java.IO.File filePath = new Java.IO.File(path + "/test.pdf");
            // Check whether the file is exist in the download directory 
            if (filePath.Exists())
            {
                // Convert the file path to stream to display it into SfPdfViewer 
                return new FileStream(filePath.AbsolutePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                // throw exception if the file is not found in the appropriate directory 
                throw new FileNotFoundException("File not found" + filePath.AbsolutePath.ToString());
            }
        }
    }
}