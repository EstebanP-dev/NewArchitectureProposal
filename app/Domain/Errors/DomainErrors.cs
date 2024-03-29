﻿using Domain.Features.Auth.Errors;
using System.Net;
using System.Text.RegularExpressions;

namespace Domain.Errors;

public static partial class DomainErrors
{
    public static Error UnExceptedError => Error
        .Unexpected(description: LocalizationResourceHelper.Current["DomainErrorResource.UnexpectedError"]);
    
    public static Error CancelledOperation => Error
        .Unexpected(
            code: "General.Cancelled",
            description: LocalizationResourceHelper.Current["DomainErrorResource.UnexpectedError"]);

    [GeneratedRegex(
        pattern: "[[\\]\"'\']|[\n]{2}",
        options: RegexOptions.Compiled)]
    private static partial Regex JsonFormatRegex();

    public static Error MapFromHttpResultMessage(HttpStatusCode statusCode, string message)
    {
        return statusCode switch
        {
            HttpStatusCode.Unauthorized => AuthErrors.Login.UnAuthorized,
            HttpStatusCode.BadRequest => Error.NotFound(description: JsonFormatRegex().Replace(message, "")),
            HttpStatusCode.Forbidden => UnExceptedError,
            HttpStatusCode.NotFound => UnExceptedError,
            HttpStatusCode.InternalServerError => UnExceptedError,
            _ => throw new InvalidRequestCodeToMapSuchAsErrorException(),
        };
    }

    public static Error MapFromListOfErrors(IEnumerable<Error> errors)
    {
        return errors.FirstOrDefault();
    }
}
