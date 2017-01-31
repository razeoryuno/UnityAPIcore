public class ErrorServerRequest
{
    public int code { get; private set; }
    public string message { get; private set; }

    public ErrorServerRequest(int code, string message = null)
    {
        this.code = code;

        if (string.IsNullOrEmpty(message))
            this.message = API_Code.messageDic[code];
        else
            this.message = message;
    }
}
