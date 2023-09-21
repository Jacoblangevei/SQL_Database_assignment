// See https://aka.ms/new-console-template for more information
using iToons;
using iToons.Models;
using Microsoft.Data.SqlClient;

string connectionString = "N-NO-01-01-6005\\SQLEXPRESS";
DatabaseManager databaseManager = new DatabaseManager(connectionString);
databaseManager.ConnectToDb();


// Get artist by id

int customerId = 10;

try
{
    var customer = databaseManager.GetCustomerById(customerId);

    if (customer.CustomerId != -1)
    {
        Console.WriteLine($"Customer found: CustomerId: {customer.CustomerId}, Name: {customer.FirstName}");
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



List<Customer> customers = databaseManager.GetAllCustomers();

foreach (var customer in customers)
{
    Console.WriteLine($"CustomerId: {customer.Id}, Name: {customer.FirstName}");
}



