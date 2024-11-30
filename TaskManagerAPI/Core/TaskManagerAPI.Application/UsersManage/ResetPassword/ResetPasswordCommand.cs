using MediatR;
using TaskManagerAPI.Application.Dtos.ResetPassword;

namespace TaskManagerAPI.Application.UsersManage.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public ResetPasswordDto ResetPasswordDto { get; set; }
    }
}

