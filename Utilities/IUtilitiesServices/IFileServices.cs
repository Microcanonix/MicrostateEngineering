
namespace IUtilitiesServices
{
    public interface IFileServices
    {
        void WriteFile(string path, string content);

        Task WriteFileAsync(string path, string content);

        string ReadFile(string path);

        Task<string> ReadFileAsync(string path);

        void MoveFile(string existingPath, string newPath);

        void DeleteFile(string path);

        bool FileExists(string path);

    }
}
