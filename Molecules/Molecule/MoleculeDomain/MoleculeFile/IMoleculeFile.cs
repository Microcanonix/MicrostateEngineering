namespace MoleculeDomain.MoleculeFile
{
    // Generic static abstract member interface to expose file extension without instance
    public interface IMoleculeFile<TSelf> where TSelf : MoleculeFile, IMoleculeFile<TSelf>
    {
        static abstract string Extension { get; }
    }
}
