using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class OrderHistoryModel
    {
        public CustomerOrder CustomerOrder { get; set; }
        public List<CartItemModel> CartItemModel { get; set; }
    }
}
