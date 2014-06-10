using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class billingPosition
    {
        public string name { get; set; }
        public decimal? price { get; set; }
        public int? amount { get; set; }
        public decimal? tax { get; set; }
    }
}
