namespace BookStoreApi.Database.Models
{
    public class InvoiceBooks
    {
        public int InvoiceId { get; set; }
        public int BookId { get; set; }
        public int Count { get; set; }
        public long Price { get; set; }
        public Invoice? Invoice { get; set; }
        public Book? Book { get; set; }
    }
}
