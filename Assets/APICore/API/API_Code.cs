using System.Collections.Generic;

public class API_Code
{
	public const int NORMAL_SUCCESS_CODE = 200;
	public const int NO_CONTENT_UPDATED_CODE = 201;
	public const int ERROR_CODE = 500;
	public const int NO_INTERNET_CONNECTION_CODE = 501;
	public const int TIME_OUT_CODE = 503;

	public const int SERVER_OFFLINE_CODE = 700;

	public const int INVALID_API_KEY_CODE = 100;
	public const int INVALID_SDK_TOKEN_CODE = 101;
	public const int INVALID_AUTH_TOKEN_CODE = 102;
	public const int INVALID_API_SIGNATURE_CODE = 103;
	public const int DUPLICATE_NONCE_CODE = 104;
	public const int EXPIRED_TOKEN_CODE = 105;

	public static Dictionary<int, string> messageDic { get; private set; }

	static API_Code ()
	{
		messageDic = new Dictionary<int, string> ();

		messageDic.Add (NORMAL_SUCCESS_CODE, "Success");
		messageDic.Add (ERROR_CODE, "Unknown Error");
		messageDic.Add (NO_INTERNET_CONNECTION_CODE, "No Internet Connection");
		messageDic.Add (TIME_OUT_CODE, "Time Out");
	}

	public static bool CodeIsNormalSuccess (int code)
	{
		return code == NORMAL_SUCCESS_CODE;
	}

	public static bool CodeIsNoContentUpdated (int code)
	{
		return code == NO_CONTENT_UPDATED_CODE;
	}

	public static bool CodeIsSuccess (int code)
	{
		return code / 100 == 2;
	}

	public static bool CodeIsErrorCode (int code)
	{
		return code == ERROR_CODE;
	}

	public static bool CodeIsNoInternetConnection (int code)
	{
		return code == NO_INTERNET_CONNECTION_CODE;
	}

	public static bool CodeIsTimeOut (int code)
	{
		return code == TIME_OUT_CODE;
	}

	public static bool CodeIsServerOffline (int code)
	{
		return code == SERVER_OFFLINE_CODE;
	}

	public static bool CodeIsInvalidApiKey (int code)
	{
		return code == INVALID_API_KEY_CODE;
	}

	public static bool CodeIsInvalidSdkToken (int code)
	{
		return code == INVALID_SDK_TOKEN_CODE;
	}

	public static bool CodeIsInvalidAuthToken (int code)
	{
		return code == INVALID_AUTH_TOKEN_CODE;
	}

	public static bool CodeIsInvalidAPISignature (int code)
	{
		return code == INVALID_API_SIGNATURE_CODE;
	}

	public static bool CodeIsDuplicateNonce (int code)
	{
		return code == DUPLICATE_NONCE_CODE;
	}

	public static bool CodeIsExpiredToken (int code)
	{
		return code == EXPIRED_TOKEN_CODE;
	}
}