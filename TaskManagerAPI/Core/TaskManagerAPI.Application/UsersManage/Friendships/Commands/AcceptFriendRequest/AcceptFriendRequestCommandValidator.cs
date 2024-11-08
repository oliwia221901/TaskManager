using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
	{
		public AcceptFriendRequestCommandValidator()
		{
			RuleFor(x => x.AcceptFriendRequestDto.FriendshipId)
				.GreaterThan(0).WithMessage("FriendshipId must be grater than 0.");
		}
	}
}
