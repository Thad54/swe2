using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
            //soapclient configuration
            //string loading = "<html>Loading Data</html>";
            //WriteResponse(loading , "text/html", OutPutStream, loading.Length);

            var binding = new BasicHttpBinding();
            binding.Name = "GlobalWeatherSoap";

            string endpointStr = "http://www.webservicex.net/globalweather.asmx";
            var endpoint = new EndpointAddress(endpointStr);

            var client = new WeatherServiceSoap.GlobalWeatherSoapClient(binding, endpoint);
            


            StringBuilder sb = new StringBuilder();
           // sb.Append("<html><body><div><h1>Weather</h1>");

           // sb.Append(client.GetCitiesByCountry("Austria"));

            //sb.Append("</div></body></html>");

            sb.Append("<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><style>#wrapper{width:39%;height:100%;background-color:grey;margin:auto;}#wrapper p{color:white;}#StreetForm{margin-left: 0;margin-top: 0%;margin-bottom: 0.5%;padding-top: 1%;}h2{margin-left:1%;}li span{color: white;}#text{margin-left:9.45%;margin-top:1%;margin-bottom:0.1%;}.subtext{margin-top:1%;margin-bottom: 0%;margin-left:20%;}</style><script src=\"http://code.jquery.com/jquery-latest.min.js\"        type=\"text/javascript\"></script><script>function sendData(e){if(e.which == 13){var country = document.getElementById(\"countryInput\").value;var city = document.getElementById(\"cityInput\").value;var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0];}if(country == \"\"){alert(\"Please enter a Country\");return;}if(city == \"\"){url = url + \"?country=\" + country;} else {url = url + \"?country=\" + country +  \"&city=\" + city;}window.location.replace(url);}}</script><title>Weather</title></head><body><div id=\"wrapper\"><div id=\"StreetForm\"><h3 id=\"text\">Enter a Country to get its weather stations. <br/> Enter a Country and a City to get the weather.</h3><p class=\"subtext\">Country:</p><input id=\"countryInput\" class=\"subtext\" onkeypress=\"sendData(event);\" type=\"text\" style=\"width:10em; height:1.1em;\"/><p class=\"subtext\">City:</p><input id=\"cityInput\" class=\"subtext\" onkeypress=\"sendData(event);\" type=\"text\" style=\"width:10em; height:1.1em;\"/></div>");

            if (data.ContainsKey("city"))
            {
                try
                {
                    string reply = client.GetWeather(data["city"], data["country"]);

                    if (reply.Equals("Data Not Found"))
                    {
                        sb.Append("<h3> Data Not Found </h3>");
                    }
                    else
                    {

                        sb.Append("<h2>");
                        sb.Append(data["country"]);
                        sb.Append("</h2>");
                        sb.Append(parseWeatherResult(reply));
                    }
                }
                catch (TimeoutException)
                {
                    sb.Append("<p>The data could not be retrieved due to a timeout. Please try again</p>");
                }
            }
            else if(data.ContainsKey("country"))
            {

                try
                {
                    string reply = client.GetCitiesByCountry(data["country"]);

                    if (reply.Equals("Data Not Found"))
                    {
                        sb.Append("<h3> Data Not Found </h3>");
                    }
                    else
                    {

                        sb.Append("<h2>");
                        sb.Append(data["country"]);
                        sb.Append("</h2>");
                        sb.Append(parseCountryResult(reply));

                    }
                }
                catch (TimeoutException)
                {
                    sb.Append("<p>The data could not be retrieved due to a timeout. Please try again</p>");
                }
            }
            sb.Append("</div></body></html>");
            WriteResponse(sb.ToString(), "text/html", OutPutStream, sb.Length);
            return;
        }

        private void WriteResponse(string content, string type, StreamWriter OutPutStream, int length)
        {
            OutPutStream.WriteLine("HTTP/1.0 200 OK");
            OutPutStream.WriteLine("Content-Type: " + type);
            OutPutStream.WriteLine("Content-Length: " + length);
            OutPutStream.WriteLine("Connection: close");
            OutPutStream.WriteLine("");

            OutPutStream.WriteLine(content);
        }

        private string parseCountryResult(string result)
        {
            StringBuilder sb = new StringBuilder();
            using(XmlReader reader = XmlReader.Create(new StringReader (result)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name.Equals("City"))
                    {
                        sb.Append("<p>");
                        sb.Append(reader.ReadElementContentAsString());
                        sb.Append("</p>");

                    }

                }
            }

            return sb.ToString();
        }

        private string parseWeatherResult(string result)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul>");
            using (XmlReader reader = XmlReader.Create(new StringReader(result)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name.Equals("CurrentWeather"))
                    {
                        reader.ReadSubtree();
                        while (reader.Read())
                        {
                            if (reader.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                sb.Append("<li>");
                                sb.Append(reader.Name.ToString());
                                sb.Append(" : ");
                                sb.Append(reader.ReadElementContentAsString());
                                sb.Append("</li>");
                            }
                        }

                    }

                }
            }
            sb.Append("</ul>");

            return sb.ToString();
        }


    }
}
