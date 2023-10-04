namespace Domain.Exceptions;

public sealed class InvalidRequestCodeToMapSuchAsErrorException : Exception
{
    public InvalidRequestCodeToMapSuchAsErrorException() : base("The response cannot be map as error.")
    {
    }
}
