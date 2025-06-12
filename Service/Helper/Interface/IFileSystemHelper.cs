public interface IFileSystemHelper
{
    Task<bool> DoesDirectoryExist(string path);
}
