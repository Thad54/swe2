using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    class mockDB
    {
        public List<XmlExchange.bill> searchBill(XmlExchange.command com, bool onlyActive)
        {
            var list = new List<XmlExchange.bill>();
 
            return list;
        }


        public List<XmlExchange.contact> searchPerson(XmlExchange.contact contact, bool onlyActive)
        {
            var list = new List<XmlExchange.contact>();

            return list;
        }

        public XmlExchange.message editContact(XmlExchange.contact contact)
        {
            var message = new XmlExchange.message();
            message.error = false;
            message.text = string.Empty;
            return message;
        }

        public XmlExchange.message editBill(XmlExchange.bill bill)
        {
            var message = new XmlExchange.message();
            message.error = false;
            message.text = string.Empty;
            return message;
        }
    }
}
