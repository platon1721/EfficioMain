namespace Efficio.BLL.Contracts.Exceptions;

/// <summary>
/// Base exception for all business logic errors.
/// Middleware maps these to appropriate HTTP status codes.
/// </summary>
public abstract class BusinessException : Exception
{
    public abstract int StatusCode { get; }

    protected BusinessException(string message) : base(message) { }
    protected BusinessException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Entity was not found. Maps to 404.
/// </summary>
public class NotFoundException : BusinessException
{
    public override int StatusCode => 404;

    public NotFoundException(string entity, object id)
        : base($"{entity} with id '{id}' was not found") { }

    public NotFoundException(string message) : base(message) { }
}

/// <summary>
/// Business rule violation. Maps to 400.
/// </summary>
public class ValidationException : BusinessException
{
    public override int StatusCode => 400;
    public IDictionary<string, string[]>? Errors { get; }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string field, string error) : base(error)
    {
        Errors = new Dictionary<string, string[]> { { field, new[] { error } } };
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }
}

/// <summary>
/// Authentication failed. Maps to 401.
/// </summary>
public class UnauthorizedException : BusinessException
{
    public override int StatusCode => 401;

    public UnauthorizedException(string message = "Invalid credentials")
        : base(message) { }
}

/// <summary>
/// User lacks permission for the operation. Maps to 403.
/// </summary>
public class ForbiddenException : BusinessException
{
    public override int StatusCode => 403;

    public ForbiddenException(string message = "You do not have permission to perform this action")
        : base(message) { }
}

/// <summary>
/// Duplicate entity or conflict. Maps to 409.
/// </summary>
public class ConflictException : BusinessException
{
    public override int StatusCode => 409;

    public ConflictException(string message) : base(message) { }

    public ConflictException(string entity, string field, string value)
        : base($"{entity} with {field} '{value}' already exists") { }
}