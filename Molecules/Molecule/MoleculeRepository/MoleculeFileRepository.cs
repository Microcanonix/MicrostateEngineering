using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;

namespace MoleculeRepository
{
    public abstract class MoleculeFileRepository<TFile>
        where TFile : MoleculeFile, IMoleculeFile<TFile>, new()
    {
        private readonly IFileServices _fileServices;
        private readonly IDirectoryServices _directoryServices;

        public static string Extension => TFile.Extension;

        protected MoleculeFileRepository(IFileServices fileServices, IDirectoryServices directoryServices)
        {
            _fileServices = fileServices;
            _directoryServices = directoryServices;
        }

        protected TFile GetMoleculeFile(string directoryPath, MoleculeFileName name)
        {
            var filePath = Path.Combine(directoryPath, name.ToString() + Extension);
            if (_fileServices.FileExists(filePath))
            {
                return new TFile
                {
                    Name =  name,
                    Content = _fileServices.ReadFile(filePath)
                };
            }
            return new TFile { Name = name };
        }

        protected List<TFile> GetMoleculeFiles(string directoryPath)
        {
            List<TFile> result = new List<TFile>();
            var filesPaths = _directoryServices.GetFilePaths(directoryPath, $"{Extension}");
            foreach(var file in filesPaths)
            {
                result.Add(new TFile
                {
                    Name = MoleculeFileName.Parse(Path.GetFileNameWithoutExtension(file)),
                    Content = _fileServices.ReadFile(file)
                });
            }
            return result;
        }

        protected void SaveMoleculeFile(string directoryPath, TFile file)
        {
            var filePath = Path.Combine(directoryPath, file.Name?.ToString() + Extension);
            Directory.CreateDirectory(directoryPath);
            _fileServices.WriteFile(filePath, file.Content);
        }
    }
}
