using Ats.Core.Domain;
using System;

namespace Ats.Domain.Customer
{
    public class CustomerRegisteredEvent : IEvent
    {
        public CustomerRegisteredEvent(Guid customerId, string firstName, string lastName, DateTime birthday)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
