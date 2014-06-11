using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace accessDB
{
    class dataAccessL
    {
        public List<XmlExchange.bill> searchBill(XmlExchange.command com, bool onlyActive)
        {
            var list = new List<XmlExchange.bill>();

            //using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Select B.BLL_ID, B.Comment, B.Contact_FK, B.Date, B.DueBy, B.Message, SUM(BP.Price * Bp.Amount * Bp.Tax) BillingAmount from Bill B inner join BillingPosition BP on BP.Bill_FK = B.BLL_ID where B.Contact_FK like @ContactFK and (B.Date between @DateFrom and @DateTo) group by B.BLL_ID, B.Comment, B.Contact_FK, B.Date, B.DueBy, B.Message having SUM(BP.Price * Bp.Amount * Bp.Tax) between @AmountFrom and @AmountTo";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (com.ContactID != null)
                {
                    cmd.Parameters.AddWithValue("@ContactFK", com.ContactID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactFK", "%");
                }
                if (com.from != null)
                {
                    cmd.Parameters.AddWithValue("@DateFrom", com.from);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateFrom", new DateTime(1753, 1, 1));
                }
                if (com.to != null)
                {
                    cmd.Parameters.AddWithValue("@DateTo", com.to);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateTo", new DateTime(9999, 12, 31));
                }
                if (com.amountFrom != null)
                {
                    cmd.Parameters.AddWithValue("@AmountFrom", com.amountFrom);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AmountFrom", 0);
                }
                if (com.amountTo != null)
                {
                    cmd.Parameters.AddWithValue("@AmountTo", com.amountTo);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AmountTo", 2147483646);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var bill = new XmlExchange.bill();
                        bill.ID = reader[0] as int? ?? default(int);
                        bill.comment = reader[1] as string;
                        bill.contactId = reader[2] as int? ?? null;
                        bill.BillingDate = reader[3] as DateTime? ?? null;
                        bill.DueByDate = reader[4] as DateTime? ?? null;
                        bill.message = reader[5] as string;
                        bill.billAmount = 0;
                        bill.billingPositions = new ObservableCollection<XmlExchange.billingPosition>();

                        list.Add(bill);

                    }



                } // end reader using
                foreach (var elem in list)
                {
                    if (elem.contactId != null)
                    {

                        query = "Select Con.CNT_ID, Con.Title, Con.FirstName, Con.LastName, Con.Suffix, Con.Address, Con.CreationDate, Con.BillingAddress, Con.DeliveryAddress, C.FirstName Company, C.UID, C.CNT_ID   from Contact Con left join Contact C on C.CNT_ID = Con.Company_FK  where Con.CNT_ID = @ContactFK";

                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ContactFK", elem.contactId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var con = new XmlExchange.contact();
                                con.id = reader[0] as int? ?? default(int);
                                con.title = reader[1] as string;
                                con.name = reader[2] as string;
                                con.lastName = reader[3] as string;
                                con.Suffix = reader[4] as string;
                                con.address = reader[5] as string;
                                con.creationDate = reader[6] as DateTime? ?? default(DateTime);
                                con.billingAddress = reader[7] as string;
                                con.shippingAddress = reader[8] as string;
                                con.company = reader[9] as string;
                                con.uid = reader[10] as string;
                                con.companyID = reader[11] as int? ?? null;
                                if (con.uid != null)
                                {
                                    con.isCompany = true;
                                }
                                else
                                {
                                    con.isCompany = false;
                                }

                                elem.contact = con;

                            }
                        } // end reader using      



                    }
                    query = "Select Name, Price, Tax, Amount from BillingPosition where Bill_FK = @BillFK";
                    
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BillFK", elem.ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var bp = new XmlExchange.billingPosition();
                            bp.name = reader[0] as string;
                            bp.price = reader[1] as decimal? ?? null;
                            bp.tax = reader[2] as decimal? ?? null;
                            bp.amount = reader[3] as int? ?? null;

                            elem.billAmount += bp.price * bp.tax * bp.amount;
                            elem.billingPositions.Add(bp);

                        }
                    } // end reader using    
                }

            } // end connection using
            

            return list;
        }


        public List<XmlExchange.contact> searchPerson(XmlExchange.contact contact, bool onlyActive)
        {
            var list = new List<XmlExchange.contact>();

            using ( SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Select Con.CNT_ID, Con.Title, Con.FirstName, Con.LastName, Con.Suffix, Con.Address, Con.CreationDate, Con.BillingAddress, Con.DeliveryAddress, C.FirstName Company, C.UID, C.CNT_ID   from Contact Con left join Contact C on C.CNT_ID = Con.Company_FK  where Con.FirstName like @FirstName and Con.LastName like @LastName";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", "%"+contact.name+"%");
                cmd.Parameters.AddWithValue("@LastName", "%" + contact.lastName + "%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var con = new XmlExchange.contact();
                        con.id = reader[0] as int? ?? default(int);
                        con.title = reader[1] as string;
                        con.name = reader[2] as string;
                        con.lastName = reader[3] as string;
                        con.Suffix = reader[4] as string;
                        con.address = reader[5] as string;
                        con.creationDate = reader[6] as DateTime? ?? default(DateTime);
                        con.billingAddress = reader[7] as string;
                        con.shippingAddress = reader[8] as string;
                        con.company = reader[9] as string;
                        con.uid = reader[10] as string;
                        con.companyID = reader[11] as int? ?? null;
                        con.isCompany = false;

                        list.Add(con);

                    }



                } // end reader using
            } // end connection using
        

            return list;
        }

        public List<XmlExchange.contact> searchCompany(XmlExchange.contact contact, bool onlyActive)
        {
            var list = new List<XmlExchange.contact>();

            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Select CNT_ID, FirstName, UID, Address, CreationDate, BillingAddress, DeliveryAddress  from Contact where FirstName like @CompanyName and UID like @UID";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompanyName", "%" + contact.company + "%");
                cmd.Parameters.AddWithValue("@UID", "%" + contact.uid + "%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var con = new XmlExchange.contact();
                        con.id = reader[0] as int? ?? default(int);
                        con.name = reader[1] as string;
                        con.uid = reader[2] as string;
                        con.address = reader[3] as string;
                        con.creationDate = reader[4] as DateTime? ?? default(DateTime);
                        con.billingAddress = reader[5] as string;
                        con.shippingAddress = reader[6] as string;
                        con.isCompany = true;

                        list.Add(con);

                    }



                } // end reader using
            } // end connection using


            return list;
        }

        public XmlExchange.message editContact(XmlExchange.contact contact)
        {
            var message = new XmlExchange.message();

            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Update Contact set Title = @title, FirstName = @firstName, LastName = @lastName, UID = @uid, Suffix = @suffix, CreationDate = @creationDate, Address = @address, BillingAddress = @billingAddress, DeliveryAddress = @shippingAddress, Company_FK = @company_fk where CNT_ID = @id";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

                if (contact.title == string.Empty || contact.title == null)
                {
                    cmd.Parameters.AddWithValue("@title", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@title", contact.title);
                }

                if (contact.Suffix == string.Empty || contact.Suffix == null)
                {
                    cmd.Parameters.AddWithValue("@suffix", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@suffix", contact.Suffix);
                }

                if (contact.lastName == string.Empty || contact.lastName == null)
                {
                    cmd.Parameters.AddWithValue("@lastName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@lastName", contact.lastName);
                }

                if (contact.uid == string.Empty || contact.uid == null)
                {
                    cmd.Parameters.AddWithValue("@uid", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@uid", contact.uid);
                }

                if (contact.companyID == null)
                {
                    cmd.Parameters.AddWithValue("@company_fk", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@company_fk", contact.companyID);
                }

                if (contact.name == string.Empty || contact.name == null)
                {
                    cmd.Parameters.AddWithValue("@firstName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@firstName", contact.name);
                }

                if (contact.creationDate == null)
                {
                    cmd.Parameters.AddWithValue("@creationDate", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@creationDate", contact.creationDate);
                }

                if (contact.address == string.Empty || contact.address == null)
                {
                    cmd.Parameters.AddWithValue("@address", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@address", contact.address);
                }

                if (contact.billingAddress == string.Empty || contact.billingAddress == null)
                {
                    cmd.Parameters.AddWithValue("@billingAddress", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@billingAddress", contact.billingAddress);
                }

                if (contact.shippingAddress == string.Empty || contact.shippingAddress == null)
                {
                    cmd.Parameters.AddWithValue("@shippingAddress", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@shippingAddress", contact.shippingAddress);
                }

                if (contact.id == null)
                {
                    cmd.Parameters.AddWithValue("@id", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@id", contact.id);
                }


                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    message.error = true;
                    message.text = "Database Error occurred";
                } else {
                    message.error = false;
                    message.text = string.Empty;
                }
            } // end connection using

            return message;
        }

        public XmlExchange.message editBill(XmlExchange.bill bill)
        {
            int rows;
            var message = new XmlExchange.message();

            using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MicroERP;integrated Security=SSPI"))
            {
                string query = "Delete BillingPosition where Bill_FK = @Bill_ID";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Bill_ID", bill.ID);

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    message.error = true;
                    message.text = "Database Error occurred";
                    return message;
                }

                foreach (var bp in bill.billingPositions)
                {
                    query = "Insert into BillingPosition(Name, Price, Tax, Amount, Bill_FK) values(@Name, @Price, @Tax, @Amount, @Bill_ID)";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Bill_ID", bill.ID);
                    cmd.Parameters.AddWithValue("@Name", bp.name);
                    cmd.Parameters.AddWithValue("@Price", bp.price);
                    cmd.Parameters.AddWithValue("@Tax", bp.tax);
                    cmd.Parameters.AddWithValue("@Amount", bp.amount);

                    rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        message.error = true;
                        message.text = "Database Error occurred";
                        return message;
                    }
                }

                query = "Update Bill set Comment = @Comment, Message = @Message, Date = @Date, DueBy = @DueBy, Contact_FK = @Contact_FK where BLL_ID = @Bill_ID";

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Bill_ID", bill.ID);
                if (bill.comment == string.Empty || bill.comment == null)
                {
                    cmd.Parameters.AddWithValue("@Comment", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Comment", bill.comment);
                }
                if (bill.message == string.Empty || bill.message == null)
                {
                    cmd.Parameters.AddWithValue("@Message", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Message", bill.message);
                }
                cmd.Parameters.AddWithValue("@Date", bill.BillingDate);
                cmd.Parameters.AddWithValue("@DueBy", bill.DueByDate);
                cmd.Parameters.AddWithValue("@Contact_FK", bill.contactId);
                

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    message.error = true;
                    message.text = "Database Error occurred";
                }
                else
                {
                    message.error = false;
                    message.text = string.Empty;
                }
            } // end connection using

            return message;
        }
    }
}
