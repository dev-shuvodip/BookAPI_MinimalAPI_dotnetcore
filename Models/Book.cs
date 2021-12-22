namespace BookAPI.Models
{
    public class Book
    {        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
