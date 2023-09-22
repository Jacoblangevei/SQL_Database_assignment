// See https://aka.ms/new-console-template for more information
using iToons;
using iToons.Models;
using iToons.Repositories;
using Microsoft.Data.SqlClient;

string connectionString = "N-NO-01-01-6005\\SQLEXPRESS";
DatabaseManager databaseManager = new DatabaseManager(connectionString);
databaseManager.ConnectToDb();

CustomerRepositoryImpl customerRepositoryImpl = new CustomerRepositoryImpl(connectionString);
customerRepositoryImpl.ConnectToDb();


Customer customerToSearch = new Customer
{
    FirstName = "Helena"
};

try
{
    // Call the GetByName method to search for the customer
    Customer foundCustomer = customerRepositoryImpl.GetByName(customerToSearch);

    // Display the details of the found customer
    Console.WriteLine($"Customer found: CustomerId: {foundCustomer.Id}, " +
        $"Name: {foundCustomer.FirstName}, Last Name: {foundCustomer.LastName}, " +
        $"Country: {foundCustomer.Country}, Postal Code: {foundCustomer.PostalCode}, " +
        $"Phone number: {foundCustomer.PhoneNumber}, Email: {foundCustomer.Email}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

int customerId = 10;

try
{
    Customer customer = customerRepositoryImpl.GetById(customerId);

    if (customer.Id != -1)
    {
        Console.WriteLine($"Customer found: CustomerId: {customer.Id}, " +
            $"Name: {customer.FirstName}, Last Name: {customer.LastName}, " +
            $"Country: {customer.Country}, Postal Code: {customer.PostalCode}, " +
            $"Phone number: {customer.PhoneNumber}, Email: {customer.Email}");
    }
    else
    {
        Console.WriteLine("Customer not found");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}



//List<Customer> customers = customerRepositoryImpl.GetAll();

//foreach (var customer in customers)
//{
//    Console.WriteLine($"Customer found: CustomerId: {customer.Id}, " +
//    $"Name: {customer.FirstName}, Last Name: {customer.LastName}, ");
//}


ICustomerRepository customerRepo =
    new CustomerRepositoryImpl(GetConnectionString());


string GetConnectionString()
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

