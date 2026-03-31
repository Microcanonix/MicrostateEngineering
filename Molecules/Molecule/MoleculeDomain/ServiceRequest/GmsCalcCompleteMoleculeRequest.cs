namespace MoleculeDomain.ServiceRequest
{
    public sealed class GmsCalcCompleteMoleculeRequest
    {
        public required string MoleculeDataFileDirectory { get; set; }

        public required string GmsOutputFileDirectory { get; set; }

        public required string MoleculeName { get; set; }
    }

    // UpdateMoleculeFromGmsOutputs
}
