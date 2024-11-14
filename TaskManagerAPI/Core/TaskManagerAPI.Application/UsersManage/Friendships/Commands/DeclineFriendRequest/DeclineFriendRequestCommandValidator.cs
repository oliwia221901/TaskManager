using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest
{
    public class DeclineFriendRequestCommandValidator : AbstractValidator<DeclineFriendRequestCommand>
	{
		public DeclineFriendRequestCommandValidator()
		{
			RuleFor(x => x.DeclineFriendRequestDto.FriendshipId)
				.GreaterThan(0).WithMessage("FriendshipId must be grater than 0.");
		}
	}
}

