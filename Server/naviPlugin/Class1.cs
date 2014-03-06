using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
//using System.Device.Location;
using SWE1_webserver_KR;

namespace naviPlugin
{
    public class naviPlugin : iPlugin
    {

        private static Mutex mutex = new Mutex();
        private static DateTime lastRefresh = default(DateTime);
        private static Dictionary<string, HashSet<string>> mapData = new Dictionary<string, HashSet<string>>();
        private static bool locked = false;
 //       private static List<Address> testdata;


        public string getName()
        {
            return "navi";
        }
        public bool checkRequest(string input)
        {
            string[] url = input.Split('/');
            if (url[1].Equals("navi"))
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

            //lastRefresh = DateTime.Now;

                if(locked == true){

                    if (lastRefresh == default(DateTime))
                    {

                        sb.Append("<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><style>#wrapper{width:39%;height:100%;background-color:grey;margin:auto;}#StreetForm{margin-left:40%;margin-bottom:1%;}#last_refresh{float:left;color: #DDDDDD;margin-left: 1%;margin-top: 1%;}#refresh{color:white;width:9em;cursor:pointer;padding-top:3%;padding-left:0.9%;}#refresh p{background-color:black;padding-left:9%;}h2{margin-left:1%;}li span{color: white;}</style><script src=\"http://code.jquery.com/jquery-latest.min.js\"        type=\"text/javascript\"></script><script>function refresh(){var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0] + \"?operation=refresh\";} else {url = url + \"?operation=refresh\";}window.location.replace(url);}function StreetSearch(e){if(e.which == 13){var street = document.getElementById(\"streetInput\").value;if(street == \"\"){alert(\"Please enter a Street Name\");return;}var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0];}url = url + \"?street=\" + street;window.location.replace(url);}}</script><title>Navi</title></head><body><div id=\"wrapper\"><p id=\"last_refresh\">Last Map Data Refresh: never</p><div id=\"refresh\" onclick=\"refresh();\"><p>Refresh Map Data</p></div><div id=\"StreetForm\" style=\"margin-left:28.45%; margin-bottom:2%;\" ><p style=\"Color:white; padding-top:0.1%; margin-left:0.1%; margin-bottom:0.5%;\">Please enter a Street</p><input id=\"streetInput\" onkeypress=\"StreetSearch(event);\" type=\"text\" style=\"width:20em; height:1.1em;\"/></div>");
                    }
                    else
                    {
                        sb.Append("<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><style>#wrapper{width:39%;height:100%;background-color:grey;margin:auto;}#StreetForm{margin-left:40%;margin-bottom:1%;}#last_refresh{float:left;color: #DDDDDD;margin-left: 1%;margin-top: 1%;}#refresh{color:white;width:9em;cursor:pointer;padding-top:3%;padding-left:0.9%;}#refresh p{background-color:black;padding-left:9%;}h2{margin-left:1%;}li span{color: white;}</style><script src=\"http://code.jquery.com/jquery-latest.min.js\"        type=\"text/javascript\"></script><script>function refresh(){var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0] + \"?operation=refresh\";} else {url = url + \"?operation=refresh\";}window.location.replace(url);}function StreetSearch(e){if(e.which == 13){var street = document.getElementById(\"streetInput\").value;if(street == \"\"){alert(\"Please enter a Street Name\");return;}var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0];}url = url + \"?street=\" + street;window.location.replace(url);}}</script><title>Navi</title></head><body><div id=\"wrapper\"><p id=\"last_refresh\">Last Map Data Refresh: ");
                        sb.Append(lastRefresh.ToString("dd.MM.yyyy HH:mm:ss"));
                        sb.Append("</p><div id=\"refresh\" onclick=\"refresh();\"><p>Refresh Map Data</p></div><div id=\"StreetForm\" style=\"margin-left:28.45%; margin-bottom:2%;\" ><p style=\"Color:white; padding-top:0.1%; margin-left:0.1%; margin-bottom:0.5%;\">Please enter a Street</p><input id=\"streetInput\" onkeypress=\"StreetSearch(event);\" type=\"text\" style=\"width:20em; height:1.1em;\"/></div>");

                    }

                    if(data.ContainsKey("operation"))
                    {
                        if(data["operation"].Equals("refresh"))
                        {
                            sb.Append("<h3>The Map Data is already being updated. Please try entering a street in a few Minutes.</h3>");
                        } else {
                            sb.Append("<h3>The Map Data is being updated. Please try entering a street in a few Minutes.</h3>");
                        }
                    } else {
                        sb.Append("<h3>The Map Data is being updated. Please try entering a street in a few Minutes.</h3>");
                    }

                } else {

                    if(lastRefresh == default(DateTime)){

                        sb.Append("<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><style>#wrapper{width:39%;height:100%;background-color:grey;margin:auto;}#StreetForm{margin-left:40%;margin-bottom:1%;}#last_refresh{float:left;color: #DDDDDD;margin-left: 1%;margin-top: 1%;}#refresh{color:white;width:9em;cursor:pointer;padding-top:3%;padding-left:0.9%;}#refresh p{background-color:black;padding-left:9%;}h2{margin-left:1%;}li span{color: white;}</style><script src=\"http://code.jquery.com/jquery-latest.min.js\"        type=\"text/javascript\"></script><script>function refresh(){var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0] + \"?operation=refresh\";} else {url = url + \"?operation=refresh\";}window.location.replace(url);}function StreetSearch(e){if(e.which == 13){var street = document.getElementById(\"streetInput\").value;if(street == \"\"){alert(\"Please enter a Street Name\");return;}var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0];}url = url + \"?street=\" + street;window.location.replace(url);}}</script><title>Navi</title></head><body><div id=\"wrapper\"><p id=\"last_refresh\">Last Map Data Refresh: never</p><div id=\"refresh\" onclick=\"refresh();\"><p>Refresh Map Data</p></div><div id=\"StreetForm\" style=\"margin-left:28.45%; margin-bottom:2%;\" ><p style=\"Color:white; padding-top:0.1%; margin-left:0.1%; margin-bottom:0.5%;\">Please enter a Street</p><input id=\"streetInput\" onkeypress=\"StreetSearch(event);\" type=\"text\" style=\"width:20em; height:1.1em;\"/></div>");
                    } else {
                        sb.Append("<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><style>#wrapper{width:39%;height:100%;background-color:grey;margin:auto;}#StreetForm{margin-left:40%;margin-bottom:1%;}#last_refresh{float:left;color: #DDDDDD;margin-left: 1%;margin-top: 1%;}#refresh{color:white;width:9em;cursor:pointer;padding-top:3%;padding-left:0.9%;}#refresh p{background-color:black;padding-left:9%;}h2{margin-left:1%;}li span{color: white;}</style><script src=\"http://code.jquery.com/jquery-latest.min.js\"        type=\"text/javascript\"></script><script>function refresh(){var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0] + \"?operation=refresh\";} else {url = url + \"?operation=refresh\";}window.location.replace(url);}function StreetSearch(e){if(e.which == 13){var street = document.getElementById(\"streetInput\").value;if(street == \"\"){alert(\"Please enter a Street Name\");return;}var url = window.location.href;if(url.indexOf(\"?\") !== -1){var parts = url.split(\"?\");url = parts[0];}url = url + \"?street=\" + street;window.location.replace(url);}}</script><title>Navi</title></head><body><div id=\"wrapper\"><p id=\"last_refresh\">Last Map Data Refresh: ");
                        sb.Append(lastRefresh.ToString("dd.MM.yyyy HH:mm:ss"));
                        sb.Append("</p><div id=\"refresh\" onclick=\"refresh();\"><p>Refresh Map Data</p></div><div id=\"StreetForm\" style=\"margin-left:28.45%; margin-bottom:2%;\" ><p style=\"Color:white; padding-top:0.1%; margin-left:0.1%; margin-bottom:0.5%;\">Please enter a Street</p><input id=\"streetInput\" onkeypress=\"StreetSearch(event);\" type=\"text\" style=\"width:20em; height:1.1em;\"/></div>");

                    }

                if(data.ContainsKey("operation"))
                {
                    if(data["operation"].Equals("refresh"))
                    {
                        /*if(locked == true){
                            sb.Append("<h3>The Map Data is already being updated. Please try entering a street in a few Minutes.</h3></div></body></html>");
                            WriteResponse(sb.ToString(), "text/html", OutPutStream);
                            return;
                        }*/

                        Thread thread = new Thread(() => loadData(mutex));
                        thread.Start();

                        sb.Append("<h3>The Map Data is being updated. Please try entering a street in a few Minutes.</h3>");
                        WriteResponse(sb.ToString(), "text/html", OutPutStream);
                        return;
     //                   OutPutStream.Flush();

        /*                mutex.Close();
                        locked = true;
                        loadData(mutex);
                        mutex.ReleaseMutex();
                        locked = false;
          */              

                    } else if(data["operation"].Equals("dump")){

                      //  mutex.WaitOne();
                        loadData();
                      //  mutex.ReleaseMutex();
                        foreach(KeyValuePair<string, HashSet<string>> entry in mapData ){
                            sb.AppendFormat("<h2>{0}</h2>",entry.Key);
                            foreach(string place in entry.Value){
                                sb.AppendFormat("<p>{0}</p>", place);
                            }
                
                        }

                    }
                } else if(data.ContainsKey("street")){

                    if(!mapData.ContainsKey(data["street"])){
                        sb.Append("<h2>The specified Street could not be found.</h2>");
                    } else {
                        sb.AppendFormat("<h2>{0}</h2><ul>",data["street"]);

                        foreach(string place in mapData[data["street"]]){
                            sb.AppendFormat("<li><span>{0}</span></li>", place);
                        }
                    }
                }
            }
            sb.Append("</div></body></html>");
            WriteResponse(sb.ToString(), "text/html", OutPutStream);
            return;
        }

