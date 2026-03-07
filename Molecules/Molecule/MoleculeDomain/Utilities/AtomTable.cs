using System.Collections.ObjectModel;

namespace MoleculeDomain.Utilities
{
    public static class AtomTable
    {
        private static readonly ReadOnlyCollection<Atom> _atomProperties =
            new([
                   new (AtomsEnum.H,"Hydrogen",1),
                   new (AtomsEnum.He,"Helium",2),
                   new (AtomsEnum.Li,"Lithium", 3),
                   new (AtomsEnum.Be,"Beryllium",4),
                   new (AtomsEnum.B,"Boron",5),
                   new (AtomsEnum.C,"Carbon",6),
                   new (AtomsEnum.N,"Nitrogen",7),
                   new (AtomsEnum.O,"Oxygen",8),
                   new (AtomsEnum.F,"Fluorine",9),
                   new (AtomsEnum.Ne,"Neon",10),
                   new (AtomsEnum.Na,"Sodium",11),
                   new (AtomsEnum.Mg,"Magnesium",12),
                   new (AtomsEnum.Al,"Aluminium",13),
                   new (AtomsEnum.Si,"Silicon",14),
                   new (AtomsEnum.P,"Phosphorus",15),
                   new (AtomsEnum.S,"Sulfur",16),
                   new (AtomsEnum.Cl,"Chlorine", 17),
                   new (AtomsEnum.Ar,"Argon", 18),
                   new (AtomsEnum.K,"Potassium",19),
                   new (AtomsEnum.Ca,"Calcium", 20),
                   new (AtomsEnum.Fe,"Iron",21)
            ]);

        public static Atom? GetAtomProperties(string symbol)
        {
            return _atomProperties.FirstOrDefault(x => x.Symbol == Enum.Parse<AtomsEnum>(symbol));
        }

        public static Atom? GetAtomProperties(AtomsEnum symbol)
        {
            return _atomProperties.FirstOrDefault(x => x.Symbol == symbol);
        }

        public static Atom? GetAtomProperties(int atomNumber)
        {
            return _atomProperties.FirstOrDefault(x => x.AtomNumber == atomNumber);
        }
    }
}
