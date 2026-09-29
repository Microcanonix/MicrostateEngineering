using CoreDomain;
using IMoleculeFactory;
using MoleculeDomain;
using MoleculeDomain.Reports;
using MoleculeFactory.Conversion;
using UtilitiesServices;

namespace MoleculeFactory
{
    public class MoleculeReportFactory : IMoleculeReportFactory
    {
        public List<MoleculeAtomPositionReport> GetAtomPositionReport(Molecule? molecule)
        {
            List<MoleculeAtomPositionReport> report = [];
            if (molecule == null) return report;
            foreach (var atom in molecule.Atoms)
            {
                MoleculeAtomPositionReport toAdd = new()
                {
                    MoleculeName = molecule.Name,
                    AtomPosition = atom.PositionInMolecule??0,
                    AtomSymbol = atom.Atom.Symbol.ToString(),
                    PosX = atom.Pos.PosX,
                    PosZ = atom.Pos.PosZ,
                    PosY = atom.Pos.PosY
                };
                report.Add(toAdd);
            }
            return report;
        }

        public List<GeneralMoleculeReport> GetGeneralMoleculeReport(Molecule? molecule)
        {
            List<GeneralMoleculeReport> report = [];
            if (molecule == null) return report;
            foreach (var atom in molecule.Atoms)
            {
                GeneralMoleculeReport toAdd = new()
                {
                    MoleculeName = molecule.Name,
                    AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                    CHelpGCharge = atom.Charge?.CHelpGCharge,
                    MullNeutral = atom.MullikenPopulation != null && atom.MullikenPopulation.Population.HasValue ? Math.Round(atom.MullikenPopulation.Population.Value, 6) : null,
                    MullLewisAcid = atom.MullikenPopulation != null && atom.MullikenPopulation.PopulationLUMO.HasValue ? Math.Round(atom.MullikenPopulation.PopulationLUMO.Value, 6) : null,
                    MullLewisBase = atom.MullikenPopulation != null && atom.MullikenPopulation.PopulationHOMO.HasValue ? Math.Round(atom.MullikenPopulation.PopulationHOMO.Value, 6) : null,
                };
                foreach (var orbitalReport in from item in atom.Orbitals orderby item.Position ascending
                                select new AtomOrbitalReport()
                                {
                                    AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                                    MoleculeName = molecule.Name,
                                    OrbitalPosition = item.Position,
                                    OrbitalSymbol = $"{item.Symbol}",
                                    Population = item.MullikenPopulation?.Population,
                                    PopulationHOMO = item.MullikenPopulation?.PopulationHOMO,
                                    PopulationLUMO = item.MullikenPopulation?.PopulationLUMO,
                                    PopulationFraction = item.MullikenPopulation?.Population / atom.MullikenPopulation?.Population,
                                    PopulationFractionHOMO = item.MullikenPopulation?.PopulationHOMO / atom.MullikenPopulation?.PopulationHOMO,
                                    PopulationFractionLUMO = item.MullikenPopulation?.PopulationLUMO / atom.MullikenPopulation?.PopulationLUMO
                                })
                {
                    toAdd.Configuration += orbitalReport.OrbitalSymbol
                        + "(" + StringConversion.ToString(orbitalReport.PopulationFraction, "0.00") + ")";
                    toAdd.ConfigurationLewisAcid += orbitalReport.OrbitalSymbol
                        + "(" + StringConversion.ToString(orbitalReport.PopulationFractionLUMO, "0.00") + ")";
                    toAdd.ConfigurationLewisBase += orbitalReport.OrbitalSymbol
                        + "(" + StringConversion.ToString(orbitalReport.PopulationFractionHOMO, "0.00") + ")";


                    toAdd.ConfigurationItems.Add(new ConfigurationReportItem(orbitalReport.OrbitalSymbol,
                                                                                orbitalReport.Population,
                                                                                    orbitalReport.PopulationFraction));

                    toAdd.ConfigurationItemsLewisBase.Add(new ConfigurationReportItem(orbitalReport.OrbitalSymbol,
                                                                                orbitalReport.PopulationHOMO,
                                                                                    orbitalReport.PopulationFractionHOMO));

                    toAdd.ConfigurationItemsLewisAcid.Add(new ConfigurationReportItem(orbitalReport.OrbitalSymbol,
                                                                                orbitalReport.PopulationLUMO,
                                                                                    orbitalReport.PopulationFractionLUMO));
                }
                report.Add(toAdd);
            }
            return report;
        }

