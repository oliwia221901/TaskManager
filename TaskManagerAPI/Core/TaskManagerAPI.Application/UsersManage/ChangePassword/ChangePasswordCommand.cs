using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.ChangePassword;

namespace TaskManagerAPI.Application.UsersManage.ResetPassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public ChangePasswordDto ChangePasswordDto { get; set; }
    }
}

