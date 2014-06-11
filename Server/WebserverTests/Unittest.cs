﻿using System;
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
            accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.ID = null;

               bool result = bl.checkBillID(bill);

              
               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillContactID_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.contactId = 1;

               bool result = bl.checkBillContactID(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillContactID_denied_null()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.contactId = null;

               bool result = bl.checkBillContactID(bill);


               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkBillingDate_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
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
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
              
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
             
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositioncount(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
     
         
         [TestMethod]
           public void businessL_checkBillPositions_confirm_one()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               position1.amount = 1;
               position1.price = 3;
               position1.tax=1;
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);

               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositions(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");

           }
           [TestMethod]
         public void businessL_checkBillPositions_confirm_more()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               position1.amount = 1;
               position1.price = 3;
               position1.tax = 1;
               XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
               position2.name = "position2";
               position2.amount = 2;
               position2.price = 4;
               position2.tax = 1;
               XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
               position3.name = "position3";
               position3.amount = 3;
               position3.price = 5;
               position3.tax = 1;

               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);
               test.Add(position2);
               test.Add(position3);
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositions(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
          [TestMethod]
           public void businessL_checkBillPositions_denied_nopositions()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
              
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               
              
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBillPositions(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
          [TestMethod]
          public void businessL_checkBillPositions_denied_one_amountNULL()
    {
        //Arrange
        bool expected_result = false;
        accessDB.businessL bl = new businessL(MockDAL: false);
        //creating a fake Contact

        System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
        XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
              position1.name = "position1";
        position1.amount = null;
        position1.price = 3;
        position1.tax = 1;
        test.Add(position1);
        XmlExchange.bill bill = new XmlExchange.bill();
        bill.billingPositions = test;


        bool result = bl.checkBillPositions(bill);

        Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
    }
          [TestMethod]
          public void businessL_checkBillPositions_denied_one_priceNULL()
          {
              //Arrange
              bool expected_result = false;
              accessDB.businessL bl = new businessL(MockDAL: false);
              //creating a fake Contact

              System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
              XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
              position1.name = "position1";
              position1.amount = 4;
              position1.tax = 1;
              position1.price = null;
              test.Add(position1);
              XmlExchange.bill bill = new XmlExchange.bill();
              bill.billingPositions = test;


              bool result = bl.checkBillPositions(bill);

              Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
          }
          [TestMethod]
          public void businessL_checkBillPositions_denied_more_amountNULL()
          {
              //Arrange
              bool expected_result = false;
              accessDB.businessL bl = new businessL(MockDAL: false);
              //creating a fake Contact

              System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
              XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
              position1.name = "position1";
              position1.amount = 1;
              position1.price = 3;
              position1.tax = 1;
              XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
              position2.name = "position2";
              position2.amount = 2;
              position2.tax = 1;
              position2.price = 4;
              XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
              position3.name = "position3";
              position3.amount = null;
              position3.price = 5;
              position3.tax = 1;
              test.Add(position1);
              test.Add(position2);
              test.Add(position3);
              XmlExchange.bill bill = new XmlExchange.bill();
              bill.billingPositions = test;


              bool result = bl.checkBillPositions(bill);

              Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
          }
          [TestMethod]
          public void businessL_checkBillPositions_denied_more_priceNULL()
          {
              //Arrange
              bool expected_result = false;
              accessDB.businessL bl = new businessL(MockDAL: false);
              //creating a fake Contact

              System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
              XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
             
              position1.name = "position1";
              position1.amount = 1;
              position1.price = 3;
              position1.tax = 1;
              XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
              position2.name = "position2";
              position2.amount = 2;
              position2.price = 4;
              position2.tax = 1;
              XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
              position3.name = "position3";
              position3.amount = 3;
              position3.tax = 3;
              position3.price = null;
              test.Add(position1);
              test.Add(position2);
              test.Add(position3);
              XmlExchange.bill bill = new XmlExchange.bill();
              bill.billingPositions = test;


              bool result = bl.checkBillPositions(bill);

              Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
          }
          [TestMethod]
          public void businessL_checkBillPositions_denied_one_taxNULL()
          {
              //Arrange
              bool expected_result = false;
              accessDB.businessL bl = new businessL(MockDAL: false);
              //creating a fake Contact

              System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
              XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
              position1.name = "position1";
              position1.amount = 4;
              position1.tax = null;
              position1.price = 1;
              test.Add(position1);
              XmlExchange.bill bill = new XmlExchange.bill();
              bill.billingPositions = test;


              bool result = bl.checkBillPositions(bill);

              Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
          }
          [TestMethod]
          public void businessL_checkBillPositions_denied_more_taxNULL()
          {
              //Arrange
              bool expected_result = false;
              accessDB.businessL bl = new businessL(MockDAL: false);
              //creating a fake Contact

              System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
              XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
              position1.name = "position1";
              position1.amount = 1;
              position1.price = 3;
              position1.tax = 1;
              XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
              position2.name = "position2";
              position2.amount = 2;
              position2.tax = 1;
              position2.price = 4;
              XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
              position3.name = "position3";
              position3.amount = 1;
              position3.price = 5;
              position3.tax = null;
              test.Add(position1);
              test.Add(position2);
              test.Add(position3);
              XmlExchange.bill bill = new XmlExchange.bill();
              bill.billingPositions = test;


              bool result = bl.checkBillPositions(bill);

              Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
          }
         [TestMethod]
           public void businessL_checkBill_denied()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
              
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
             
               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;


               bool result = bl.checkBill(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
       public void businessL_checkBill_confirm()
           {
               //Arrange
               bool expected_result = true;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               position1.amount = 1;
               position1.price = 3;
               position1.tax = 1;
               XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
               position2.name = "position2";
               position2.amount = 2;
               position2.price = 4;
               position2.tax = 1;
               XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
               position3.name = "position3";
               position3.amount = 3;
               position3.price = 5;
               position3.tax = 1;
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);
               test.Add(position2);
               test.Add(position3);

               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;
               bill.BillingDate = System.DateTime.Now;
               bill.DueByDate = System.DateTime.Now;
               bill.ID = 1;
               bill.contactId = 2;



               bool result = bl.checkBill(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");

           }
           [TestMethod]
           public void businessL_checkBill_denied_IDNULL()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.billingPosition position1 = new XmlExchange.billingPosition();
               position1.name = "position1";
               position1.amount = 1;
               position1.price = 3;
               position1.tax = 1;
               XmlExchange.billingPosition position2 = new XmlExchange.billingPosition();
               position2.name = "position2";
               position2.amount = 2;
               position2.price = 4;
               position2.tax = 1;
               XmlExchange.billingPosition position3 = new XmlExchange.billingPosition();
               position3.name = "position3";
               position3.amount = 3;
               position3.price = 5;
               position3.tax = 1;
               System.Collections.ObjectModel.ObservableCollection<billingPosition> test = new System.Collections.ObjectModel.ObservableCollection<billingPosition>();
               test.Add(position1);
               test.Add(position2);
               test.Add(position3);

               XmlExchange.bill bill = new XmlExchange.bill();
               bill.billingPositions = test;
               bill.BillingDate = System.DateTime.Now;
               bill.DueByDate = System.DateTime.Now;
               bill.ID = null;
               bill.contactId = 2;



               bool result = bl.checkBill(bill);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");

           }
           [TestMethod]
           public void businessL_checkContact_denied()
           { 
           //Arrange
            bool expected_result = true;
            accessDB.businessL bl = new businessL(MockDAL: false);
                    //creating a fake Contact
            XmlExchange.contact contact = new XmlExchange.contact();
            contact.id = 1;
            contact.address = "Mühlfeldgasse 15/34 1020 Wien";
            contact.billingAddress = contact.address;
            contact.shippingAddress = contact.address;
            contact.name = "Andi";
            


            bool result = bl.checkContactID(contact);
            
            Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
           [TestMethod]
           public void businessL_checkContact_confirm()
           {
               //Arrange
               bool expected_result = false;
               accessDB.businessL bl = new businessL(MockDAL: false);
               //creating a fake Contact
               XmlExchange.contact contact = new XmlExchange.contact();
               contact.id = 1;
               contact.address = "Mühlfeldgasse 15/34 1020 Wien";
               contact.billingAddress = contact.address;
               contact.shippingAddress = null;
               contact.name = "hallo";



               bool result = bl.checkContact(contact);

               Assert.AreEqual(expected_result, result, "businessL_checkContactID_confirm");
           }
     
     
     }
}
