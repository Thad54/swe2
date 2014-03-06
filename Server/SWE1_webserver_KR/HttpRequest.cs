using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Folgene Namespaces werden für Sockets benötigt
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace SWE1_webserver_KR
{
    
    public class HttpRequest
    {
        private Hashtable httpHeaders = new Hashtable();
        public Hashtable getHeaders()
        {
            return httpHeaders;
        }
        public void readHeaders(string request)
        {
           
            String line;
            //  while ((line = streamReadLine(inputStream)) != null)
            while ((line = request) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                
                httpHeaders[name] = value;
                
                break;

            }
        }
        private String http_method;
        private String http_url;
        private String http_protocol_versionstring;
        HttpUrl httpurl = new HttpUrl();

        public string GetUrl()
        {
            return http_url;
        }
        public void SetUrl( string url)
        {
             http_url = url;
        }
        public string GetMethod()
        {
            return http_method;
        }
        public void parseRequest(string request)
        {
            // String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            //httpurl.GetVar(http_url, this);



            Console.WriteLine("starting: " + request);
        }
        
    }
}
