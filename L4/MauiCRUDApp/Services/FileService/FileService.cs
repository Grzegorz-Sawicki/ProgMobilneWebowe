using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUDAppService.Services;
using System.IO;
using System.Threading.Tasks;

public class FileService : IFileService
{
    public string GetAppDataDirectory() => FileSystem.AppDataDirectory;

    public async Task<string> ReadFileAsync(string filePath)
    {
        var fullPath = Path.Combine(FileSystem.AppDataDirectory, filePath);
        if (File.Exists(fullPath))
        {
            return await File.ReadAllTextAsync(fullPath);
        }
        return string.Empty;
    }

    public async Task WriteFileAsync(string filePath, string content)
    {
        var fullPath = Path.Combine(FileSystem.AppDataDirectory, filePath);
        await File.WriteAllTextAsync(fullPath, content);
    }
}
