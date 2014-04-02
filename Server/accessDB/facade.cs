﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_webserver_KR;
using System.Data.SqlClient;
using System.IO;

namespace accessDB
{
    public class accessDB : iPlugin
    {
        private businessL _bl;

        accessDB()
        {
            _bl = new businessL();
        }

        public string getName()
        {
            return "accessDB";
        }
        public bool checkRequest(string input)
        {
            string[] url = input.Split('/');
            if (url[1].Equals("db"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void handleRequest(Dictionary<string, string> data, StreamWriter OutPutStream)
        {
            var answer = new StringBuilder();
            var xml = new StringReader(data["xml"]);
         //   var xml = new System.Xml.XmlReader();

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(XmlExchange.command));
            var com = (XmlExchange.command) xs.Deserialize(xml);

            if ((com.type == "search") && (com.table == "contacts"))
            {
                var result = _bl.searchContacts(com.searchText);

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<XmlExchange.contact>));

                using (TextWriter writer = new StringWriter(answer))
                {
                    serializer.Serialize(writer, result);
                }
            }

 
             WriteResponse(answer.ToString(), "text/html", OutPutStream);
        }

        private void WriteResponse(string content, string type, StreamWriter OutPutStream)
        {
            OutPutStream.WriteLine("HTTP/1.0 200 OK");
            OutPutStream.WriteLine("Content-Type: " + type);
            OutPutStream.WriteLine("Connection: close");
            OutPutStream.WriteLine("");

            OutPutStream.WriteLine(content);
        }
    }
}
