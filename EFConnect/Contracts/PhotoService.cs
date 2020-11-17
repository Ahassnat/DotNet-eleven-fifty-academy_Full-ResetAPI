using System.Threading.Tasks;
using CloudinaryDotNet;
using EFConnect.Data;
using EFConnect.Helpers;
using EFConnect.Models.PhotoModel;
using Microsoft.Extensions.Options;

namespace EFConnect.Contracts
{
    public class PhotoService : IPhotoService
    {
        private readonly IUserService _userService;
        private readonly IOptions<CloudinarySetting> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly EFConnectContext _context;

        public PhotoService(IUserService userService,
            IOptions<CloudinarySetting> cloudinaryConfig,
            EFConnectContext context)
        {
            _context = context;
            _userService = userService;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public Task<PhotoForReturn> AddPhotoForUser(int userId, PhotoForCreation photoDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveAll()
        {
            throw new System.NotImplementedException();
        }
    }
}