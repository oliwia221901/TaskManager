using FluentValidation;
using MediatR;

namespace TaskManagerAPI.Application.Common.Behaviors
{
    public class ValidationBehaviour<TRequset, TResponse> : IPipelineBehavior<TRequset, TResponse>
        where TRequset : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequset>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequset>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequset request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequset>(request);

                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var listOfErrorMessages = new List<string>();

                    foreach (var error in failures)
                    {
                        listOfErrorMessages.Add(error.ErrorMessage);
                    }

                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }

}
