namespace Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<Error> errors)
        : base("The validation failed.")
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get; }
}
