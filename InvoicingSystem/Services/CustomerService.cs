using InvoicingSystem.Models;

namespace InvoicingSystem.Services
{
    public class CustomerService
    {
        private readonly List<Customer> _customers = new List<Customer>();

        public CustomerService()
        {
            _customers = new List<Customer>()
            {
                new Customer() {Id = 1,Name="John",Address="Mangalore", ContactNumber="9876543191",Email="abc@example.com"},
                new Customer() {Id = 2,Name="Ben",Address="Chennai", ContactNumber="9874563210",Email="abc1@example.com"},
                new Customer() {Id = 3,Name="Carl",Address="Kolkota", ContactNumber="987463211",Email="abc2@example.com"},
                new Customer() {Id = 4,Name="Lexi",Address="London", ContactNumber="9874561233",Email="abc3@example.com"},
                new Customer() {Id = 5,Name="Marc",Address="Sydney", ContactNumber="98745612345",Email="abc4@example.com"},
            };
        }
        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            ValidateCustomer(customer);
            customer.Id = GenerateCustomerId();
            _customers.Add(customer);
        }

        public void UpdateCustomer(int id, Customer updatedCustomer)
        {
            if (updatedCustomer == null)
            {
                throw new ArgumentNullException(nameof(updatedCustomer));
            }

            var existingCustomer = _customers.FirstOrDefault(c => c.Id == id);
            if (existingCustomer == null)
            {
                throw new ArgumentException($"Customer with ID {id} not found.");
            }

            if (existingCustomer.Email != updatedCustomer.Email || existingCustomer.ContactNumber != updatedCustomer.ContactNumber)
            {
                ValidateCustomer(updatedCustomer);
            }

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.Address = updatedCustomer.Address;
            existingCustomer.ContactNumber = updatedCustomer.ContactNumber;
        }

        public void DeleteCustomer(int id)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.Id == id);
            if (existingCustomer == null)
            {
                throw new ArgumentException($"Customer with ID {id} not found.");
            }

            _customers.Remove(existingCustomer);
        }

        private void ValidateCustomer(Customer customer)
        {
            if (_customers.Any(c => c.Email == customer.Email))
            {
                throw new ArgumentException($"Customer with email '{customer.Email}' already exists.");
            }

            if (_customers.Any(c => c.ContactNumber == customer.ContactNumber))
            {
                throw new ArgumentException($"Customer with contact number '{customer.ContactNumber}' already exists.");
            }
        }

        private int GenerateCustomerId()
        {
            return _customers.Count > 0 ? _customers.Max(c => c.Id) + 1 : 1;
        }
        public bool CustomerExists(int customerId)
        {
            return _customers.Any(c => c.Id == customerId);
        }
    }
}
