using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class CurrencyResponse
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rates> rates { get; set; }

        public class Rates
        {
            public string currerncy { get; set; }
            public string code { get; set; }
            public double mid { get; set; }
        }
    }
}
