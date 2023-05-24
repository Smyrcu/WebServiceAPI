using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class CustomerOrder
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalNumber { get; set; }
        public string ShippingCountry { get; set;}
        public string ShippingAdress { get; set;}
        public int PaymentId { get; set; }
        public int ShippingMethodId { get; set; }
        public double Price { get; set;}

    }
}