        public static void loadData(Mutex semaphore)
        {
            locked = true;
            semaphore.WaitOne();
            var file = "austria-latest.osm";
            using(var fs = System.IO.File.OpenRead(file))
            {
                using(var xml = new System.Xml.XmlTextReader(fs))
                {
                    while(xml.Read())
                    {
                        if(xml.NodeType == System.Xml.XmlNodeType.Element && xml.Name == "osm")
                        {
                            parseOsm(xml);
                        }
                    }
                } // read from file
            } // open from file   
            semaphore.ReleaseMutex();
            locked = false;
            lastRefresh = DateTime.Now;
        }

        public static void loadData()
        {
            var file = "austria-latest.osm";
            using (var fs = System.IO.File.OpenRead(file))
            {
                using (var xml = new System.Xml.XmlTextReader(fs))
                {
                    while (xml.Read())
                    {
                        if (xml.NodeType == System.Xml.XmlNodeType.Element && xml.Name == "osm")
                        {
                            parseOsm(xml);
                        }
                    }
                } // read from file
            } // open from file   
            lastRefresh = DateTime.Now;
        }

        private static void parseOsm(System.Xml.XmlTextReader xml)
        {
            using(var osm = xml.ReadSubtree())
            {
                while(osm.Read())
                {
                    if(osm.NodeType == System.Xml.XmlNodeType.Element && (osm.Name == "Node" || osm.Name == "way" || osm.Name == "relation"))
                    {
                        parseElement(osm);
                    }
                }
            } // use xml subtree
        }

        private static void parseElement( System.Xml.XmlReader osm)
        {
            using(var element = osm.ReadSubtree())
            {
                string street = string.Empty;
                string city = string.Empty;
                int state = 0;

                while(element.Read())
                {
                    
                    if(element.NodeType == System.Xml.XmlNodeType.Element && element.Name == "tag")
                    {
                        //readTag(element);
                        string tag = element.GetAttribute("k");
                        string value = element.GetAttribute("v");
                        
                        

                        switch (tag)
                        {
                            case "addr:city":
                                city = value;
                                state++;
                                break;

                            case "addr:street":
                                street = value;
                                state++;
                                break;

                        }

                    }
                    if(state == 2){
                        if(!mapData.ContainsKey(street)){
                        mapData.Add(street, new HashSet<string>());
                        }
                        mapData[street].Add(city);
                    }        

                }
            } // use xml subtree
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
