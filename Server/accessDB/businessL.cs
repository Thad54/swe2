using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accessDB
{
    public class businessL
    {
        private DAL _dl;

        public businessL(bool MockDAL)
        {
            if (MockDAL)
            {
                _dl = new mockDB();
            }
            else
            {
                _dl = new dataAccessL();
            }

            
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

        public XmlExchange.message editBill(XmlExchange.bill bill)
        {
            return _dl.editBill(bill);
        }
        public XmlExchange.message addBill(XmlExchange.bill bill)
        {
            return _dl.addBill(bill);
        }
        public XmlExchange.message addContact(XmlExchange.contact contact)
        {
            return _dl.addContact(contact);
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

        //checkbillcontactID
        //checkbilltax
        public bool checkBillContactID(XmlExchange.bill bill)
        {
            if (bill.contactId != null)
            {
                return true;
            }
            else
            {
                return false;
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
            int count = 0;
            int emty = 0;
                
            foreach (XmlExchange.billingPosition zeile in bill.billingPositions)
            {
                emty++;
                if (zeile.amount == null)
                { count++; }
                else if (zeile.price == null)
                { count++; }
                else if(zeile.tax ==null)
                { count++; }
                
            }
            if (count > 0|| emty==0)
            { return false; }
            else
            { return true; }
        }
        public bool checkBill(XmlExchange.bill bill)
        {
            if (checkBillContactID(bill)==true && checkBillDueByDate(bill) == true && checkBillID(bill) == true && checkBillingDate(bill) == true && checkBillPositioncount(bill) == true && checkBillPositions(bill) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkContact(XmlExchange.contact contact)
        {
            if (checkaddress(contact)==true && checkbillingadress(contact)==true && checkContactID(contact)==true && checkname(contact)==true&&checkshippingadress(contact)==true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
