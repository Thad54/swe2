using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class bill : iTable
    {
        public int? ID;
        public DateTime? BillingDate;
        public DateTime? DueByDate;
        public contact contact;
        public int? contactId;
        public string   message;
        public string   comment;
        public List<billingPosition> billingPositions;
    }
}
