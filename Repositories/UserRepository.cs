using Friends_Notify.Data;
using Friends_Notify.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Friends_Notify.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FriendsNotifyDbContext _context;
        public UserRepository(FriendsNotifyDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddUser(User user)
        {
            var addedUser = await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }

        public Task<List<User>> GetUsers(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Where(predicate).ToListAsync();
        }

        public async Task<TrackUsers> StartTrackingUser(ulong userId, ulong userToTrackId)
        {
            var user = await GetUser(userId);
            var userToTrack = await GetUser(userToTrackId);

            if (user == null || userToTrack == null ) 
            {
                throw new ArgumentException("provided user does not exist in db");
            }

            var trackUsers = await _context.TrackUsers
                .AddAsync(new TrackUsers { UserId = userId, TrackingUserId = userToTrackId });
            await _context.SaveChangesAsync();

            return trackUsers.Entity;
        }

        public Task<List<User>> GetUsersThatTracking(ulong userToTrackId)
        {
            return _context.TrackUsers
                .Include(userToTrack => userToTrack.User)
                .Where(trackUser => trackUser.TrackingUserId == userToTrackId)
                .Select(trackUser => trackUser.User)
                .ToListAsync();
        }

        public async Task<User> GetUser(ulong userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
