
namespace IUtilitiesServices
{
    public interface IDirectoryServices
    {
        List<string> GetFilePaths(string directoryPath, string searchPattern);

        List<string> GetDirectoryPaths(string directoryPath, string searchPattern);

        void CreateDirectory(string path);

        void DeleteDirectory(string path);

        void RenameDirectory(string path, string name);

        bool DirectoryExists(string path);

    }
}
