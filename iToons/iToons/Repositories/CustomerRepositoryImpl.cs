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
            // Jacob's pc: "N-NO-01-01-2827\\SQLEXPRESS";
            // Replace this with your actual database connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "N-NO-01-01-2827\\SQLEXPRESS";
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
            string query = "SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer";
            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                string country = reader.GetString(3);
                string postalCode = reader.IsDBNull(4) ? null : reader.GetString(4);
                string phone = reader.IsDBNull(5) ? null : reader.GetString(5);
                string email = reader.GetString(6);

                Customer customer = new Customer
                {
                    Id = customerId,
                    FirstName = firstName,
                    LastName = lastName,
                    Country = country,
                    PostalCode = postalCode,
                    PhoneNumber = phone,
                    Email = email
                };
                customers.Add(customer);
            }
            return customers;
        }

        public Customer GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, PostalCode, Phone, Email FROM Customer WHERE CustomerId = @Id";
            using SqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                string country = reader.GetString(3);
                string postalCode = reader.IsDBNull(4) ? null : reader.GetString(4);
                string phone = reader.IsDBNull(5) ? null : reader.GetString(5);
                string email = reader.GetString(6);

                return new Customer
                {
                    Id = customerId,
                    FirstName = firstName,
                    LastName = lastName,
                    Country = country,
                    PostalCode = postalCode,
                    PhoneNumber = phone,
                    Email = email
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

            string query = "SELECT CustomerId, FirstName, LastName, Country, PostalCode FROM Customer WHERE FirstName LIKE @Name"; // Replace YourTableName with your actual table name

            using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", "%" + customer.FirstName + "%");

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                string country = reader.GetString(3);
                string postalCode = reader.IsDBNull(4) ? null : reader.GetString(4);

                return new Customer
                {
                    Id = customerId,
                    FirstName = firstName,
                    LastName = lastName,
                    Country = country,
                    PostalCode = postalCode
                };
            }
            else
            {
                // If no customer is found
                throw new Exception("Customer not found");
            }
        }
        public List<Customer> GetCustomersPage(int limit, int offset)
        {
            List<Customer> customers = new List<Customer>(); 

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "SELECT CustomerId, FirstName, LastName, " +
                               "Country, PostalCode, Phone, Email " +
                               "FROM Customer " +
                               "ORDER BY CustomerId " +
                               "OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@Limit", limit);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string country = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string postalCode = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string phone = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            string email = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            customers.Add( new Customer
                            {
                                Id = customerId,
                                FirstName = firstName,
                                LastName = lastName,
                                Country = country,
                                PostalCode = postalCode,
                                PhoneNumber = phone,
                                Email = email
                            });

                        }
                    }
                }
            }

            return customers; // Return the list of customers
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "UPDATE Customer " +
                               "SET FirstName = @FirstName, LastName = @LastName, " +
                               "Country = @Country, PostalCode = @PostalCode, " +
                               "Phone = @Phone, Email = @Email " +
                               "WHERE CustomerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", updatedCustomer.Id);
                    command.Parameters.AddWithValue("@FirstName", updatedCustomer.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedCustomer.LastName);
                    command.Parameters.AddWithValue("@Country", updatedCustomer.Country);
                    command.Parameters.AddWithValue("@PostalCode", updatedCustomer.PostalCode);
                    command.Parameters.AddWithValue("@Phone", updatedCustomer.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", updatedCustomer.Email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        // Handle the case where no customer was updated (e.g., customer not found).
                        throw new Exception("Customer not found or not updated.");
                    }
                }
            }
        }
        public Dictionary<string, int> GetCustomerCountByCountry()
        {
            Dictionary<string, int> customerCounts = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "SELECT Country, COUNT(*) AS CustomerCount " +
                               "FROM Customer " +
                               "GROUP BY Country " +
                               "ORDER BY CustomerCount DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string country = reader.GetString(0);
                            int customerCount = reader.GetInt32(1);

                            customerCounts.Add(country, customerCount);
                        }
                    }
                }
            }

            return customerCounts;
        }
        public List<Customer> GetHighestSpenders()
        {
            List<Customer> highestSpenders = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "SELECT c.CustomerId, c.FirstName, c.LastName, SUM(i.Total) AS TotalSpent " +
                               "FROM Customer AS c " +
                               "JOIN Invoice AS i ON c.CustomerId = i.CustomerId " +
                               "GROUP BY c.CustomerId, c.FirstName, c.LastName " +
                               "ORDER BY TotalSpent DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            decimal totalSpent = reader.GetDecimal(3);

                            Customer customer = new Customer
                            {
                                Id = customerId,
                                FirstName = firstName,
                                LastName = lastName,
                                TotalSpent = totalSpent
                            };

                            highestSpenders.Add(customer);
                        }
                    }
                }
            }
            return highestSpenders;
        }

        public List<CustomerGenre> GetMostPopularGenresByCustomerId(int customerId)
        {
            List<CustomerGenre> genres = new List<CustomerGenre>();

            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();

            string sql = @"
                SELECT TOP 1 WITH TIES g.Name, COUNT(t.TrackId) as TrackCount
                FROM Customer c
                JOIN Invoice i ON c.CustomerId = i.CustomerId
                JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId
                JOIN Track t ON il.TrackId = t.TrackId
                JOIN Genre g ON t.GenreId = g.GenreId
                WHERE c.CustomerId = @CustomerId
                GROUP BY g.Name
                ORDER BY TrackCount DESC
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var genre = new CustomerGenre
                {
                    Id = customerId,
                    Genre = reader.GetString(0)
                };

                genres.Add(genre);
            }

            return genres;
        }
    }
}