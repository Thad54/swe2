using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class contact : iTable
    {
        public string name { get; set;}
        public string lastName { get; set; }
        public string title { get; set; }
        public string Suffix { get; set; }
        public string address { get; set; }
        public string billingAddress { get; set; }
        public string shippingAddress { get; set; }
        public DateTime creationDate { get; set; }
        public string uid { get; set; }
        public int? id { get; set; }
        public int? companyID { get; set; }
        public string company { get; set; }
        public bool isCompany { get; set; }

    }
}
