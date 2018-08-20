using System.Collections.Generic;

namespace UnitTests.Models
{
    public class ComplexModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
    }

    public class Address
    {
        public string Street { get; set; }
    }
}