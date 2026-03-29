using CommonDomain;
using IMoleculeProcessServices;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeProcessDomain;

namespace MoleculeProcessService
{
    public sealed class MoleculeProcessService : IMoleculeProcessService
    {
        private readonly ILogger<MoleculeProcessService> _logger;

        private readonly IMoleculeService _moleculeService;

        private readonly IGmsInputService _gmsInputService;

        public MoleculeProcessService(ILogger<MoleculeProcessService> logger,
                                        IMoleculeService moleculeService,
                                            IGmsInputService gmsInputService )
        {
            _logger = logger;
            _moleculeService = moleculeService;
            _gmsInputService = gmsInputService;
        }

        public async Task<MoleculeGmsResult> HandleImportData(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleGeometryOptimization(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleElectronicStructure(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleFukui(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleChelpGCharge(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleGeoDiskCharge(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }
    }
}
