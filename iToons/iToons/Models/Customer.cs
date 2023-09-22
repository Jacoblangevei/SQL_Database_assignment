using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iToons.Models
{
    public struct Customer
    {
        public int Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal TotalSpent { get; set; }

        public Customer(int id, string firstName, string lastName, string country, string postalCode, string phoneNumber, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public override bool Equals(object? obj)
        {
            return obj is Customer customer && 
                Id == customer.Id &&
                FirstName == customer.FirstName &&
                LastName == customer.LastName;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName);
        }
        public override string ToString()
        {
            return $"ID= {Id}, First name: {FirstName}, Last name: {LastName}";
        }
    }
}