        public List<MoleculeAtomOrbitalReport> GetMoleculeAtomOrbitalReport(Molecule? molecule)
        {
            List<MoleculeAtomOrbitalReport> report = [];
            if (molecule == null) return report;
            foreach (var atom in molecule.Atoms)
            {
                report.Add(new MoleculeAtomOrbitalReport()
                {
                    MoleculeName = molecule.Name,
                    AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                    MullikenPopulation = atom.MullikenPopulation != null && atom.MullikenPopulation.Population.HasValue ? Math.Round(atom.MullikenPopulation.Population.Value, 6) : null,
                    OrbitalReport = (from item in atom.Orbitals
                                     orderby item.Position ascending
                                     select new AtomOrbitalReport()
                                     {
                                         AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                                         MoleculeName = molecule.Name,
                                         OrbitalPosition = item.Position,
                                         OrbitalSymbol = $"{item.Symbol}",
                                         PopulationFraction = item.MullikenPopulation?.Population / atom.MullikenPopulation?.Population,
                                         PopulationFractionHOMO = item.MullikenPopulation?.PopulationHOMO / atom.MullikenPopulation?.PopulationHOMO,
                                         PopulationFractionLUMO = item.MullikenPopulation?.PopulationLUMO / atom.MullikenPopulation?.PopulationLUMO
                                     }).ToList()
                });
            }
            return report;
        }

        public List<MoleculeAtomsChargeReport> GetMoleculeAtomsChargeReport(Molecule? molecule)
        {
            List<MoleculeAtomsChargeReport> report = [];
            if (molecule == null) return report;
            foreach (var atom in molecule.Atoms)
            {
                report.Add(new MoleculeAtomsChargeReport()
                {
                    MoleculeName = molecule.Name,
                    AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                    MullikenCharge = atom.Atom.AtomNumber - atom.MullikenPopulation?.Population,
                    CHelpGHCharge = atom.Charge?.CHelpGCharge,
                    GeoDiscCharge = atom.Charge?.GeoDiscCharge,
                    LowdinCharge = atom.Atom.AtomNumber - atom.LowdinPopulation?.Population,
                });
            }
            return report;
        }

        public List<MoleculeBondsReport> GetMoleculeBondsReports(Molecule? molecule)
        {
            List<MoleculeBondsReport> report = [];
            if (molecule == null) return report;
            foreach (var bond in molecule.Bonds)
            {
                if (bond.OverlapPopulation?.Population >= MoleculesConstants.BondThreshold
                        || bond.OverlapPopulation?.PopulationHOMO >= MoleculesConstants.BondThreshold
                        || bond.OverlapPopulation?.PopulationLUMO >= MoleculesConstants.BondThreshold)
                {
                    MoleculeAtom? atom1 = molecule.Atoms.Find(a => a.PositionInMolecule == bond.Atom1Position);
                    MoleculeAtom? atom2 = molecule.Atoms.Find(a => a.PositionInMolecule == bond.Atom2Position);

                    report.Add(new MoleculeBondsReport()
                    {
                        MoleculeName = molecule.Name,
                        BondID = $"{atom1?.Atom.Symbol}{atom1?.PositionInMolecule}-{atom2?.Atom.Symbol}{atom2?.PositionInMolecule}",
                        Distance = bond.Distance,
                        BondOrder = bond.BondOrder?.BondOrder,
                        Atom1Pos = bond.Atom1Position,
                        Atom2Pos = bond.Atom2Position,
                        OverlapPopulation = bond.OverlapPopulation?.Population,
                        OverlapPopulationHOMO = bond.OverlapPopulation?.PopulationHOMO,
                        OverlapPopulationLUMO = bond.OverlapPopulation?.PopulationLUMO
                    }); ;
                }
            }
            return report;
        }

        public List<MoleculeAtomsPopulationReport> GetMoleculePopulationReport(Molecule? molecule)
        {
            List<MoleculeAtomsPopulationReport> report = [];
            if (molecule == null) return report;
            foreach (var atom in molecule.Atoms)
            {
                report.Add(new MoleculeAtomsPopulationReport()
                {
                    MoleculeName = molecule.Name,
                    AtomID = $"{atom.Atom.Symbol}{atom.PositionInMolecule}",
                    MullikenPopulation = atom.MullikenPopulation?.Population,
                    MullikenPopulationHOMO = atom.MullikenPopulation?.PopulationHOMO,
                    MullikenPopulationLUMO = atom.MullikenPopulation?.PopulationLUMO,
                    LowdinPopulation = atom.LowdinPopulation?.Population,
                    LowdinPopulationHOMO = atom.LowdinPopulation?.PopulationHOMO,
                    LowdinPopulationLUMO = atom.LowdinPopulation?.PopulationLUMO
                });
            }
            return report;
        }
    }
}
