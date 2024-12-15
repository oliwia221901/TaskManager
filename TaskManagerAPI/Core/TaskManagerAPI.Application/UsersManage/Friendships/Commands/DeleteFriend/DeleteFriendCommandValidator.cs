using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeleteFriend
{
    public class DeleteFriendCommandValidator : AbstractValidator<DeleteFriendCommand>
	{
		public DeleteFriendCommandValidator()
		{
			RuleFor(x => x.FriendshipId)
				.GreaterThan(0).WithMessage("FriendshipId must be grater than 0.");
        }
	}
}
