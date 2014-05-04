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
using SWE1_webserver_KR;

namespace SWE1_webserver_KR
{
  
        public class ResponseProcessor
       {
           private TcpClient socket;
           private Server.MyHttpServer srv;
           private pluginM plugins;

           private Stream inputStream;
           private StreamReader reader;
           private StreamWriter outputStream;
           public StreamWriter OutPutStream { get { return outputStream; } }
           HttpUrl hurl = new HttpUrl();
           HttpRequest hr = new HttpRequest();
           Dictionary<string, string> data;

           public string GetUrl()
           {
               return hr.GetUrl();
           }

           private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB
            
           internal ResponseProcessor (TcpClient s, Server.MyHttpServer lol, pluginM plugin)
           {
               this.socket = s;
               this.srv = lol;
               this.plugins = plugin;
           }


           private string streamReadLine(StreamReader reader)
           {
               int next_char;
               string data = "";
     //          MemoryStream memStream = new MemoryStream();
               while (true)   
               {
                   next_char = inputStream.ReadByte();
                   if (next_char == '\n') { break; }
                   if (next_char == '\r') { continue; }
                   if (next_char == -1) { Thread.Sleep(1); continue; };
                   data += Convert.ToChar(next_char);
               }
               return data;
           }
           public void process()
           {
               // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
               // "processed" view of the world, and we want the data raw after the headers
             
               inputStream = new BufferedStream(socket.GetStream());
               reader = new StreamReader(inputStream, Encoding.UTF8);

               // we probably shouldn't be using a streamwriter for all output from handlers either
               outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
               try
               {
                   hr.parseRequest(reader.ReadLine());
                   bool trigger = true;
                   while (trigger)
                   {
                       string input = reader.ReadLine();
                       if (input == "")
                       { break; }
                       else 
                       {
                           hr.readHeaders(input);
                       }
                   }
                   if (hr.GetMethod().Equals("GET"))
                   {
                       handleGETRequest();
                   }
                   else if (hr.GetMethod().Equals("POST"))
                   {
                       handlePOSTRequest();
                   }
               }
               catch (Exception e)
               {
                   MessageBox.Show(e.Message);
               }
               outputStream.Flush();
               // bs.Flush(); // flush any remaining output
               inputStream.Flush();
               inputStream = null;  // bs = null;
               reader = null;
               outputStream = null; // bs = null;            
               socket.Close();
           }

           private void handleGETRequest()
           {

               hurl.CWebURL(GetUrl());
               data = hurl.WebParameters;
               string url = hurl.WebAddress;

               if (url.Equals("/impressum"))
               {
                   writeSuccess("file/html");
                   Stream fs = File.Open("../../index.html", FileMode.Open);
                   /* BinaryReader reader = new BinaryReader(fs);
                    byte[] bytes = new byte[fs.Length];
                    int read;
                    String sResponse = "";
                    int iTotBytes = 0;
                    while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Read from the file and write the data to the network
                        sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);

                        iTotBytes = iTotBytes + read;

                    }
                    reader.Close();
                    fs.Close();
                    OutPutStream.Write(bytes);*/

                   fs.CopyTo(OutPutStream.BaseStream);
                   OutPutStream.BaseStream.Flush();
               }
               else
               {
                   
                   Console.WriteLine("request: {0}", url);
                  // writeSuccess("text/xml");

       //            pluginM plugins = new pluginM();
    //               plugins.loadPlugins();

                   List<string> names = plugins.getNames();

                   plugins.handleRequest(url, data, OutPutStream);
                   /*if(response[0] == 'x'){
                       response = response.Substring(1, response.Length - 1);
                       writeSuccess("text/xml");
                   } else {
                       writeSuccess();
                   }
                   OutPutStream.WriteLine(response);
                    
                    */
                   /*
                    
                   OutPutStream.WriteLine("<html><body><h1>test server</h1>");
                   OutPutStream.WriteLine("Current Time: " + DateTime.Now.ToString());
                   OutPutStream.WriteLine("url : {0}", url);

                   OutPutStream.WriteLine("<form method=post action=/form>");
                   OutPutStream.WriteLine("<input type=text name=foo value=foovalue>");
                   OutPutStream.WriteLine("<input type=submit name=bar value=barvalue>");
                   OutPutStream.WriteLine("</form>");*/
               }
           }

           private const int BUF_SIZE = 4096;
           public void handlePOSTRequest()
           {
               // this post data processing just reads everything into a memory stream.
               // this is fine for smallish things, but for large stuff we should really
               // hand an input stream to the request processor. However, the input stream 
               // we hand him needs to let him see the "end of the stream" at this content 
               // length, because otherwise he won't know when he's seen it all! 

               hurl.CWebURL(GetUrl());
               string url = hurl.WebAddress;
               string pluginn = url.TrimStart('/');
               Console.WriteLine("get post data start");
               int content_len = 0;
               MemoryStream ms = new MemoryStream();
               if (hr.getHeaders().ContainsKey("Content-Length"))
               {
                   content_len = Convert.ToInt32(hr.getHeaders()["Content-Length"]);
                   if (content_len > MAX_POST_SIZE)
                   {
                       throw new Exception(
                           String.Format("POST Content-Length({0}) too big for this simple server",
                             content_len));
                   }
                   byte[] buf = new byte[BUF_SIZE];
                   int to_read = content_len;
                   while (to_read > 0)
                   {
                       Console.WriteLine("starting Read, to_read={0}", to_read);

                       int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                       Console.WriteLine("read finished, numread={0}", numread);
                       if (numread == 0)
                       {
                           if (to_read == 0)
                           {
                               break;
                           }
                           else
                           {
                               throw new Exception("client disconnected during post");
                           }
                       }
                       to_read -= numread;
                       ms.Write(buf, 0, numread);
                   }
                   ms.Seek(0, SeekOrigin.Begin);
               }
               Console.WriteLine("get post data end");
               StreamReader inputData = new StreamReader(ms);
               string data = inputData.ReadToEnd();

               var dictionary = new Dictionary<string, string>();
               dictionary.Add("xml", data);
               ms.Close();
              // string url = GetUrl();
               hurl.CWebURL(GetUrl());
               hurl.PostParameters(data);
               
            //   Console.WriteLine("POST request: {0}", hurl.WebAddress);

             //  writeSuccess("text/plain");
              // inputStream.Flush();
               plugins.handleRequest(url, dictionary, OutPutStream);
               
              // inputStream.Close();

             // OutPutStream.WriteLine("OK");
             

           }

           public void writeSuccess(string content_type = "text/html")
           {
               outputStream.WriteLine("HTTP/1.0 200 OK");
               outputStream.WriteLine("Content-Type: " + content_type);
               outputStream.WriteLine("Connection: close");
               outputStream.WriteLine("");
           }

           public void writeFailure()
           {
               outputStream.WriteLine("HTTP/1.0 404 File not found");
               outputStream.WriteLine("Connection: close");
               outputStream.WriteLine("");
           }
           public class HttpResponse
           { }
    }
}
