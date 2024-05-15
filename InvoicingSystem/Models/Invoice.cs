using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace InvoicingSystem.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public string CustomerAddress { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class InvoiceItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }

    public enum PaymentMethod
    {
        Cash=1,
        CreditCard,
        DebitCard,
        OnlinePayment
    }
}
