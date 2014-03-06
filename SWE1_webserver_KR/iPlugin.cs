using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace SWE1_webserver_KR
{
    public interface iPlugin
    {
        bool checkRequest(string input);
        void handleRequest(Dictionary<string, string> data, StreamWriter OutPutStream );
        string getName();
    }
}
