using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentServies
{
    public interface IAttachmentServies
    {
        string? Upload(string Foldername, IFormFile fileName);

        bool Delete(string Foldername,string fileName);
    }
}
