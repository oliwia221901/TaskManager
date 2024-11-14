using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage;
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

            var requesterId = await GetRequesterId(userName, cancellationToken);

            var friendship = await GetFriendship(request.AcceptFriendRequestDto.FriendshipId, cancellationToken);

            ValidateAcceptRequest(friendship, requesterId);

            friendship.Status = FriendshipStatus.Accepted;
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new AcceptFriendshipVm
            {
                IsSuccess = true,
                Status = FriendshipStatus.Accepted
            };
        }

        public async Task<string> GetRequesterId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("User was not found.");
        }

        public async Task<Friendship> GetFriendship(int friendshipId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.Friendships
                .Where(x => x.FriendshipId == friendshipId)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Friendship was not found.");
        }

        public static void ValidateAcceptRequest(Friendship friendship, string requesterId)
        {
            if (friendship.RequesterId == requesterId)
                throw new BadRequestException("You cannot accept friend invitation from yourself.");

            if (friendship.Status != FriendshipStatus.Pending)
                throw new BadRequestException("Friendship status is not pending.");
        }
    }
}
