using Guest_Book_Ayax.Models;

namespace Guest_Book_Ayax.Repository
{
    public interface IRepository
    {
        List<Message> GetMessageList();

        List<Users> GetUsersList();

        void CreateUser(Users user);

        void Save();

        void AddMessage(Message message);


    }
}
