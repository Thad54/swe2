using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWE_UI;

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
    }
}
