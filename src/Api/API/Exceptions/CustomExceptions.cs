public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}



public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}



public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}