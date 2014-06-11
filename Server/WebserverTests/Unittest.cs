using System;
using SWE1_webserver_KR;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlExchange;
using accessDB;


namespace WebserverTests
{
     [TestClass]
    public class Unittest
    {
       
        //Testing XML

        [TestMethod]
        public void URL_withPOST_XML()
        {

            //Arrange
            string test_Url = "https://myserver.com/accessDB";
            string expected_Url = "https://myserver.com/accessDB";
            string test_POSTstream = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><verzeichnis><titel>Wikipedia Städteverzeichnis</titel><eintrag><stichwort>Genf</stichwort><eintragstext>Genf ist der Sitz von ...</eintragstext></eintrag> <eintrag> <stichwort>Köln</stichwort> <eintragstext>Köln ist eine Stadt, die ...</eintragstext> </eintrag></verzeichnis>";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><verzeichnis><titel>Wikipedia Städteverzeichnis</titel><eintrag><stichwort>Genf</stichwort><eintragstext>Genf ist der Sitz von ...</eintragstext></eintrag> <eintrag> <stichwort>Köln</stichwort> <eintragstext>Köln ist eine Stadt, die ...</eintragstext> </eintrag></verzeichnis>");
            //Act
            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);
            URL_Handler.PostParameters(test_POSTstream);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;
            //Assert
            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void accsessDB_boolcheck_confirm()
        {

            string bool_input = "/accessDB";
            bool expectet_result = true;

            accessDB.accessDB unit = new accessDB.accessDB();
            bool resutlt = unit.checkRequest(bool_input);

            Assert.AreEqual(resutlt, expectet_result, "checkRequest does not work propertly");
        }
        [TestMethod]
        public void accsessDB_boolcheck_denied()
        {

            string bool_input = "/WorngNAME";
            bool expectet_result = false;

            accessDB.accessDB unit = new accessDB.accessDB();
            bool resutlt = unit.checkRequest(bool_input);

            Assert.AreEqual(resutlt, expectet_result, "checkRequest does not work propertly");
        }
        [TestMethod]
        public void accsessDB_getnamecheck()
        {

            string expected_result = "accessDB";


            accessDB.accessDB unit = new accessDB.accessDB();


            Assert.AreEqual(expected_result, unit.getName(), "checkRequest does not work propertly");
        }
        [TestMethod]
        public void businessL_checkContactID_confirm()
        {
            //Arrange
            bool expected_result = true;
            accessDB.businessL bl = new businessL();
                    //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.id = 1;

            bool result = bl.checkContactID(contact);
            
            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
        [TestMethod]
        public void businessL_checkContactID_denied()
        {
            //Arrange
            bool expected_result = false;
            accessDB.businessL bl = new businessL();
            //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.id = null;

            bool result = bl.checkContactID(contact);

            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
        [TestMethod]
        public void businessL_checkContactname_confirm()
        {
            //Arrange
            bool expected_result = true;
            accessDB.businessL bl = new businessL();
            //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.name = "Musterman";

            bool result = bl.checkname(contact);

            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
        [TestMethod]
        public void businessL_checkContactname_denied_Emty()
        {
            //Arrange
            bool expected_result = false;
            accessDB.businessL bl = new businessL();
            //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.name = "";

            bool result = bl.checkname(contact);

            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
           [TestMethod]
        public void businessL_checkContactname_denied_null()
        {
            //Arrange
            bool expected_result = false;
            accessDB.businessL bl = new businessL();
            //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.name = null;

            bool result = bl.checkname(contact);

            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
           [TestMethod]
        public void businessL_checkContactname_denied_Whitespace()
        {
            //Arrange
            bool expected_result = false;
            accessDB.businessL bl = new businessL();
            //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.name = " ";

            bool result = bl.checkname(contact);

            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
        }
         [TestMethod]
           public void businessL_checkshippingadress_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.shippingAddress = "Mühlfeldgasse 15/3/34 1020 Wien";

               bool result = bl.checkshippingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
         public void businessL_checkshippingadress_denied_Emty()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.shippingAddress = "";

               bool result = bl.checkshippingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkshippingadress_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.shippingAddress = null;

               bool result = bl.checkshippingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkshippingadressdenied_Whitespace()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.shippingAddress = " ";

               bool result = bl.checkshippingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkaddress_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.address = "Mühlfeldgasse 15/3/34 1020 Wien";

               bool result = bl.checkaddress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkaddress_denied_Emty()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.address = "";

               bool result = bl.checkaddress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkaddress_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.address = null;

               bool result = bl.checkaddress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkaddressdenied_Whitespace()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.address = " ";

               bool result = bl.checkaddress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }

           [TestMethod]
           public void businessL_checkbillingadress_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.billingAddress = "Mühlfeldgasse 15/3/34 1020 Wien";

               bool result = bl.checkbillingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkbillingadress_denied_Emty()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.billingAddress = "";

               bool result = bl.checkbillingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkbillingadress_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.billingAddress = null;

               bool result = bl.checkbillingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkbillingadress_denied_Whitespace()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.billingAddress = " ";

               bool result = bl.checkbillingadress(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }

           [TestMethod]
           public void businessL_checkBillID_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.ID = 1;

               bool result = bl.checkBillID(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillID_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.ID = null;

               bool result = bl.checkBillID(bill);

              
               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillingDate_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.BillingDate=System.DateTime.Now;

               bool result = bl.checkBillingDate(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillingDate_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.BillingDate = null;

               bool result = bl.checkBillingDate(bill);


               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
         [TestMethod]  
         public void businessL_checkBillDueByDate_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.DueByDate = System.DateTime.Now;

               bool result = bl.checkBillDueByDate(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillDueByDate_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.DueByDate = null;

               bool result = bl.checkBillDueByDate(bill);


               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillPositioncount_confirm_one()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);

               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;
              

               bool result = bl.checkBillPositioncount(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");

           }
           [TestMethod]
           public void businessL_checkBillPositioncount_confirm_more()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
               position2.name = "position2";
               XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
               position3.name = "position3";
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);
               test.Add(position2);
               test.Add(position3);
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositioncount(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
    [TestMethod]
           public void businessL_checkBillPositioncount_denied()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL();
               //creating a fake Contact
              
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
             
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositioncount(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
     
     
     
     }
}
