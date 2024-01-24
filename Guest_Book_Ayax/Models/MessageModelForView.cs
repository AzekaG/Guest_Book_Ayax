using System.ComponentModel.DataAnnotations;

namespace Guest_Book_Ayax.Models
{
	public class MessageModelForView
	{
		public int id { get; set; }

		public string? Msg { get; set; }

		public string? Date { get; set; }

		public string? UserFirstName { get; set; }
		public string? UserLastName { get; set; }

	}
}
