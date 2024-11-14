using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest
{
    public class DeclineFriendRequestCommandHandler : IRequestHandler<DeclineFriendRequestCommand, DeclineFriendRequestVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public DeclineFriendRequestCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<DeclineFriendRequestVm> Handle(DeclineFriendRequestCommand request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();

			var requesterId = await GetRequesterId(userName, cancellationToken);
			var friendship = await GetFriendship(request.DeclineFriendRequestDto.FriendshipId, cancellationToken);

			ValidateDeclineRequest(friendship, requesterId);

            friendship.Status = FriendshipStatus.Declined;
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new DeclineFriendRequestVm
			{
				IsSuccess = true,
				Status = FriendshipStatus.Declined
			};
		}

		public async Task<string> GetRequesterId(string userName, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == userName)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("Requester was not found.");
		}

		public async Task<Friendship> GetFriendship(int friendshipId, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.Friendships
				.Where(x => x.FriendshipId == friendshipId)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("Friendship was not found.");
		}


		public static void ValidateDeclineRequest(Friendship friendship, string requesterId)
		{
            if (friendship.RequesterId == requesterId)
                throw new BadRequestException("You cannot decline friend invitation from yourself.");

            if (friendship.Status != FriendshipStatus.Pending)
                throw new BadRequestException("Friendship status is not pending.");
        }
	}
}

