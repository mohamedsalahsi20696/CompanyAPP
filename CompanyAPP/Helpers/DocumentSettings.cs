using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CompanyAPP.Helpers
{
    public class DocumentSettings
    {
        public static string uploadImage(IFormFile file, string folderName)
        {
            // 1) get located folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2) get file name and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3) get file path 
            string filePath = Path.Combine(folderPath, fileName);

            // 4) make stream
            var fs = new FileStream(filePath, FileMode.CreateNew);

            file.CopyTo(fs);

            return fileName;
        }

        public static void DeleteImage(string fileName, string folderName)
        {
            if (fileName is not null && folderName is not null)
            {

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }




        }
    }
}
