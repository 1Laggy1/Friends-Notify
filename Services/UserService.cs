using Friends_Notify.Models;
using Friends_Notify.Repositories;

namespace Friends_Notify.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public Task<TrackUsers> StartTrackingUser(ulong userId, ulong userToTrackId)
        {
            return _userRepository.StartTrackingUser(userId, userToTrackId);
        }

        public Task<User> GetUser(ulong userId)
        {
            return _userRepository.GetUser(userId);
        }
    }
}
