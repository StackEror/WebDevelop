using FluentValidation;
using MediatR;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if(failures.Count() != 0)
        {
            var validationErrors =
                failures
                .GroupBy(p => p.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(",", g.Select(x => x.ErrorMessage))
                    );

            var errorMessage = "One or more validation errors occurred.";

            return (TResponse)(object)new Response<Dictionary<string, string>>(validationErrors)
            {
                IsSuccess = false,
                Message = errorMessage
            };
        }
        return await next();
    }
}
