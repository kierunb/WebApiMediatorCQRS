using FluentValidation;
using MediatR;

namespace WebApiMediatorCQRS.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context))
        );

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage
            ))
            .ToList();

        if (errors.Count != 0)
        {
            // TODO: better handle error details
            throw new ValidationException("Validation Errors");
        }

        var response = await next();

        return response;
    }
}

internal class ValidationError
{
    private string _propertyName;
    private string _errorMessage;

    public ValidationError(string propertyName, string errorMessage)
    {
        _propertyName = propertyName;
        _errorMessage = errorMessage;
    }
}
