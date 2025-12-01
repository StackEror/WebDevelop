namespace WebDevelopment.Shared.Response;

public class Response
{
    public Response(string message, bool isSucces)
    {
        Message = message;
        IsSuccess = isSucces;
    }

    public Response(string message)
    {
        Message = message;
    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Response()
    {
        IsSuccess = true;
    }
#pragma warning restore CS8618

    public bool IsSuccess { set; get; }
    public string Message { get; set; }
}

public class Response<T> : Response
{
    public T Data { get; set; }

    public Response(T data)
    {
        Data = data;
    }

    /// <summary>
	/// Creates a success response with the provided data.
	/// </summary>
	/// <param name="data">The data to include in the response.</param>
	/// <returns>A success response.</returns>
    public static Response<T> Succes(T data)
    {
        return new Response<T>(data)
        {
            IsSuccess = true,
            Data = data,
            Message = null!
        };
    }

    
    /// <summary>
    /// Creates a failure response with the provided error message.
    /// </summary>
    /// <param name="errorMessage">The error message to include in the response.</param>
    /// <returns>A failure response.</returns>
    public static Response<T> Failure(string errorMessage)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return new Response<T>(default)
#pragma warning restore CS8604
        {
            IsSuccess = false,
            Data = default!,
            Message = errorMessage
        };
    }
}
