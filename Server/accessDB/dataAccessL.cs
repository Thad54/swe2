using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace accessDB
{
    class dataAccessL
    {

        public List<XmlExchange.contact> searchContact(string searchText, bool onlyActive)
        {
            var list = new List<XmlExchange.contact>();

            using ( SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Select FirstName, LastName, Title, Suffix, Address, CreationDate, BillingAddress, DeliveryAddress  from Contact where FirstName like @name or LastName like @name";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", "%"+searchText+"%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var con = new XmlExchange.contact();
                        con.name = reader[0] as string;
                        con.lastName = reader[1] as string;
                        con.title = reader[2] as string;
                        con.Suffix = reader[3] as string;
                        con.address = reader[4] as string;
                        con.creationDate = reader[5] as DateTime? ?? default(DateTime);
                        con.billingAddress = reader[6] as string;
                        con.shippingAddress = reader[7] as string;

                        list.Add(con);

                    }



                } // end reader using
            } // end connection using
        

            return list;
        }
    }
}
