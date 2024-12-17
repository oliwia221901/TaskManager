using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.Email;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/email")]
    public class EmailController : BaseController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
