namespace Application.DTOs.Common;

public class ErrorResponse
{
    public ErrorResponse(string message, string errorCode, Dictionary<string, string[]>? details = null)
    {
        Message = message;
        ErrorCode = errorCode;
        Details = details;
    }
    public string Message { get; set; }

    public string ErrorCode { get; set; }

    public Dictionary<string, string[]>? Details { get; set; }
}