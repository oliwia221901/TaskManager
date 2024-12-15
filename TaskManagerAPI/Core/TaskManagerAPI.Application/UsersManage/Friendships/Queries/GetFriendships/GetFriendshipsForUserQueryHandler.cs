using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.UsersManage.GetFriendships;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class GetFriendshipsQueryHandler : IRequestHandler<GetFriendshipsQuery, FriendshipsVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetFriendshipsQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

        public async Task<FriendshipsVm> Handle(GetFriendshipsQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var currentUserId = await GetRequesterId(userName, cancellationToken);
            var friendships = await GetFriendships(currentUserId, request.Status, cancellationToken);

            var friendshipsDto = await MapFriendshipsToDto(friendships, cancellationToken);

            return new FriendshipsVm
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

        public async Task<IEnumerable<Friendship>> GetFriendships(string userId, FriendshipStatus status, CancellationToken cancellationToken)
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

        public async Task<IEnumerable<GetFriendshipsDto>> MapFriendshipsToDto(IEnumerable<Friendship> friendships, CancellationToken cancellationToken)
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

                return new GetFriendshipsDto
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
