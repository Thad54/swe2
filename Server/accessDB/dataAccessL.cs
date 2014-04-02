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
                        con.name = reader.GetString(0);
                        con.lastName = reader.GetString(1);
                        con.title = reader.GetString(2);
                        con.Suffix = reader.GetString(3);
                        con.address = reader.GetString(4);
                        con.creationDate = reader.GetDateTime(5);
                        con.billingAddress= reader.GetString(6);
                        con.shippingAddress = reader.GetString(7);

                        list.Add(con);

                    }



                } // end reader using
            } // end connection using
        

            return list;
        }
    }
}
