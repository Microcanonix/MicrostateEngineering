namespace MoleculeDomain.MoleculeFile
{
    public sealed record MoleculeFileXyz : MoleculeFile, IMoleculeFile<MoleculeFileXyz>
    {
        public static string Extension => ".xyz";
    }
}
