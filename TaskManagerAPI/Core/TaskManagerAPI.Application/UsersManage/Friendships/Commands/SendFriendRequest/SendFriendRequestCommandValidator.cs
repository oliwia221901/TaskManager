using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest
{
    public class SendFriendRequestCommandValidator : AbstractValidator<SendFriendRequestCommand>
	{
		public SendFriendRequestCommandValidator()
		{
			RuleFor(x => x.SendFriendRequestDto.FriendId)
				.NotEmpty().WithMessage("FriendId is requred.");
		}
	}
}
