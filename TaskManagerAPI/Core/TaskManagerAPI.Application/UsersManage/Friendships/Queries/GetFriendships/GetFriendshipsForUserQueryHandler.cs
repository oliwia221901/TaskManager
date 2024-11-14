using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetFriendshipsForUser;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class GetFriendshipsForUserQueryHandler : IRequestHandler<GetFriendshipsForUserQuery, FriendshipsForUserVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetFriendshipsForUserQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

        public async Task<FriendshipsForUserVm> Handle(GetFriendshipsForUserQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var currentUserId = await GetRequesterId(userName, cancellationToken);
            var friendships = await GetFriendshipsForUser(currentUserId, request.Status, cancellationToken);

            var friendshipsDto = await MapFriendshipsForUserToDto(friendships, currentUserId, cancellationToken);

            return new FriendshipsForUserVm
            {
                Friendships = friendshipsDto
            };
        }

        public async Task<string> GetRequesterId(string currentUserId, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == currentUserId)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("Requester was not found.");
		}

        public async Task<IEnumerable<Friendship>> GetFriendshipsForUser(string userId, FriendshipStatus status, CancellationToken cancellationToken)
        {
            var friendshipsExist = await _taskManagerDbContext.Friendships
                .AnyAsync(x => x.RequesterId == userId || x.FriendId == userId, cancellationToken);

            if (!friendshipsExist)
                throw new NotFoundException("Friendships were not found.");

            var friendships = await _taskManagerDbContext.Friendships
                .Where(x => (x.RequesterId == userId || x.FriendId == userId) && x.Status == status)
                .ToListAsync(cancellationToken);

            return friendships;
        }

        public async Task<IEnumerable<GetFriendshipsForUserDto>> MapFriendshipsForUserToDto(IEnumerable<Friendship> friendships, string userId, CancellationToken cancellationToken)
        {
            var userIds = friendships.SelectMany(f => new[] { f.RequesterId, f.FriendId }).Distinct();

            var users = await _taskManagerDbContext.AppUsers
                .Where(user => userIds.Contains(user.Id))
                .ToDictionaryAsync(user => user.Id, user => user.UserName, cancellationToken);

            return friendships.Select(f =>
            {
                var requesterName = users.GetValueOrDefault(f.RequesterId)
                    ?? throw new NotFoundException("Requester not found.");

                var friendName = users.GetValueOrDefault(f.FriendId)
                    ?? throw new NotFoundException("Friend not found.");

                return new GetFriendshipsForUserDto
                {
                    FriendshipId = f.FriendshipId,
                    RequesterName = requesterName,
                    FriendName = friendName,
                    CreatedAt = f.CreatedAt
                };
            });
        }
    }
}
