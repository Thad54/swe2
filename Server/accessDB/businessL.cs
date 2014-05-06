using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    class businessL
    {
        private dataAccessL _dl;

       public businessL()
        {
            _dl = new dataAccessL();
        }

       public List<XmlExchange.contact> searchPerson(XmlExchange.contact contact)
        {
            return _dl.searchPerson(contact, onlyActive: true);
        }

        public List<XmlExchange.contact> searchCompany(XmlExchange.contact contact)
        {
            return _dl.searchCompany(contact, onlyActive: true);
        }

        public XmlExchange.message editContact(XmlExchange.contact contact){
            return _dl.editContact(contact);
        }
    }
}
