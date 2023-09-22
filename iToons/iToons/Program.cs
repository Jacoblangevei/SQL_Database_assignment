using iToons.Models;
using iToons.Repositories;
using Microsoft.Data.SqlClient;

string connectionString = "N-NO-01-01-2827\\SQLEXPRESS";
//DatabaseManager databaseManager = new DatabaseManager(connectionString);
//databaseManager.ConnectToDb();

CustomerRepositoryImpl customerRepositoryImpl = new CustomerRepositoryImpl(connectionString);
customerRepositoryImpl.ConnectToDb();

List<Customer> highestSpenders = customerRepositoryImpl.GetHighestSpenders();

Console.WriteLine("Highest Spenders");
Console.WriteLine("=================");

foreach (var customer in highestSpenders)
{
    Console.WriteLine($"{customer.FirstName} {customer.LastName}: ${customer.TotalSpent}");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

Dictionary<string, int> customerCounts = customerRepositoryImpl.GetCustomerCountByCountry();
    try
    {
        Console.WriteLine("Customer per country");

        foreach (var kvp in customerCounts)
        {
            Console.WriteLine($"{kvp.Key}\t{kvp.Value}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

Customer updatedCustomer = new Customer
{
    Id = 2, 
    FirstName = "Kasper",
    LastName = "Brud",
    Country = "Norge",
    PostalCode = "3214",
    PhoneNumber = "90270930",
    Email = "kaspeprp@experis.com"
};

try
{
    customerRepositoryImpl.UpdateCustomer(updatedCustomer);
    Console.WriteLine("Customer updated successfully.");
    Customer customer = customerRepositoryImpl.GetById(2);

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

int limit = 10; 
int offset = 5; 

try
{
    List<Customer> customerPages = customerRepositoryImpl.GetCustomersPage(limit, offset);

    if (customerPages.Count == 0)
    {
        Console.WriteLine("No customers found.");
    }
    else
    {
        foreach (Customer customer in customerPages)
        {
            Console.WriteLine($"ID={customer.Id}, First Name: {customer.FirstName}, Last Name: {customer.LastName}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

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

List<Customer> customers = customerRepositoryImpl.GetAll();

foreach (var customer in customers)
{
    Console.WriteLine($"CustomerId: {customer.Id}, " +
    $"Name: {customer.FirstName}, Last Name: {customer.LastName}, " +
    $"Country: {customer.Country}, Postal Code: {customer.PostalCode}, " +
    $"Phone number: {customer.PhoneNumber}, Email: {customer.Email}");
}

ICustomerRepository customerRepo =
    new CustomerRepositoryImpl(GetConnectionString());

string GetConnectionString()
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