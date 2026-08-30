using IUtilitiesServices;
using MoleculesViewer.Model;

namespace MoleculesViewer.Services
{
    public sealed class DocumentService
    {
        private readonly IFileServices _fileServices;

        private readonly IJsonParser<MoleculesDocument> _jsonParser;


        public DocumentService(IFileServices fileServices
                                , IJsonParser<MoleculesDocument> jsonParser)
        {

            _fileServices = fileServices;
            _jsonParser = jsonParser;
        }


        public MoleculesDocument LoadDocument(string documentFilePath)
        {
            try
            {
                var fileContent = _fileServices.ReadFile(documentFilePath);
                return _jsonParser.Parse(fileContent);
            }
            catch(Exception e)
            {
                throw new ArgumentException($"Invalid Document {documentFilePath}", e);
            }
        }


        public void SaveDocument(string documentFilePath, MoleculesDocument moleculesDocument )
        {
            var fileContent = _jsonParser.Serialize(moleculesDocument);
            _fileServices.WriteFile(documentFilePath, fileContent);
        }


    }
}
