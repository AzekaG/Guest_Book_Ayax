using System.ComponentModel.DataAnnotations;

namespace Guest_Book_Ayax.Models
{
    public class Message
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Отзыв не может быть пустым")]

        public string? Msg { get; set; }

        public DateTime? Date { get; set; }

        public Users? User { get; set; }


    }
}
