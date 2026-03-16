using CommonDomain;
using IMoleculeProcessServices;
using IMoleculeServices;
using Microsoft.Extensions.Logging;

namespace MoleculeProcessService
{
    public sealed class MoleculeProcessService : IMoleculeProcessService
    {

        private readonly ILogger<MoleculeProcessService> _logger;

        private readonly IMoleculeService _moleculeService;


        public MoleculeProcessService(ILogger<MoleculeProcessService> logger,
                                        IMoleculeService moleculeService )
        {
            _logger = logger;
            _moleculeService = moleculeService;
        }


        public Task InitilializeXyzFilesAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }



        public Task GenerateChelpgChargeGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task GenerateElecStructGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task GenerateFukuiGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task GenerateGeoDiskChargeGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task GenerateGeoOptGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }



        public Task ProcessChelpgChargeGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcessElecStructGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcessFukuiGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcessGeoDiskChargeGmsInputAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcessGeoOptGmsdOutputFileAsync(MoleculeContext context)
        {
            throw new NotImplementedException();
        }
    }
}
