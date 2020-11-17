using System.Threading.Tasks;
using EFConnect.Models.PhotoModel;

namespace EFConnect.Contracts
{
    public interface IPhotoService
    {
        Task<PhotoForReturn> AddPhotoForUser(int userId, PhotoForCreation photoDto);
        Task<bool> SaveAll();
    }
}