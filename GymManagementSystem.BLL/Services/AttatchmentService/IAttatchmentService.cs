using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.AttatchmentService
{
    public interface IAttatchmentService
    {
        Task<string?> UploadAsync(Stream FileStream,string FileName, string FolderName, CancellationToken ct = default);
        bool Delete(string FileName,string FolderName);
        (Stream stream,string ContentType)? GetFile(string FileName,string FolderName);

    }
}
