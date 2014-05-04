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
    class Server
    {
        

   public class NewHttpServer
   {
    private int port;
        private string name;
        private HttpListener listener;
        private HttpListenerContext context;
        HttpUrl hurl = new HttpUrl();
        private pluginM plugins;
        public NewHttpServer(int _port, string _name)
        {
            port = _port;
            name = _name;
        }
        public void startServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:"+port+"/");
          //  listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
           // listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Start();
            Console.WriteLine(" Start listening...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(startListening));
 
        }
 
        private void startListening(object o)
        {
            while (true)
            {
                Console.WriteLine("listening...");
                context = listener.GetContext();
                sendResponse();
            }
        }
 
        private void sendResponse()
        {

            if (context.Request.HttpMethod == "GET")
            { handleGETrequest(); }
            else if (context.Request.HttpMethod == "POST")
            { handlePOSTrequest(); }
            else
            {



                HttpListenerResponse response = context.Response;
                string responseString = "<HTML><BODY><h3> Your connected to<h1> " + name + "</h1></h3></BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
        private void handleGETrequest()
        { hurl.CWebURL(context.Request.RawUrl);

        HttpListenerResponse response = context.Response;
        string responseString = "<HTML><BODY><h3> Your connected to<h1> " + name + "</h1></h3></BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        }
        private void handlePOSTrequest()
        { 
            string input = new StreamReader(context.Request.InputStream, 
    context.Request.ContentEncoding).ReadToEnd();
            hurl.PostParameters(input);
            plugins.handleRequest(hurl.WebAddress,hurl.WebParameters,HttpListenerResponse response = context.Response;);
        
        }
   }


   #region old server

   public class MyHttpServer 
        {
            
            protected int port;
            TcpListener listener;
            bool is_active = true;

            public MyHttpServer(int port)
            {
                this.port = port;
            }

            public void listen()
            {
                listener = new TcpListener(port);
                listener.Start();

                pluginM plugins = new pluginM();
                plugins.loadPlugins();

                while (is_active)
                {
                    TcpClient s = listener.AcceptTcpClient();
                    ResponseProcessor processor = new ResponseProcessor(s, this, plugins);
                    Thread thread = new Thread(new ThreadStart(processor.process));
                    thread.Start();
                    Thread.Sleep(1);
                }
            }

           
        }
    }
   #endregion
}
