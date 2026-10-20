namespace MoleculeDomain.Reports
{
    public record ConfigurationReportItem(string Symbol, double? Population)
    {
        public override string ToString()
        {
            return Symbol + $"({Population:0.0000})";
        }
    }
}
