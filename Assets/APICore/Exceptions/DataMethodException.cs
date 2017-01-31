using System;

public class DataMethodException : Exception
{
    public int Code { get; private set; }

    public DataMethodException(string methodName, int code, string message)
		: base(methodName + " error: " + message + ".")
	{
        Code = code;
    }
}
