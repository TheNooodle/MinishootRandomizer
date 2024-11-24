namespace MinishootRandomizer;

public class MessageProcessingResult
{
    public bool Success { get; }
    public string ErrorMessage { get; }

    public MessageProcessingResult(bool success, string errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
}
