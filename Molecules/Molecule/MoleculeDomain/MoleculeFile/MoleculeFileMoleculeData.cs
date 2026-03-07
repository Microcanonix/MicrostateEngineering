namespace MoleculeDomain.MoleculeFile
{
    public sealed record MoleculeFileMoleculeData : MoleculeFile, IMoleculeFile<MoleculeFileMoleculeData>
    {
        public static string Extension => ".json";
    }
}
