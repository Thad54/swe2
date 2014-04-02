using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace SWE_UI
{
    class proxy
    {

        private WebRequest request;
        private Stream dataStream;
        public string Response { get; set; }
        public List<XmlExchange.contact> searchContacts(string text)
        {
            var com = new XmlExchange.command();

            com.type = "search";
            com.table = "contacts";
            com.searchText = text;

            SendString(serialize(com));

            string answer = ReceiveString();

            var xml = new StringReader(answer);

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<XmlExchange.contact>));
            var contacts = (List<XmlExchange.contact>) xs.Deserialize(xml);
        
            return contacts;
        }
        public void SendString(String input)
        {
            request = WebRequest.Create("http://localhost:8080/accessDB");
            request.Method = "POST";
            request.ContentType = "text/xml";

            byte[] data = Encoding.UTF8.GetBytes(input.ToString());
            request.ContentLength = data.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
           // this.Receive();
        
        }
        public void Receive()
        {
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            Response = reader.ReadToEnd();
            
            reader.Close();
            dataStream.Close();
            response.Close();
        }
        public string ReceiveString()
        {
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            Response = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();
            return Response;
        }

        private string serialize(XmlExchange.command com){

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(XmlExchange.command));

            var request = new StringBuilder();
            using (TextWriter writer = new StringWriter(request))
            {
                xs.Serialize(writer, com);
                return  writer.ToString();
            }
        }

    }
}
