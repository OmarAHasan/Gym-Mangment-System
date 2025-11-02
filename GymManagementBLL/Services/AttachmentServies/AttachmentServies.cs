using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentServies
{
    public class AttachmentServies : IAttachmentServies
    {
        private readonly IWebHostEnvironment _webHost;

        public AttachmentServies(IWebHostEnvironment webHost)
        {
            _webHost = webHost; 
        }



        // define array for allowed file extensions && max file size
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        long maxFileSize = 5 * 1024 * 1024; // 5 MB



        public string? Upload(string Foldername, IFormFile file)
        {
            try
            {
                // check if foldername and filename not null or empty
                if (Foldername is null || file is null || file.Length == 0) return null;

                // check if file extension is allowed
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension)) return null;

                // check if file size is within the limit
                if (file.Length > maxFileSize) return null;

                // generate folderpath that the pic will stored inside
                var folderpath = Path.Combine(_webHost.WebRootPath, "images", Foldername);
                if (!Directory.Exists(folderpath))
                    Directory.CreateDirectory(folderpath);

                // generate unique filename
                var filename = Guid.NewGuid().ToString() + fileExtension;

                // generate fullpath for the file
                var filepath = Path.Combine(folderpath, filename);

                using var stream = new FileStream(filepath, FileMode.Create);
                file.CopyTo(stream);


                return filename;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Upload File To Folder {Foldername} : {ex}");
                return null;
            }
           
        }
        public bool Delete(string Foldername, string fileName)
        {
            try
            {
                if (String.IsNullOrEmpty(Foldername) || String.IsNullOrEmpty(fileName)) return false;

                var filepath = Path.Combine(_webHost.WebRootPath, "images", Foldername, fileName);
                if (File.Exists(filepath))
                {

                    File.Delete(filepath);
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Delete File With Name {fileName} : {ex}");
                return false;
            }
        }


    }
}
