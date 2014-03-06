using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_webserver_KR
{
    class temp : plugin
    {
        public int checkRequest(string request){
            if (request.StartsWith("/temp") == true)
            {
                Console.Write("Correct");
                return 1;
            }
            else
            {
                Console.Write("No match");
            }
            return 0;
        }
    }
}
