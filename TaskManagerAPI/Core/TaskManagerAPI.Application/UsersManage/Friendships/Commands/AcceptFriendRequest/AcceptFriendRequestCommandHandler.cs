using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, AcceptFriendshipVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;

        public AcceptFriendRequestCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<AcceptFriendshipVm> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var requesterId = await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken);

            var friendship = await _taskManagerDbContext.Friendships
                .Where(x => x.FriendshipId == request.AcceptFriendRequestDto.FriendshipId)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Friendship does not exist.");

            if (friendship.Status != FriendshipStatus.Pending)
            {
                return new AcceptFriendshipVm
                {
                    IsSuccess = false,
                    Status = friendship.Status
                };
            }

            if (friendship.RequesterId == requesterId)
                throw new BadRequestException("You cannot accept friend invitation from yourself.");

            friendship.Status = FriendshipStatus.Accepted;
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new AcceptFriendshipVm
            {
                IsSuccess = true,
                Status = FriendshipStatus.Accepted
            };
        }
    }
}
