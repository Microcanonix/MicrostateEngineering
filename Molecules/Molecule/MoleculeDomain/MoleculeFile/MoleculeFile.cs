namespace MoleculeDomain.MoleculeFile
{
    public abstract record MoleculeFile
    {
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
