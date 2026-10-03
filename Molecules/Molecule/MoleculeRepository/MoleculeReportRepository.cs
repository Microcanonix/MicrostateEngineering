using IMoleculeRepository;
using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Reports;

namespace MoleculeRepository
{
    public sealed class MoleculeReportRepository(
                                      IFileServices fileServices
                                    , IDirectoryServices directoryServices
                                    , IJsonParser<MoleculeReport> jsonParser)
                                : IMoleculeReportRepository
    {
        public void Save(string directoryPath
                                , MoleculeFileName fileName
                                , MoleculeReport moleculeReport)
        {
            var filePath = Path.Combine(directoryPath, fileName.ToString() + ".json");
            directoryServices.CreateDirectory(directoryPath);
            var content = jsonParser.Serialize(moleculeReport);
            fileServices.WriteFile(filePath, content);
        }
    }
}
