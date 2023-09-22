using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
 // Exceptions folder with custom exceptions.
using iToons.Models;
using Microsoft.Data.SqlClient;

namespace iToons.Repositories
{
    internal class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
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

        public void Add(Customer customer)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            string sql = "INSERT INTO Customers (FirstName, LastName, Country, PostalCode, PhoneNumber, Email) VALUES (@FirstName, @LastName, @Country, @PostalCode, @PhoneNumber, @Email)";
            using SqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Country", customer.Country);
            command.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
            command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.ExecuteNonQuery();
        }

        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            string query = "SELECT * FROM Customer";
            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();
            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                    (reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)
                    //reader.GetString(3),
                    //reader.GetString(4),
                    //reader.GetString(5),
                    //reader.GetString(6))
                    ));
            }
            return customers;
        }

        public Customer GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            string sql = "SELECT CustomerId, FirstName, LastName FROM Customer WHERE CustomerId = @Id";
            using SqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);

                return new Customer
                {
                    Id = customerId,
                    FirstName = firstName,
                    LastName = lastName
                };
            }
            else
            {
                throw new Exception("No customer exists with that ID");
            }
        }

        public Customer GetByName(Customer customer)
        {
            using SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();

            string query = "SELECT CustomerId, FirstName FROM Customer WHERE FirstName LIKE @Name"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", "%" + customer.FirstName + "%"); // Partial match with LIKE

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);

                return new Customer
                {
                    Id = customerId,
                    FirstName = firstName
                };
            }
            else
            {
                // Handle the case where no customer with the specified name was found.
                throw new Exception("Customer not found");
            }
        }


        // Implement other methods (pagination, update, etc.) similar to above.
    }
}

