using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    public class DAL
    {
        public virtual List<XmlExchange.bill> searchBill(XmlExchange.command com, bool onlyActive) { return null; }
        public virtual XmlExchange.message editContact(XmlExchange.contact contact) { return null; }
        public virtual XmlExchange.message editBill(XmlExchange.bill bill) { return null; }
        public virtual XmlExchange.message addBill(XmlExchange.bill bill) { return null; }
        public virtual XmlExchange.message addContact(XmlExchange.contact contact) { return null; }
        public virtual List<XmlExchange.contact> searchPerson(XmlExchange.contact contact, bool onlyActive) { return null; }
        public virtual List<XmlExchange.contact> searchCompany(XmlExchange.contact contact, bool onlyActive) { return null; }
    }
}
