using IMoleculeServices;
using IResearchDefintionServices;
using IScientificReportServices;
using Microsoft.Extensions.Logging;
using ScientificReportDomain;

namespace ScientificReportServices
{
    public sealed class ScientificReportService(ILogger<ScientificReportService> logger,
                                                    IResearchDefinitionService researchDefintionService,
                                                        IMoleculeReportService moleculeReportService) : IScientificReportService
    {
        public ReseachDefinitionScientificReport? GetResearchDefinitionReport(string researchDefinitionName)
        {
            ReseachDefinitionScientificReport? result = null;
            var researchDef = researchDefintionService.GetMoleculesResearchDefinition(researchDefinitionName);
            if ( researchDef is not null)
            {
                result = new()
                {
                    ResearchDefinitionName = researchDef.Name
                };
                var moleculeDataFolder = Path.Combine(researchDef.PackageRoot, researchDef.Name, researchDef.MoleculeData);
                foreach (var molecule in researchDef.Molecules)
                {
                    var report = moleculeReportService.Get(moleculeDataFolder, molecule.Name);
                    if (report is not null)
                    {
                        result.MoleculeReports.Add(new MoleculeScientificReport()
                        {
                            MoleculeName = molecule.Name,
                            Report = report
                        });
                    }
                }
            }
            return result;
        }

        public List<ReseachDefinitionScientificReport> GetResearchDefinitionReports()
        {
            List<ReseachDefinitionScientificReport> result = new();
            var researchDefs = researchDefintionService.GetMoleculesResearchDefinitions();
            foreach (var researchDef in researchDefs)
            {
                ReseachDefinitionScientificReport newReport = new ()
                {
                    ResearchDefinitionName = researchDef.Name
                };

                var moleculeDataFolder = Path.Combine(researchDef.PackageRoot, researchDef.Name, researchDef.MoleculeData);
                foreach(var molecule in researchDef.Molecules)
                {
                   var report = moleculeReportService.Get(moleculeDataFolder, molecule.Name);
                   if ( report is not null)
                   {
                        newReport.MoleculeReports.Add(new MoleculeScientificReport()
                        {
                            MoleculeName = molecule.Name,
                            Report = report
                        });
                   }
                }
                result.Add(newReport);
            }
            return result;
        }
    }
}
