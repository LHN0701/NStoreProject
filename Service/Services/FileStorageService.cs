using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Service.Interfaces;

namespace Service.Services
{
    public class FileStorageService : IStorage
    {
        private readonly string _userContentFolder;
        private readonly string _deleteUserContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "wwwroot\\Picture\\Products";

        public FileStorageService(IHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.ContentRootPath, USER_CONTENT_FOLDER_NAME);
            _deleteUserContentFolder = webHostEnvironment.ContentRootPath;
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = _deleteUserContentFolder + "\\wwwroot" + fileName.Replace("/", "\\");
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                System.IO.File.Delete(filePath);
            }

            //var filePath = Path.Combine(_userContentFolder, fileName);
            //if (File.Exists(filePath))
            //{
            //await Task.Run(() => File.Delete(filePath));
            //    System.IO.File.Delete(filePath);
            //}
        }
    }
}