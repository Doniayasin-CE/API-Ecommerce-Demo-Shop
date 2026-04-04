using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DemoShop.BLL.Service
{
    public class FileService : IFileService
    {
        public async Task<string?> UploadAsync(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                //create a secure file name from GUID and add the extension from the input file
                var fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(file.FileName);

                //create the image file path->PL->Images->fileName
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(), //PL
                    "wwwroot",
                    "Images",
                    fileName
                );

                //create the file path then copy to move the file
                using( var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

        public void DeleteAsync(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                fileName
            );
            if(File.Exists(path))
                File.Delete(path);
        }
    }
}
