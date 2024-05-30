using Friends_Notify.Models;
using System.Linq.Expressions;

namespace Friends_Notify.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsers(Expression<Func<User, bool>> predicate);
        public Task<User> AddUser(User user);
        public Task<TrackUsers> StartTrackingUser(ulong userId, ulong userToTrackId);
    }
}
