namespace Domain.Features.Auth.Errors;

public static class AuthErrors
{
    private static readonly string _baseCode = "Authentication";

    public static class Login
    {
        private static readonly string _loginCode = $"{_baseCode}.Login";

        public static Error NullValue => Error
            .NotFound(
                code: $"{_loginCode}.{nameof(NullValue)}",
                description: LocalizationResourceHelper.Current["AuthErrorsResource.CustomUnexpectedError"]);

        public static Error UnAuthorized => Error
            .Unauthorized(description: LocalizationResourceHelper.Current["AuthErrorsResource.UnAuthorizedError"]);
    }
}
