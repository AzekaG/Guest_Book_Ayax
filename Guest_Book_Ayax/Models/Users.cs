namespace Guest_Book_Ayax.Models
{
    public class Users
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public ICollection<Message>? messages { get; set; }

        public Users()
        {
            messages = new HashSet<Message>();
        }
    }
}
