﻿using System;
using SWE1_webserver_KR;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlExchange;
using accessDB;


namespace WebserverTests
{
    [TestClass]
    public class URLTest
    {
        #region Testing POST
        //Testing POST 
        [TestMethod]
        public void URL_withPOST_singleParameters()
        {

            //Arrange
            string test_Url = "https://myserver.com";
            string expected_Url = "https://myserver.com";
            string test_POSTstream = "data=test1";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", "data=test1");
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
        public void Complex_URL_withPOST_singleParameters()
        {

            //Arrange
            string test_Url = "https://myserver.com/Home/lol";
            string expected_Url = "https://myserver.com/Home/lol";
            string test_POSTstream = "data=test1";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void URL_withPOST_whitespace()
        {

            //Arrange
            string test_Url = "https://myserver.com";
            string expected_Url = "https://myserver.com";
            string test_POSTstream = "data=test 1";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void complex_URL_withPOST_whitespace()
        {

            //Arrange
            string test_Url = "https://myserver.com/test/unit";
            string expected_Url = "https://myserver.com/test/unit";
            string test_POSTstream = "data=test 1";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void URL_withPOST_mutipleParameters()
        {

            //Arrange
            string test_Url = "https://myserver.com";
            string expected_Url = "https://myserver.com";
            string test_POSTstream = "data=test 1&testing=awesome";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void complex_URL_withPOST_mutipleParameters()
        {

            //Arrange
            string test_Url = "https://myserver.com/julia/sweet/";
            string expected_Url = "https://myserver.com/julia/sweet/";
            string test_POSTstream = "data=test 1&testing=awesome";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void URL_withPOST_singleParameters_UNICODE()
        {

            //Arrange
            string test_Url = "https://myserver.com";
            string expected_Url = "https://myserver.com";
            string test_POSTstream = "data=Straße&data1=Einöde&data2=§";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        public void complex_URL_withPOST_singleParameters_UNICODE()
        {

            //Arrange
            string test_Url = "https://myserver.com/alles/easy";
            string expected_Url = "https://myserver.com/alles/easy";
            string test_POSTstream = "data=Straße&data1=Einöde&data2=§";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("Xml", test_POSTstream);
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
        #endregion

        #region Testing XML
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

        #endregion

        #region Simple URL
        //Testing Simple URL
        [TestMethod]
        public void SimpleURL()
        {
            string test_Url = "https://myserver.com";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, test_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void SimpleURL_withUNICODE()
        {
            string test_Url = "https://myserver.com/$$$/";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, test_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void notso_SimpleURL()
        {
            string test_Url = "https://myserver.com/a/b/c/d/f/Gfdgsgf";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, test_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void notso_SimpleURL_withUNICODE()
        {
            string test_Url = "https://myserver.com/öö/§";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, test_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }





        //testing GET
        [TestMethod]
        public void URL_with_single_Parameter()
        {
            string test_Url = "https://myserver.com?data=helloWorld";
            string expected_Url = "https://myserver.com";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "helloWorld");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void complex_URL_with_single_Parameter()
        {
            string test_Url = "https://myserver.com/test/?data=helloWorld";
            string expected_Url = "https://myserver.com/test/";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "helloWorld");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void URL_with_single_Parameter_with_Whitespace()
        {
            string test_Url = "https://myserver.com?data=hello%20World";
            string expected_Url = "https://myserver.com";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "hello World");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void complex_URL_with_single_Parameter_with_Whitespace()
        {
            string test_Url = "https://myserver.com/Julia/?data=hello%20World";
            string expected_Url = "https://myserver.com/Julia/";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "hello World");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void URL_with_multiple_Parameters()
        {
            string test_Url = "https://myserver.com?data=helloWorld&data2=stuff";
            string expected_Url = "https://myserver.com";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "helloWorld");
            expected_parameters.Add("data2", "stuff");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void complex_URL_with_multiple_Parameters()
        {
            string test_Url = "https://myserver.com/Hallo/1337/?data=helloWorld&data2=stuff";
            string expected_Url = "https://myserver.com/Hallo/1337/";

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("data", "helloWorld");
            expected_parameters.Add("data2", "stuff");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }

        [TestMethod]
        public void URL_with_unicode_chars()
        {
            string test_Url = "https://myserver.com?street=Wiener Straße";
            string expected_Url = "https://myserver.com";

            test_Url = System.Net.WebUtility.UrlEncode(test_Url);

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("street", "Wiener Straße");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        [TestMethod]
        public void complex_URL_with_unicode_chars()
        {
            string test_Url = "https://myserver.com/cool/stuff/?street=Wiener Straße";
            string expected_Url = "https://myserver.com/cool/stuff/";

            test_Url = System.Net.WebUtility.UrlEncode(test_Url);

            Dictionary<string, string> expected_parameters = new Dictionary<string, string>();
            expected_parameters.Add("street", "Wiener Straße");

            HttpUrl URL_Handler = new HttpUrl();
            URL_Handler.CWebURL(test_Url);

            string actual_Address = URL_Handler.WebAddress;
            Dictionary<string, string> actual_parameters = URL_Handler.WebParameters;

            Assert.AreEqual(actual_Address, expected_Url, "Address was not parsed correctly");
            CollectionAssert.AreEquivalent(actual_parameters, expected_parameters, "The parameters were not parsed correctly");

        }
        #endregion






    }
}
