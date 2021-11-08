using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace NiqabCommonLibrary
{
    public class FileProcessor
    {
       
        public Dictionary<string, string> StoreImage(IFormFileCollection files, IHostingEnvironment hostingEnvironment, string folderName, string userName)
        {
            var fileMapping = new Dictionary<string, string>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Getting Filename
                    var fileName = file.FileName;
                    // Getting Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // Concating filename + fileExtension (unique filename)
                    var newFileName = string.Concat(userName, fileExtension);
                    //  Generating Path to store photo 
                    var folderPath = Path.Combine(hostingEnvironment.ContentRootPath, folderName);
                    var filePath = Path.Combine(hostingEnvironment.ContentRootPath, folderName) + $@"\{newFileName}";
                    fileMapping.Add("FolderPath", folderPath);
                    fileMapping.Add("FilePath", filePath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        // Storing Image in Folder
                        StoreInFolder(file, filePath);
                    }
                }
            }
            return fileMapping;
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
        public void RemoveFromFolder(string filePath)
        {
             File.Delete(filePath);
        }
    }
}
