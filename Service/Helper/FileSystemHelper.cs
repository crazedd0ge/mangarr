
namespace Service.Helper.Interface;

public class FileSystemHelper : IFileSystemHelper
{
    public async Task<bool> DoesDirectoryExist(string path)
    {
        return Directory.Exists(path); ;
    }
}
