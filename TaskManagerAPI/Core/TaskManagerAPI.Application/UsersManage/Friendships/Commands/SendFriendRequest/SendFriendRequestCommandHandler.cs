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

            if (userName == request.SendFriendRequestDto.FriendName)
                throw new BadRequestException("You cannot send friend request to yourself.");

            var requesterId = await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Requester was not found.");

            var friendId = await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == request.SendFriendRequestDto.FriendName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Friend was not found.");

            var existingFriendship = await _taskManagerDbContext.Friendships
                .FirstOrDefaultAsync(f => (f.RequesterId == requesterId || f.FriendId == friendId) &&
                                           f.Status == FriendshipStatus.Accepted || f.Status == FriendshipStatus.Pending,
                                           cancellationToken);

            if (existingFriendship is not null)
                return new SendFriendshipVm
                {
                    IsSuccess = false,
                    Status = existingFriendship.Status
                };

            var friendship = new Friendship
            {
                RequesterId = requesterId,
                FriendId = friendId,
                Status = FriendshipStatus.Pending
            };

            _taskManagerDbContext.Friendships.Add(friendship);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new SendFriendshipVm { IsSuccess = true, Status = FriendshipStatus.Pending };
        }
    }
}
