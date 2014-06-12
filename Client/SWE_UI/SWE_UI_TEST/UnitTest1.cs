using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWE_UI;
using SWE_UI.ViewModel;

namespace SWE_UI_TEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string test_response = "HTTP/1.1 200 OK Server: Apache/1.3.29 (Unix) PHP/4.3.4 Content-Length: (Größe von infotext.html in Byte) Content-Language: de (nach RFC 3282 sowie RFC 1766) Connection: close Content-Type: text/html  <?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><verzeichnis><titel>Wikipedia Städteverzeichnis</titel><eintrag><stichwort>Genf</stichwort><eintragstext>Genf ist der Sitz von ...</eintragstext></eintrag> <eintrag> <stichwort>Köln</stichwort> <eintragstext>Köln ist eine Stadt, die ...</eintragstext> </eintrag></verzeichnis>";

            string expected_parameters = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><verzeichnis><titel>Wikipedia Städteverzeichnis</titel><eintrag><stichwort>Genf</stichwort><eintragstext>Genf ist der Sitz von ...</eintragstext></eintrag> <eintrag> <stichwort>Köln</stichwort> <eintragstext>Köln ist eine Stadt, die ...</eintragstext> </eintrag></verzeichnis>";

            string actual_parameters = "";


            proxy proxy = new proxy();

            actual_parameters = proxy.trimXML(test_response);
            Assert.AreEqual(actual_parameters, expected_parameters, "Address was not parsed correctly");
        }

        [TestMethod]
        public void check_NewContact_Clearable()
        {
            var vm = new ViewModel();
            string obj = "";

            Assert.IsTrue(vm.ClearContactClicked.CanExecute(obj));

            //Assert.AreEqual(actual_parameters, expected_parameters, "Address was not parsed correctly");
        }

        [TestMethod]
        public void check_InsertContact_withoutAddressess_not_possible()
        {
            var vm = new ViewModel();
            string obj = "";

            vm.FirstName_Edit = "Test";
            vm.LastName_Edit = "Test2";
            Assert.IsFalse(vm.EditContactClicked.CanExecute(obj));

            //Assert.AreEqual(actual_parameters, expected_parameters, "Address was not parsed correctly");
        }

        [TestMethod]
        public void check_bill_ByAmount_From()
        {
            var vm = new ViewModel();
            string obj = "";

            vm.BillingAmountFrom = 100;
            Assert.IsTrue(vm.SearchBillClicked.CanExecute(obj));

            //Assert.AreEqual(actual_parameters, expected_parameters, "Address was not parsed correctly");
        }

    }
}
