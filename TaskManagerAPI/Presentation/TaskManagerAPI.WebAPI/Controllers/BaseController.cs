using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.WebAPI.Controllers
{

    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}