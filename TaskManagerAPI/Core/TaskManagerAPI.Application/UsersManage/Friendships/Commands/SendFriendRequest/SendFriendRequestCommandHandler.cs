using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships
{
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, SendFriendshipVm>
    {
        private readonly ITaskManagerDbContext _context;

        public SendFriendRequestCommandHandler(ITaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<SendFriendshipVm> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f => (f.RequesterId == request.SendFriendRequestDto.FriendId || f.FriendId == request.SendFriendRequestDto.FriendId) &&
                                           f.Status == FriendshipStatus.Accepted, cancellationToken);

            if (existingFriendship != null)
                return new SendFriendshipVm { IsSuccess = false, Status = FriendshipStatus.Accepted };

            var friendship = new Friendship
            {
                RequesterId = request.SendFriendRequestDto.FriendId,
                FriendId = request.SendFriendRequestDto.FriendId,
                Status = FriendshipStatus.Pending
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync(cancellationToken);

            return new SendFriendshipVm { IsSuccess = true, Status = FriendshipStatus.Pending };
        }
    }
}
