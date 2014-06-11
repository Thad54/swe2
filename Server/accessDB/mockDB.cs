using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    class mockDB :  DAL
    {
        public override List<XmlExchange.bill> searchBill(XmlExchange.command com, bool onlyActive)
        {
            var list = new List<XmlExchange.bill>();
 
            return list;
        }


        public override List<XmlExchange.contact> searchPerson(XmlExchange.contact contact, bool onlyActive)
        {
            var list = new List<XmlExchange.contact>();

            return list;
        }

        public override XmlExchange.message editContact(XmlExchange.contact contact)
        {
            var message = new XmlExchange.message();
            message.error = false;
            message.text = string.Empty;
            return message;
        }

        public override XmlExchange.message editBill(XmlExchange.bill bill)
        {
            var message = new XmlExchange.message();
            message.error = false;
            message.text = string.Empty;
            return message;
        }

        public override XmlExchange.message addBill(XmlExchange.bill bill)
        {

            var message = new XmlExchange.message();
            return message;
        }

        public override XmlExchange.message addContact(XmlExchange.contact contact)
        {
            var message = new XmlExchange.message();
            return message;
        }
    }
}
