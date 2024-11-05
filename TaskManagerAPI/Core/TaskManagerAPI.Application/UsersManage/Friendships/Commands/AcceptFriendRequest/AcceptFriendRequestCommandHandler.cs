using MediatR;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, AcceptFriendshipVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;

        public AcceptFriendRequestCommandHandler(ITaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public async Task<AcceptFriendshipVm> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendship = await _taskManagerDbContext.Friendships.FindAsync(
                new object[] { request.AcceptFriendRequestDto.FriendshipId }, cancellationToken)
                ?? throw new NotFoundException("Relation does not exist.");

            if (friendship.Status != FriendshipStatus.Pending)
            {
                return new AcceptFriendshipVm
                {
                    IsSuccess = false,
                    Status = friendship?.Status ?? FriendshipStatus.Declined
                };
            }

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
