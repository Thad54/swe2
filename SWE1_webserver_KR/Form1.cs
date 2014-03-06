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
    public partial class Form1 : Form
    {


        

        public Form1()
        {
            InitializeComponent();
        }
        public static string input;
        
            public void writeinLIST(string inputt)
            {
                listlog.Items.Add(inputt);
            }

        
            
       
    

        private void button1_Click(object sender, EventArgs e)
        {
           

            Server.MyHttpServer httpServer = new Server.MyHttpServer(Convert.ToInt16(textPort.Text));

            label1.Text = "Running...";
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textPort_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
