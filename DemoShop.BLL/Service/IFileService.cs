using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.BLL.Service
{
    public interface IFileService
    {
        Task<string?> UploadAsync(IFormFile file);
        void DeleteAsync(string fileName);
    }
}
