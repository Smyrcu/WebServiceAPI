using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EAN { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int StockQty { get; set; }
        public bool InStock { get; set; }
        public int Category { get; set; }
        public string Type{ get; set; }

    }
}
