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

        public Guid CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime Birthday { get; }
    }
}
