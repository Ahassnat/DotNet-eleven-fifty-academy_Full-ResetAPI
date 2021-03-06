using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFConnect.Data;
using EFConnect.Helpers;
using EFConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace EFConnect.Contracts
{
    public class UserService : IUserService
    {
        private readonly EFConnectContext _context;
        public UserService(EFConnectContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<UserForDetail> GetUser(int id)
        {
            var user = await _context.Users
                            .Include(p => p.Photos)
                            .FirstOrDefaultAsync(u => u.Id == id);              // 1

            if (user == null)
                return null;

            var photosToReturn = new List<PhotoForDetail>();                    // 2

            foreach (var photo in user.Photos)                                  // 3
            {
                var userPhoto = new PhotoForDetail
                {
                    Id = photo.Id,
                    Url = photo.Url,
                    Description = photo.Description,
                    DateAdded = photo.DateAdded,
                    IsMain = photo.IsMain
                };

                photosToReturn.Add(userPhoto);
            }

            var userToReturn = new UserForDetail                                // 4
            {
                Id = user.Id,
                Username = user.Username,
                Specialty = user.Specialty,
                Age = user.DateOfBirth.CalculateAge(),                          //  (extension method)
                KnownAs = user.KnownAs,
                Created = user.Created,
                LastActive = user.LastActive,
                Introduction = user.Introduction,
                LookingFor = user.LookingFor,
                Interests = user.Interests,
                City = user.City,
                State = user.State,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                Photos = photosToReturn
            };

            return userToReturn;                                                // 5
        }

        public async Task<IEnumerable<UserForList>> GetUsers()
        {
            var users = await _context.Users
                            .Include(p => p.Photos)
                            .Select(
                                e => new UserForList
                                {
                                    Id = e.Id,
                                    Username = e.Username,
                                    Specialty = e.Specialty,
                                    Age = e.DateOfBirth.CalculateAge(),
                                    KnownAs = e.KnownAs,
                                    Created = e.Created,
                                    LastActive = e.LastActive,
                                    City = e.City,
                                    State = e.State,
                                    PhotoUrl = e.Photos.FirstOrDefault(p => p.IsMain).Url
                                }
                            )
                            .ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUser(int id, UserForUpdate model)
        {
            var user = await _context.Users
                                .FirstOrDefaultAsync(u => u.Id == id);

            user.Introduction = model.Introduction;
            user.LookingFor = model.LookingFor;
            user.Interests = model.Interests;
            user.City = model.City;
            user.State = model.State;

            return await _context.SaveChangesAsync() == 1;
        }
    }
}