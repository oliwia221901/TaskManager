using TaskManagerAPI.Application.Dtos.Email;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface IEmailService
	{
		void SendEmail(EmailDto request);
	}
}

