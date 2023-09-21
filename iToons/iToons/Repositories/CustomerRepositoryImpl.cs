using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using iToons.Exceptions; // Exceptions folder with custom exceptions.
using iToons.Models;

namespace iToons.Repositories
{
    internal class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddCustomer(Customer customer)
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

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            string sql = "SELECT Id, FirstName, LastName, Country, PostalCode, PhoneNumber, Email FROM Customers";
            using SqlCommand command = new(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Country = reader.GetString(3),
                    PostalCode = reader.GetString(4),
                    PhoneNumber = reader.GetString(5),
                    Email = reader.GetString(6)
                });
            }
            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            Customer customer = null;
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            string sql = "SELECT Id, FirstName, LastName, Country, PostalCode, PhoneNumber, Email FROM Customers WHERE Id = @Id";
            using SqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                customer = new Customer
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Country = reader.GetString(3),
                    PostalCode = reader.GetString(4),
                    PhoneNumber = reader.GetString(5),
                    Email = reader.GetString(6)
                };
            }
            else
            {
                throw new CustomerNotFoundException("No customer exists with that ID");
            }
            return customer;
        }

        // Implement other methods (pagination, update, etc.) similar to above.
    }
}

