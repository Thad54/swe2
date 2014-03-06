using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using SWE1_webserver_KR;

namespace weatherPlugin
{
    public class weatherPlugin : iPlugin
    {

        public string getName()
        {
            return "weather";
        }
        public bool checkRequest(string input)
        {
            string[] url = input.Split('/');
            if (url[1].Equals("weather"))
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
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body><div><h1>Weather</h1>");

            ClassLibrary2.weatherService.GlobalWeatherSoapClient client = new ClassLibrary2.weatherService.GlobalWeatherSoapClient();

            var result = from ThisData in client.GetCitiesByCountry("Austria") select ThisData;

            sb.Append("</div></body></html>");
            WriteResponse(sb.ToString(), "text/html", OutPutStream);
            return;
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
