namespace MoleculeDomain.Reports
{
    public record ConfigurationReportItem(string Symbol, double? Population, double? PopulationFraction)
    {
        public override string ToString()
        {
            return Symbol + $"({PopulationFraction:0.00})";
        }
    }
}
