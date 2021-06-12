using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Helper
{
    public class FileUpload
    {
        public static string UPLOAD_ROOT = "wwwroot";

        [HttpPost]
        public static async Task<string> UploadFile(IFormFile file)
        {
            if (file == null) return null;

            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = Convert.ToString(Guid.NewGuid()) + fileExtension;
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_ROOT, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fullPath;
        }
    }
}
