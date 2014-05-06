using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class contact : iTable
    {
        public string name;
        public string lastName;
        public string title;
        public string Suffix;
        public string address;
        public string billingAddress;
        public string shippingAddress;
        public DateTime creationDate;
        public string uid;
        public int? id;
        public int? companyID;
        public string company;

    }
}
