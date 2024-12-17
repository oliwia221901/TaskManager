using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships
{
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, SendFriendshipVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;

        public SendFriendRequestCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<SendFriendshipVm> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var requesterId = await GetRequesterId(userName, cancellationToken);
            var friendId = await GetFriendId(request, cancellationToken);

            var existingFriendship = await GetExistingFriendship(requesterId, friendId, cancellationToken);

            ValidateSendRequest(userName, request);

            if (existingFriendship != null)
            {
                existingFriendship.Status = FriendshipStatus.Pending;
                _taskManagerDbContext.Friendships.Update(existingFriendship);
            }
            else
            {
                var friendship = new Friendship
                {
                    RequesterId = requesterId,
                    FriendId = friendId,
                    Status = FriendshipStatus.Pending
                };

                _taskManagerDbContext.Friendships.Add(friendship);
            }

            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new SendFriendshipVm
            {
                IsSuccess = true,
                Status = FriendshipStatus.Pending
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

        public async Task<string> GetFriendId(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == request.SendFriendRequestDto.FriendName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Friend was not found.");
        }

        public async Task<Friendship?> GetExistingFriendship(string requesterId, string friendId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.Friendships
                .FirstOrDefaultAsync(x => x.RequesterId == requesterId && x.FriendId == friendId, cancellationToken);
        }

        public static void ValidateSendRequest(string userName, SendFriendRequestCommand request)
        {
            if (userName == request.SendFriendRequestDto.FriendName)
                throw new BadRequestException("You cannot send a friend request to yourself.");
        }
    }
}
