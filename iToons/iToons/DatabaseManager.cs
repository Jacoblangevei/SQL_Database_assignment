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

            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();

            string query = "SELECT * FROM Customer"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(
                    new Customer(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2))
                    );
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
                // You can throw an exception, return null, or handle it according to your requirements.
                throw new Exception("Customer not found");
            }
        }




    }
}
