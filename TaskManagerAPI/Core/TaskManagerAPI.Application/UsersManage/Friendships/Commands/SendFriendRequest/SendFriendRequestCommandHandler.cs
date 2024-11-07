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

            var userId = await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserId was not found.");

            if (userId == request.SendFriendRequestDto.FriendId)
                throw new BadRequestException("You cannot send friend request to yourself.");

            var existingFriendship = await _taskManagerDbContext.Friendships
                .FirstOrDefaultAsync(f => (f.RequesterId == request.SendFriendRequestDto.FriendId || f.FriendId == request.SendFriendRequestDto.FriendId) &&
                                           f.Status == FriendshipStatus.Accepted, cancellationToken);

            if (existingFriendship is not null)
                return new SendFriendshipVm { IsSuccess = false, Status = FriendshipStatus.Accepted };

            var existingFriendUserName = await _taskManagerDbContext.AppUsers
                .AnyAsync(x => x.Id == request.SendFriendRequestDto.FriendId, cancellationToken);

            if (!existingFriendUserName)
                throw new NotFoundException("User was not found");

            var friendship = new Friendship
            {
                RequesterId = userId,
                FriendId = request.SendFriendRequestDto.FriendId,
                Status = FriendshipStatus.Pending
            };

            _taskManagerDbContext.Friendships.Add(friendship);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return new SendFriendshipVm { IsSuccess = true, Status = FriendshipStatus.Pending };
        }
    }
}
