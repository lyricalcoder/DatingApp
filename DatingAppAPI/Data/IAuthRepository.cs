using System.Threading.Tasks;
using DatingAppAPI.Models;

namespace DatingAppAPI.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string userName, string password);
         Task<bool> UserExists(string userName);
    }
}