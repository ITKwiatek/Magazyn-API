using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public class FileHelper
    {
        private readonly string _dir;
        public FileHelper(string dir)
        {
            _dir = dir;
        }
        public async Task<bool> DeleteFilesInDir()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(_dir);

            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    var run = Task.Run(() => file.Delete());
                }
                catch (IOException e)
                {
                    LogService.SaveLog($"Nie udało się usunąć pliku {file.FullName}. Error: {e.Message}");
                }
                catch (Exception e)
                {
                    LogService.SaveLog($"Nie udało się usunąć pliku {file.FullName}. Error: {e.Message}");
                }
            }
            return true;
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            string filePath = Path.Combine(_dir + file.FileName);
            try
            {
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Dispose();
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return filePath;
        }

        public async Task<bool> DeleteFile(string path)
        {
            var run = Task.Run(() => File.Delete(path));

            return true;
        }
    }
}
