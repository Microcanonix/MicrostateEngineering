using IUtilitiesServices;

namespace UtilityServices
{
    public class FileServices : IFileServices
    {
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void MoveFile(string existingPath, string newPath)
        {
            File.Move(existingPath, newPath, true);
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public async Task<string> ReadFileAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public async Task WriteFileAsync(string path, string content)
        {
            await File.WriteAllTextAsync(path, content);
        }
    }
}
