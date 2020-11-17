using System.Collections.Generic;
using System.Threading.Tasks;
using EFConnect.Models;

namespace EFConnect.Contracts
{
    public interface IUserService
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<UserForList>> GetUsers();
        Task<UserForDetail> GetUser(int id);
        Task<bool> UpdateUser(int id, UserForUpdate userForUpdate);
    }
}