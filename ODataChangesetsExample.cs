using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.OData.Edm;
using ODataUtility.Microsoft.Dynamics.DataEntities;

namespace ODataConsoleApplication
{
    class ODataChangesetsExample
    {
        

        public static void CreateSalesOrderInSingleChangeset(Resources context)
        {
            string paymentBankAccount = "KGS123456789";
            try
            {
                
                Customer customer = new Customer();
                DataServiceCollection<Customer> customerCollection = new DataServiceCollection<Customer>(context);
                customerCollection.Add(customer);

                customer.PaymentBankAccount = paymentBankAccount;
                customer.DataAreaId = "USMF";
                customer.PrimaryContactEmail = "Example@Mail.com";   
                context.SaveChanges(SaveChangesOptions.PostOnlySetProperties | SaveChangesOptions.BatchWithSingleChangeset); // Batch with Single Changeset ensure the saved changed runs in all-or-nothing mode.
                Console.WriteLine(string.Format("Invoice {0} - Saved !", paymentBankAccount));
            }
            catch (DataServiceRequestException e)
            {
                Console.WriteLine(string.Format("Invoice {0} - Save Failed !", paymentBankAccount));
            }
        }
        public static void getDataBase(Resources context)
        {
            SqlConnection connection;
            SqlCommand command;
            string queryString, name ,paymentBankAccount = "";
            SqlDataReader dataReader;
            string dataareaid = "USMF";
            string connectionString = "Data Source=MININT-5REKJI3;Initial Catalog=WebServices;Integrated Security=True";

            queryString = "select  name , paymentBankAccount  from dbo.Customer where dataareaid = @Find";

            connection = new SqlConnection(connectionString);

            connection.Open();

            command = new SqlCommand(queryString , connection);

            command.Parameters.Add(new SqlParameter("Find", dataareaid));

            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("{0),{1}", dataReader.GetString(0), dataReader.GetString(1));
                name = dataReader.GetString(0);
;               paymentBankAccount= dataReader.GetString(1);

            }
            dataReader.Close();
        }
        public static void insertIntoDataBase(Resources context)
        {
            string connectionString, Name=null, Email=null;

            string bankAccountNumber = null;

            SqlConnection con;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd;
            connectionString = "Data Source = MININT - 5REKJI3; Initial Catalog = WebServices; Integrated Security = True";

            con = new SqlConnection(connectionString);
            try
            {
                con.Open();

                Customer customer = new Customer();
                DataServiceCollection<Customer> customerDetailCollection = new DataServiceCollection<Customer>(context);

                customerDetailCollection.Add(customer);

                var CustomerDetailsnew = context.Customers.Where(x => x.DataAreaId == "USMF");

                foreach(var Customer in customerDetailCollection)
                {

                    bankAccountNumber = Customer.PaymentBankAccount;
                    Name = Customer.Name;
                    Email = Customer.PrimaryContactEmail;
                    cmd = new SqlCommand("insert into ax.CustTable (bankAccountNumber,Name,Email) values (@bankAccountNumber,@Name,@Email)");
                    cmd.Parameters.AddWithValue("@bankAccountNumber", bankAccountNumber);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("BackAccount", Customer.PaymentBankAccount);
                  
                }
                con.Close();
                //console.ReadLine();

                //MessageBox.Show("Rows inserted");

            }
            catch(Exception ex)
            {   
                //MessageBox.Show(ex.ToString());
            }

        }
    }
}
