namespace MoleculeDomain.MoleculeFile
{
    public abstract record MoleculeFile
    {
        public MoleculeFileName? Name { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
