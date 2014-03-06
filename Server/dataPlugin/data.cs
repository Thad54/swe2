using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_webserver_KR;
using System.IO;

namespace dataPlugin
{
    public class dataPlugin : iPlugin
    {
        private string URLfile;
        public string getName()
        {
            return "";
        }
        public bool checkRequest(string input)
        {

            string[] url = input.Split('/');
            if (url[1].Equals("data") || url[1].Equals(""))
            {
                //URLfile = url[2];
                return true;
             
            }
            else
            {
                return false;
            }
        }

        public void handleRequest(Dictionary<string, string> data, StreamWriter OutPutStream)
        { 
     

            //with parameters
            if(data.Count>0)
            
            {
                string filename = "";

                foreach (KeyValuePair<string, string> entry in data)
                {
                    if (entry.Key == "file")
                    {
                        filename = entry.Value;
                                           }
                }
                if (filename == "")
                {
               //     filename = URLfile;
                }
                byte[] file;
                filename = "../../DATA/" + filename;
                FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

                file = new byte[fileStream.Length];

                fileStream.Read(file, 0, Convert.ToInt32(fileStream.Length));

               
               int ContentLength = file.Length;

                string[] fileparts = filename.Split('.');
                string ContentType = "";
                if (fileparts[fileparts.Length - 1] == "jpeg" || fileparts[fileparts.Length - 1] == "jpg")
                {
                    //jpeg
                    ContentType = "image/jpeg";
                }
                else if (fileparts[fileparts.Length - 1] == "png")
                {
                    //png
                  ContentType = "image/png";
                }
                else if (fileparts[fileparts.Length - 1] == "gif")
                {
                    //gif
                   ContentType = "image/gif";
                }
                else if (fileparts[fileparts.Length - 1] == "html" || fileparts[fileparts.Length - 1] == "htm" || fileparts[fileparts.Length - 1] == "xhtml")
                {
                    //html
                    ContentType = "text/html";
                }
                else if (fileparts[fileparts.Length - 1] == "xml")
                {
                    //xml
                    ContentType = "text/xml";
                }
                else if (fileparts[fileparts.Length - 1] == "txt" || fileparts[fileparts.Length - 1] == "ini" || fileparts[fileparts.Length - 1] == "config")
                {
                    //rawtext
                    ContentType = "text/plain";
                }
                else
                {
                    //octet-stream
                    string ContentDisposition = filename;
                    ContentType = "application/octet-stream";
                }


                OutPutStream.WriteLine("HTTP/1.0 200 OK");
                OutPutStream.WriteLine("Content-Type: "+ContentType);
                OutPutStream.WriteLine("Content-Length: " + ContentLength);
                OutPutStream.WriteLine("Connection: close");
                OutPutStream.WriteLine("");
                OutPutStream.Flush();
                OutPutStream.BaseStream.Write(file,0,file.Length);
            }
             
                //no Parameters

            else{
            string path = "../../DATA";
            String sResponse = "";
            string[] filePaths = Directory.GetFiles(path);
            string sFiles = "";
            sResponse = @"
                            <html>
                                <head>
                                    <title>DATA by Kernjak und Rechberger</title>
		                            </head>
                                <body>
                                    <h1>Static File Plugin</h1>
                                    <p><i>Folgende Dateien stehen zum Download bereit:</i></p>";
            foreach (var buffer in filePaths)
            {
                sFiles = buffer.Substring(buffer.LastIndexOf("\\")).Remove(0, 1);
                sResponse += "<p><a href='?file=" + sFiles + "'>" + sFiles + @"</a></p>";
            }
            sResponse += @"   
                                </body>
                            </html>";
          
            
            /* Stream fs = File.Open("../../index.html", FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);
            byte[] bytes = new byte[fs.Length];
            int read;
           
            int iTotBytes = 0;
            while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Read from the file and write the data to the network
                sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);

                iTotBytes = iTotBytes + read;

            }
            reader.Close();
            fs.Close();*/


            //  writeSuccess("text/html", bytes.Length);
            //OutPutStream.Write(sResponse);
//            return "d" + size.ToString() + "ö" + sResponse;

            OutPutStream.WriteLine("HTTP/1.0 200 OK");
            OutPutStream.WriteLine("Content-Type: text/html");
          //  OutPutStream.WriteLine("Content-Length: " + Convert.ToString(bytes.Length));
            OutPutStream.WriteLine("Connection: close");
            OutPutStream.WriteLine("");
            OutPutStream.WriteLine(sResponse);

//            OutPutStream.WriteLine("<h2>Data Plugin</h2>");
            }
            return;
        }
    }
}