using IUtilitiesServices;

namespace UtilityServices
{
    public class DirectoryServices : IDirectoryServices
    {
        public void CreateDirectory(string path)
        {
            _ = Directory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public List<string> GetDirectoryPaths(string directoryPath, string searchPattern)
        {
            return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
        }

        public List<string> GetFilePaths(string directoryPath, string searchPattern)
        {
            return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
        }

        public void RenameDirectory(string path, string name)
        {
            Directory.Move(path, name);
        }
    }
}
