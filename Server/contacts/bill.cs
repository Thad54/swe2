using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExchange
{
    public class bill : iTable
    {
        public int? ID { get; set; }
        public DateTime? BillingDate { get; set; }
        public DateTime? DueByDate { get; set; }
        public contact contact { get; set; }
        public int? contactId { get; set; }
        public string message { get; set; }
        public string comment { get; set; }
        public decimal? billAmount { get; set; }
        public ObservableCollection<billingPosition> billingPositions;
    }
}
