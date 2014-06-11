using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    public class businessL
    {
        private dataAccessL _dl;

        public businessL()
        {
            _dl = new dataAccessL();
        }

        public List<XmlExchange.bill> searchBill(XmlExchange.command com)
        {
            return _dl.searchBill(com, onlyActive: true);
        }

        public List<XmlExchange.contact> searchPerson(XmlExchange.contact contact)
        {
            return _dl.searchPerson(contact, onlyActive: true);
        }

        public List<XmlExchange.contact> searchCompany(XmlExchange.contact contact)
        {
            return _dl.searchCompany(contact, onlyActive: true);
        }

        public XmlExchange.message editContact(XmlExchange.contact contact)
        {
            return _dl.editContact(contact);
        }
        public bool checkContactID(XmlExchange.contact contact)
        {
            if (contact.id != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkaddress(XmlExchange.contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.address) == true)
            {

                return false;
            }
            else
            {
                return true;
            }

        }
        public bool checkshippingadress(XmlExchange.contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.shippingAddress) == true)
            {

                return false;
            }
            else
            {
                return true;
            }

        }
        public bool checkbillingadress(XmlExchange.contact contact)
        {


            if (string.IsNullOrWhiteSpace(contact.billingAddress) == true)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public bool checkname(XmlExchange.contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.name) == true)
            {


                return false;
            }
            else
            {
                return true;
            }

        }



        public bool checkBillID(XmlExchange.bill bill)
        {
            if (bill.ID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkBillingDate(XmlExchange.bill bill)
        {
            if (bill.BillingDate == null)
            {

                return false;
            }
            else
            {
                return true;
            }

        }
        public bool checkBillDueByDate(XmlExchange.bill bill)
        {
            if (bill.DueByDate == null)
            {

                return false;
            }
            else
            {
                return true;
            }

        }
        public bool checkBillPositioncount(XmlExchange.bill bill)
        {
            int i = 0;
            foreach (XmlExchange.billingPosition zeile in bill.billingPositions)
            { i++; }
            if (i > 0) return true;
            else return false;


        }
        public bool checkBillPositions(XmlExchange.bill bill )
        {

            foreach (XmlExchange.billingPosition zeile in bill.billingPositions)
            {
                if (zeile.amount == null)
                { return false; }
                else if (zeile.price == null)
                { return false; }
                else
                { return true; }
            }
            return false;
        }
    }
}
