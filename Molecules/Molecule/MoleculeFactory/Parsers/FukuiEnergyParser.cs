using MoleculeFactory.Conversion;

namespace MoleculeFactory.Parsers
{
    internal abstract class FukuiEnergyParser
    {
        #region tags

        protected abstract string GetStartTag();

        protected abstract string GetEnergyTag();

        #endregion

        public double Parse(List<string> input)
        {
            var retval = 0.0;
            bool startProcessing = false;
            for (int c = 0; c < input.Count; ++c)
            {
                var line = input[c];

                if (line.Contains(GetStartTag()))
                {
                    startProcessing = true;
                }

                if (startProcessing && line.Contains(GetStartTag()))
                {
                    var results = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (results.Length == 2)
                    {
                        retval = StringConversion.ToDouble(results[1].Trim());
                    }
                }
            }
            return retval;
        }
    }
}
