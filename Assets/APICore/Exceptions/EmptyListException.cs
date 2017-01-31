using System;

public class EmptyListException : Exception
{
	public int Code = 400;
	public EmptyListException (string nameOfEmptyList)
		: base(nameOfEmptyList + " list is empty.")
	{
		
	}
}
