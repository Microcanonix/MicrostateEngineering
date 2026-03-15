using CommonDomain;

namespace IMoleculeProcessServices
{
    public interface IMoleculeProcessService
    {
        Task InitilializeXyzFilesAsync(MoleculeContext context);

        Task GenerateGeoOptGmsInputAsync(MoleculeContext context);

        Task GenerateElecStructGmsInputAsync(MoleculeContext context);

        Task GenerateFukuiGmsInputAsync(MoleculeContext context);

        Task GenerateGeoDiskChargeGmsInputAsync(MoleculeContext context);

        Task GenerateChelpgChargeGmsInputAsync(MoleculeContext context);

        Task ProcessGeoOptGmsdOutputFileAsync(MoleculeContext context);

        Task ProcessElecStructGmsInputAsync(MoleculeContext context);

        Task ProcessFukuiGmsInputAsync(MoleculeContext context);

        Task ProcessGeoDiskChargeGmsInputAsync(MoleculeContext context);

        Task ProcessChelpgChargeGmsInputAsync(MoleculeContext context);

    }
}
