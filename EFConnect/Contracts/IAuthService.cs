using System.Threading.Tasks;
using EFConnect.Data.Entities;

namespace EFConnect.Contracts
{
     public interface IAuthService
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}