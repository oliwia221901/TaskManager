using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class GetFriendshipsQueryValidator : AbstractValidator<GetFriendshipsQuery>
    {
        public GetFriendshipsQueryValidator()
        {
            RuleFor(query => query.Status)
                .IsInEnum().WithMessage("The provided status is not a valid FriendshipStatus value.");
        }
    }
}
