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

        public void setData(bill bill)
        {
            ID = bill.ID;
            BillingDate = bill.BillingDate;
            DueByDate = bill.DueByDate;
            contact = bill.contact;
            contactId = bill.contactId;
            message = bill.message;
            comment = bill.comment;
            billAmount = bill.billAmount;

            var bp = new ObservableCollection<XmlExchange.billingPosition>();

            foreach (var elem in bill.billingPositions)
            {
                bp.Add(elem);
            }

            billingPositions = bp;
        }
    }
}
