using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [ApiController]

    public abstract class BaseController : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}