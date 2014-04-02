using System;
using System.Web;
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
    public class HttpUrl
    {
        private string _webAddress;
        private Dictionary<string, string> _webParameters = new Dictionary<string, string>();
        private int count = 0;
        public int GetCount { get { return count; } }
        public void CWebURL(string webUrl)
        {
            webUrl = WebUtility.UrlDecode(webUrl);
            string[] parts = webUrl.Split('?');
            this._webAddress = parts[0];
            if (parts.Length > 1)
            {
                createParameters(parts[1]);
            }
        }
        public void PostParameters(string stream)
        {
           
                _webParameters.Add("Xml", stream);
            
        
        }


        private void createParameters(string parameters)
        {
            string[] pairs = parameters.Split('&');
            foreach(string pair in pairs)
            {
                if(pair.Contains('=')){
                    string[] parts = pair.Split('=');
                    //parts[1] = HttpUtility.UrlPathEncode
                    //parts[1] = parts[1].Replace("%20", " ");
                    _webParameters.Add(parts[0].ToString(), parts[1].ToString());
                }
                count++;
            }
        }

        public string WebAddress
        {
            get
            {
                return this._webAddress;
            }
        }

        public Dictionary<string, string> WebParameters
        {
            get
            {
                return this._webParameters;
            }
        }
    }

        
}

