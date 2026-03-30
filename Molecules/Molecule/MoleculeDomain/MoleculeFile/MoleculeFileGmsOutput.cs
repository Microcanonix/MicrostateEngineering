namespace MoleculeDomain.MoleculeFile
{
    public sealed record MoleculeFileGmsOutput : MoleculeFile , IMoleculeFile<MoleculeFileGmsOutput>
    {
        private static readonly string[] _returns = ["\r\n", "\r", "\n"];

        public static string Extension => $".log";

        public List<string> GetLines() => Content.Split(_returns, StringSplitOptions.None).ToList();
    }
}
