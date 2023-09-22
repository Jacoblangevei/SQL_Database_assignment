using iToons.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iToons
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void ConnectToDb()
        {
            try
            {
                using SqlConnection sqlConnection = new(GetConnectionString());
                sqlConnection.Open();
                Console.WriteLine("Connected");
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine("Sql exception: " + sqlException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetConnectionString()
        {

            // Jan's pc: "N-NO-01-01-6005\\SQLEXPRESS";

            // Replace this with your actual database connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "N-NO-01-01-6005\\SQLEXPRESS";
            builder.InitialCatalog = "Chinook";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            { 

            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();

            string query = "SELECT * FROM Customer"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = sqlCommand.ExecuteReader();

                //while (reader.Read())
                //{
                //    customers.Add(new Customer
                //        (reader.GetInt32(0),
                //        reader.GetString(1),
                //        reader.GetString(2))
                        
                //        );
                //}
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Sql exception: " + sqlEx.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return customers;

        }
        public (int CustomerId, string FirstName) GetCustomerById(int customerId)
        {
            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();

            string query = "SELECT CustomerId, FirstName from Customer where CustomerId = @CustomerId"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CustomerId", customerId);

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                int artistId = reader.GetInt32(0);
                string name = reader.GetString(1);
                return (artistId, name);
            }
            else
            {
                // Handle the case where the customer with the specified Id was not found.
                throw new Exception("Customer not found");
            }
        }

        public (int CustomerId, string FirstName) GetCustomerByName(string name)
        {
            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();

            string query = "SELECT CustomerId, FirstName FROM Customer WHERE FirstName LIKE @Name"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", "%" + name + "%"); // Partial match with LIKE

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                return (customerId, firstName);
            }
            else
            {
                // Handle the case where no customer with the specified name was found.
                throw new Exception("Customer not found");
            }
        }


    }
}
