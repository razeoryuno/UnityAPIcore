using System;

public class MemberMethodException : Exception
{
	public int Code { get; private set; }
	
	public MemberMethodException(string methodName, int code, string message)
		: base(methodName + " error: " + message)
	{
		Code = code;
	}
}
