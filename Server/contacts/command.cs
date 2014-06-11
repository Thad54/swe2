using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class command
    {
        public string type;
        public string table;
        public string searchParameter;
        public XmlExchange.contact contact;
        public XmlExchange.bill bill;
        public int? ContactID;
        public decimal? amountFrom;
        public decimal? amountTo;
        public DateTime? from;
        public DateTime? to;
        public string searchText;
        public int column;


    }
}
