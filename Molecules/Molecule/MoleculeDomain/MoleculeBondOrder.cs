using System.Text.Json.Serialization;

namespace MoleculeDomain
{
    public class MoleculeBondOrder
    {
        private const double BondThreshold = 0.1;

        private const double BondOrder2Threshold = 1.0;

        private const double BondOrder3Threshold = 2.0;

        private const char SingleBondSymbol = '-';

        private const char DoubleBondSymbol = '=';

        private const char TripleBondSymbol = '≡';

        public double? BondOrder { get; set; }

        public double? BondOrderMinus1 { get; set; }

        public double? BondOrderPlus1 { get; set; }

        [JsonIgnore]
        public double? BondOrderHOMO => BondOrder - BondOrderMinus1;

        [JsonIgnore]
        public double? BondOrderLUMO => BondOrderPlus1 - BondOrder;

        [JsonIgnore]
        public string BondSymbol
        {
            get
            {
                if (BondOrder > BondOrder3Threshold)
                {
                    return TripleBondSymbol.ToString();
                }
                else if (BondOrder > BondOrder2Threshold)
                {
                    return DoubleBondSymbol.ToString();
                }
                else if (BondOrder > BondThreshold)
                {
                    return SingleBondSymbol.ToString();
                }
                else
                {
                    return "..";
                }
            }
        }
    }
}
