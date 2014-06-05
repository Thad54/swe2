using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SWE_UI
{
    public class proxy
    {

        private WebRequest request;
        private Stream dataStream;
        public string Response { get; set; }
        public List<XmlExchange.contact> searchPerson(string firstName, string lastName)
        {
            var com = new XmlExchange.command();
            var contact = new XmlExchange.contact();

            contact.name = firstName;
            contact.lastName = lastName;

            com.type = "search";
            com.table = "person";
            com.contact = contact;

            SendString(serialize(com));
            string answer = ReceiveString();

            var xml = new StringReader(answer);

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<XmlExchange.contact>));
            var contacts = (List<XmlExchange.contact>) xs.Deserialize(xml);
        
            return contacts;
        }

        public List<XmlExchange.contact> searchCompany(string companyName, string uid)
        {
            var com = new XmlExchange.command();
            var contact = new XmlExchange.contact();

            contact.company = companyName;
            contact.uid = uid;

            com.type = "search";
            com.table = "company";
            com.contact = contact;

            SendString(serialize(com));
            string answer = ReceiveString();

            var xml = new StringReader(answer);

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<XmlExchange.contact>));
            var contacts = (List<XmlExchange.contact>)xs.Deserialize(xml);

            return contacts;
        }

        public XmlExchange.message EditContact(XmlExchange.contact contact)
        {
            var com = new XmlExchange.command();
            com.type = "edit";
            com.table = "contacts";
            com.contact = contact;

            SendString(serialize(com));
            string answer = ReceiveString();

            var xml = new StringReader(answer);

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(XmlExchange.message));
            var message = (XmlExchange.message)xs.Deserialize(xml);

            return message;


        }
        public void SendString(String input)
        {
            request = WebRequest.Create("http://localhost:8080/accessDB");
//            request = WebRequest.Create(ConfigurationManager.AppSettings["SERVER"].ToString());
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

            Response = trimXML(Response);
            return Response;
        }
        public string trimXML(String totrim)
        {
            return totrim.Substring(totrim.IndexOf('<'));
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
