using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace GymManagementSystem.BLL.Services.AttatchmentService
{
    public class AttatchmentService : IAttatchmentService
    {
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] _AllowedExtensions ={ ".jpg", ".jpeg", ".png"};
        private readonly ILogger<AttatchmentService> _logger;
        private readonly IWebHostEnvironment _env;

        public AttatchmentService(IWebHostEnvironment env, ILogger<AttatchmentService> logger) 
        {
            _logger = logger;
             _env = env;
        }
        public bool Delete(string FileName, string FolderName)
        {
            if(string.IsNullOrEmpty(FileName)||string.IsNullOrEmpty(FolderName)) return false;
            try
            {
                var FullPath = Path.Combine(_env.WebRootPath, FolderName, FileName);
                if (!File.Exists(FullPath)) return false;
                File.Delete(FullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed To Delete File {FileName}",FileName);
                return false;
            }
        }

        public (Stream stream, string ContentType)? GetFile(string FileName, string FolderName)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return null;
            var FullPath = Path.Combine(_env.WebRootPath, FolderName, FileName);
            if (!File.Exists(FullPath)) return null;
            var ContentType = Path.GetExtension(FullPath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".image" => "image/jpeg",
                _ => "application/octet-stream"
            };
            var stream = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, ContentType);
        }

        public async Task<string?> UploadAsync(Stream FileStream, string FileName,string FolderName, CancellationToken ct = default)
        {
            if (FileStream is null || !FileStream.CanRead) return null;
            if(FileStream.Length==0)return null;
            if (FileStream.Length > MaxFileSize)
            { 
             _logger.LogWarning("Rejacted Upload:{File Too Large{Size} bytes", FileStream.Length);
                return null;
            }
            var Extension = Path.GetExtension(FileName);
            if (string.IsNullOrEmpty(Extension) || !_AllowedExtensions.Contains(Extension))
            {
                _logger.LogWarning("Rejected Upload: Exetension{Not Allowed}", Extension);
                
                return null;
            }
            var UploadFolder = Path.Combine(_env.ContentRootPath, FolderName);
            Directory.CreateDirectory(UploadFolder);
            var StoredFileName = $"{Guid.NewGuid()}{Extension}";
            var FilePath = Path.Combine(UploadFolder, StoredFileName);
            try
            {
                await using var FS = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write,FileShare.None);
                await FS.CopyToAsync(FileStream);
                return StoredFileName;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Failed To Upload File {FileName}",FileName); 
                return null;
            }
        }
    }
}
