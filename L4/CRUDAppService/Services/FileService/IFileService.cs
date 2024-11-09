using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAppService.Services
{
    public interface IFileService
    {
        Task<string> ReadFileAsync(string filePath);
        Task WriteFileAsync(string filePath, string content);
        string GetAppDataDirectory();
    }
}

