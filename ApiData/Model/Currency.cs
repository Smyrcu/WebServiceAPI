using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }
    }
}
