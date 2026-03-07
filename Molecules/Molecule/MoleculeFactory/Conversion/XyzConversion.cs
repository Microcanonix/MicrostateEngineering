using MoleculeDomain.Utilities;
using System.Text;

namespace MoleculeFactory.Conversion
{
    public static class XyzConversion
    {
        private static readonly string[] _returns = ["\r\n", "\r", "\n"];

        public static List<AtomPosition> ParseXyz(string xyz)
        {
            List<AtomPosition> retval = [];
            string[] lines = xyz.Split(_returns, StringSplitOptions.None);
            foreach (var line in lines)
            {
                string[] lineItems = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (lineItems.Length > 2)
                {
                    retval.Add(new AtomPosition(lineItems[0],
                                    StringConversion.ToDouble(lineItems[1]),
                                        StringConversion.ToDouble(lineItems[2]),
                                            StringConversion.ToDouble(lineItems[3])));
                }
            }
            return retval;
        }

        public static string SerializeXyz(List<AtomPosition> atomPositions)
        {
            StringBuilder retval = new();
            if (atomPositions.Count > 1)
            {
                retval.AppendLine($"{atomPositions.Count}");
                retval.AppendLine();
                foreach (var ln in atomPositions)
                {
                    retval.AppendLine($"{ln.Symbol}" +
                                        $" {StringConversion.ToString(ln.PosX)}" +
                                        $" {StringConversion.ToString(ln.PosY)}" +
                                        $" {StringConversion.ToString(ln.PosZ)}");
                }
            }
            return retval.ToString();
        }

    }
}
