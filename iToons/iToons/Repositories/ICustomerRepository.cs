using System;

public interface ICustomerRepository
{
    List<Customer> GetAllCustomers();
    Customer GetCustomerById(int id);
    List<Customer> SearchCustomersByName(string name);
    List<Customer> GetCustomersWithPagination(int limit, int offset);
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    List<CustomerCountry> GetCustomerCountByCountry();
    List<CustomerSpender> GetTopSpenders();
    List<CustomerGenre> GetFavoriteGenre(int customerId);
}

