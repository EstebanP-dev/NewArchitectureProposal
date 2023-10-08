using FluentValidation;
using FluentValidation.Results;

namespace Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    protected ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        ValidationContext<TRequest> context = new(request);

        var validations = await Task.WhenAll(_validators
            .Select(validator => validator.ValidateAsync(context: context)))
            .ConfigureAwait(false);

        IEnumerable<Error> errors = (validations ?? Array.Empty<ValidationResult>())
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => Error.Conflict(description: validationFailure.ErrorMessage))
            .ToList();

        if (errors.Any())
        {
            throw new Exceptions.ValidationException(errors: errors);
        }

        TResponse response = await next()
            .ConfigureAwait(false);

        return response;
    }
}
