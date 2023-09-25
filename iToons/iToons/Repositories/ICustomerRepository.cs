using iToons.Models;
using System;

public interface ICustomerRepository : ICrudRepository<Customer, int>
{
    
    //List<Customer> SearchCustomersByName(string name);
    //List<Customer> GetCustomersWithPagination(int limit, int offset);
    //void AddCustomer(Customer customer);
    //void UpdateCustomer(Customer customer);
    //List<CustomerCountry> GetCustomerCountByCountry();
    //List<CustomerSpender> GetTopSpenders();
    //List<CustomerGenre> GetFavoriteGenre(int customerId);
}