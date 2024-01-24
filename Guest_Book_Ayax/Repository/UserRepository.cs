using Guest_Book_Ayax.Models;
using Microsoft.EntityFrameworkCore;

namespace Guest_Book_Ayax.Repository
{
    public class UserRepository : IRepository
    {
        readonly UsersContext _context;
        public UserRepository(UsersContext context) => _context = context;

        public List<Message> GetMessageList() =>/* _context.Messages.ToList();*/ _context.Messages.Include(m => m.User).ToList();


        public List<Users> GetUsersList() => _context.Users.ToList();

        public void CreateUser(Users user) => _context.Users.Add(user);

        public void Save() => _context.SaveChanges();

        public void AddMessage(Message mes) => _context.Messages.Add(mes);

    }
}
