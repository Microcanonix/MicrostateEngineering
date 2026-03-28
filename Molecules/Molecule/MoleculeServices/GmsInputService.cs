using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;

namespace MoleculeServices
{
    public sealed class GmsInputService : IGmsInputService
    {

        private readonly IMoleculeService _moleculeService;

        private readonly IGmsCalcInputFactory _gmsCalcInputFactory;

        private readonly IMoleculeGmsInputRepository _moleculeGmsInputRepository;


        public GmsInputService(IMoleculeService moleculeService,
                               IGmsCalcInputFactory gmsCalcInputFactory,
                                IMoleculeGmsInputRepository moleculeGmsInputRepository)
        {
            _moleculeService = moleculeService;
            _gmsCalcInputFactory = gmsCalcInputFactory;
            _moleculeGmsInputRepository = moleculeGmsInputRepository;
        }

        public MoleculeFileGmsInput? CreateGmsInput(string gmsInputDirectory, string moleculeDirectory,
                                                            string moleculeName, CalcBasisSetCodeEnum basisSet)
        {
            var molecule = _moleculeService.GetMolecule(moleculeDirectory, moleculeName);
            if (molecule is null) return null;

            //_gmsCalcInputFactory.BuildFukuiHOMOInput(new GmsCalcInputRequest()
            //{

            //});

            return null;
        }
    }
}
