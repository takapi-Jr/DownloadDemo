using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DownloadDemo.Droid;
using DownloadDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace DownloadDemo.Droid
{
    public class FileService : IFileService
    {
        public string GetPicturesFolderPath()
        {
            // ファイルが作成されても、ユーザーがファイルマネージャーからアクセス不可
            // /data/user/0/PACKAGE_NAME/files
            //var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            // /data/user/0/PACKAGE_NAME/files/Pictures
            //var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);

            // ファイルマネージャーからアクセス可能
            // /storage/emulated/0/Pictures
            //var folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;

            // /storage/emulated/0/Android/data/PACKAGE_NAME/files/Pictures
            var context = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity as Context;
            var folder = context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path;

            return folder;
        }
    }
}