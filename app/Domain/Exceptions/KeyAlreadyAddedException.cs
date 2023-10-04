namespace Domain.Exceptions;

public sealed class KeyAlreadyAddedException : Exception
{
    public KeyAlreadyAddedException() : base("The key was already added.")
    {
    }
}
