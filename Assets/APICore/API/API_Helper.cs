using System;
using System.Collections.Generic;

public static class API_Helper
{
    #region Get Helper
    public static void ValidateHasKey(JSONObject json, string key)
    {
        if (!json.HasField(key))
            throw new KeyNotFoundException("ValidateHasKey not found key \"" + key + "\" in json");
    }

    public static int GetCode(JSONObject json)
    {
        string codeKey = "code";
        ValidateHasKey(json, codeKey);

        return json[codeKey].GetAsInteger();
    }

    public static string GetMessage(JSONObject json)
    {
        string messageKey = "message";
        ValidateHasKey(json, messageKey);

        return json[messageKey].GetAsString();
    }

    public static JSONObject GetResult(JSONObject json)
    {
        string resultKey = "result";
        ValidateHasKey(json, resultKey);

        return json["result"];
    }
    #endregion

    #region Code
    public static bool CodeIsSuccess(int code)
    {
        return API_Code.CodeIsSuccess(code);
    }

    public static bool CodeIsErrorCode(int code)
    {
        return API_Code.CodeIsErrorCode(code);
    }

    public static bool CodeIsNoInternetConnection(int code)
    {
        return API_Code.CodeIsNoInternetConnection(code);
    }

    public static bool CodeIsTimeOut(int code)
    {
        return API_Code.CodeIsTimeOut(code);
    }
    #endregion
}
