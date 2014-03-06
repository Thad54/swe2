using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_webserver_KR;

namespace SWE1_webserver_KR
{
    public class Class1 : iPlugin
    {
      public bool checkRequest(string request)
        {


            return false;
        }

      public string handleRequest()
      {
          return "right";
      }

    }
}
