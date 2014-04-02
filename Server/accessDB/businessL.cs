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

       public List<XmlExchange.contact> searchContacts(string text)
        {

            return _dl.searchContact(text, onlyActive: true);
        }
    }
}
