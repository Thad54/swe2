using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_webserver_KR;
using System.Data.SqlClient;
using System.IO;

namespace accessDB
{
    public class accessDB : iPlugin
    {
        public string getName()
        {
            return "accessDB";
        }
        public bool checkRequest(string input)
        {
            string[] url = input.Split('/');
            if (url[1].Equals("db"))
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
            StringBuilder answer = new StringBuilder();

            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Select FirstName from Contact";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

              /*  foreach (SqlParameter par in myparam)
                {
                    if (par != null)
                    {
                        cmd.Parameters.Add(par);
                    }
                }*/


                using (SqlDataReader reader = cmd.ExecuteReader())
                {


                        answer.Append("<!DOCTYPE html><html><body>");

                        while (reader.Read())
                        {
                            answer.AppendFormat("<p>{0}</p>", reader.GetString(0));
                        }

                        answer.Append("</body></html>");


                } // end reader using
            } // end connection using

             WriteResponse(answer.ToString(), "text/html", OutPutStream);
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
