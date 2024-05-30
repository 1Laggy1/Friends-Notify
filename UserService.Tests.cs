using Friends_Notify.Models;
using Friends_Notify.Repositories;
using Friends_Notify.Services;
using Moq;
using Xunit;

namespace Friends_Notify
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task AddUser_ShouldReturnCorrectUser_WhenUserIsAdded()
        {
            var user = new User { Id = 1 };
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(user);

            var result = await _userService.AddUser(user);

            Assert.Equal(user, result);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task StartTrackingUser_ShouldReturnCorrectUsers_WhenUserGetsTracked()
        {
            var userToTrack = new User { Id = 1 };
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(userToTrack);

            var userWhoTrack1 = new User { Id = 11 };
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(userWhoTrack1);

            var userWhoTracks2 = new User { Id = 12 };
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(userWhoTracks2);

            await _userService.AddUser(userToTrack);
            await _userService.AddUser(userWhoTrack1);
            await _userService.AddUser(userWhoTracks2);

            var expectedResult = new List<User> { userWhoTrack1, userWhoTracks2 };
            _userRepositoryMock.Setup(repo => repo.GetUsersThatTracking(It.IsAny<ulong>())).ReturnsAsync(expectedResult);

            var result = await _userService.GetUsersThatTracking(userToTrack.Id);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task StartTrackingUser_ShouldReturnTrackUsers_WhenUserStartsTracking()
        {
            var userId = 1ul;
            var userToTrackId = 2ul;

            var user = new User { Id = userId };
            var userToTrack = new User { Id = userToTrackId };

            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetUser(userToTrackId)).ReturnsAsync(userToTrack);

            var expectedTrackUsers = new TrackUsers { UserId = userId, TrackingUserId = userToTrackId };

            _userRepositoryMock.Setup(repo => repo.StartTrackingUser(userId, userToTrackId)).ReturnsAsync(expectedTrackUsers);

            var result = await _userService.StartTrackingUser(userId, userToTrackId);

            Assert.Equal(expectedTrackUsers, result);
        }
    }
}
