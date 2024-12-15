using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeleteFriend
{
	public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommand>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public DeleteFriendCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();

			var userId = await GetUserId(userName, cancellationToken);

			var friendship = await GetFriendship(request, userId, cancellationToken);

			_taskManagerDbContext.Friendships.Remove(friendship);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		public async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == userName)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("UserId was not found.");
		}

        public async Task<Friendship> GetFriendship(DeleteFriendCommand request, string userId, CancellationToken cancellationToken)
        {
            var friendship = await _taskManagerDbContext.Friendships
                .SingleOrDefaultAsync(x => x.FriendshipId == request.FriendshipId, cancellationToken)
				?? throw new NotFoundException("FriendshipId was not found.");

            if (friendship.FriendId != userId && friendship.RequesterId != userId)
                throw new BadRequestException("User is not associated with this friendship.");

            if (friendship.Status != FriendshipStatus.Accepted)
                throw new BadRequestException("Friendship has bad status.");

            return friendship;
        }
	}
}
