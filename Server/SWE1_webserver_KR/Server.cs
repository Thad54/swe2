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
}
