namespace MoleculeDomain.MoleculeFile
{
    public sealed record MoleculeFileGmsInput : MoleculeFile, IMoleculeFile<MoleculeFileGmsInput>
    {
        public static string Extension => ".inp";
    }
}
