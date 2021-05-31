using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DownloadDemo.Models
{
    public class Common
    {
        public static readonly List<Permissions.BasePermission> DownloadPermissions = new List<Permissions.BasePermission>
        {
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        /// <summary>
        /// パーミッションチェック処理
        /// </summary>
        /// <returns>権限付与フラグ(true:付与された, false:付与されなかった)</returns>
        public static async Task<bool> CheckPermissions(List<Permissions.BasePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var status = await Common.CheckAndRequestPermissionAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    // Notify user permission was denied
                    return false;
                }
            }

            return true;
        }

        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        /// <summary>
        /// 指定したURLからファイルをダウンロード
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DownloadFileAsync(string url)
        {
            var filePath = string.Empty;
            try
            {
                var webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) =>
                {
                    Console.WriteLine("INFO: DownloadFile done");
                });

                var folderPath = Xamarin.Forms.DependencyService.Get<IFileService>().GetPicturesFolderPath();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);

                    // Ensure this media doesn't show up in gallery apps
                    File.Create(Path.Combine(folderPath, ".nomedia")).Close();
                }

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                var ext = Path.GetExtension(url);
                var name = $"IMG_{timestamp}{ext}";
                filePath = GetUniquePath(folderPath, name);
                await webClient.DownloadFileTaskAsync(new Uri(url), filePath);
                Console.WriteLine($"INFO: Path= {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
            return filePath;
        }

        /// <summary>
        /// フォルダ内でユニークなファイルパスを取得
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetUniquePath(string folder, string name)
        {
            var ext = Path.GetExtension(name);
            name = Path.GetFileNameWithoutExtension(name);

            var nname = name + ext;
            var i = 1;
            while (File.Exists(Path.Combine(folder, nname)))
            {
                nname = name + "_" + (i++) + ext;
            }

            return Path.Combine(folder, nname);
        }
    }
}
