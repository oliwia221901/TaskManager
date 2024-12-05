using MediatR;
using TaskManagerAPI.Application.Dtos.ChangePassword;

namespace TaskManagerAPI.Application.UsersManage.ResetPassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public ChangePasswordDto ChangePasswordDto { get; set; }
    }
}

