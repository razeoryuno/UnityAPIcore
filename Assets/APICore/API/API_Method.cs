using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class API_Method
{
    private static string apiKey;
    private static string apiVersion;

    public static string ApiKey
    {
        get { return apiKey; }
    }

    public static string ApiVersion
    {
        get { return apiVersion; }
    }

    public static Dictionary<int, Action<int, string>> defaultActionHandleErrorCodeDict { get; set; }

    public static void Init(string apiKey, string apiVersion)
    {
        API_Method.apiKey = apiKey;
        API_Method.apiVersion = apiVersion;
    }

    public MonoBehaviour monoBehaviour { get; set; }
    public Dictionary<int, Action<int, string>> actionHandleErrorCodeDict { get; set; }

    public API_Method()
    {
        if (defaultActionHandleErrorCodeDict.IsNotNull())
            actionHandleErrorCodeDict = new Dictionary<int, Action<int, string>>(defaultActionHandleErrorCodeDict);
    }

    public virtual void AddDefaultToServerRequestForm(ServerRequestForm serverRequestForm)
    {
        serverRequestForm.AddField("apiKey", ApiKey);
        serverRequestForm.headers["transport"] = "websocket";
        serverRequestForm.methodName = methodName;
    }

    public string methodServerUrl
    {
        get
        {
            string methodUrlPath = methodName.Replace(".", "/");
            string resultPath = API_Config.Instance.GetHost() + methodUrlPath;

            if (API_Config.Instance.isDebug)
                Debug.Log("Request url :: " + resultPath);

            return resultPath;
        }
    }

    public void OnRequestDataSuccess(int code, string message)
    {
        OnRequestDataSuccess(code, message, null);
    }

    public abstract void OnRequestDataSuccess(int code, string message, JSONObject result);

    public abstract void OnRequestDataFailed(int code, string message = null);

    public abstract string methodName { get; }

    public virtual void ValidateServerRequestForm(ServerRequestForm serverRequestForm)
    {
        ValidateServerRequestFormByKey(serverRequestForm, "apiKey");
    }

    public void ValidateServerRequestFormByKey(ServerRequestForm serverRequestForm, string key)
    {
        if (!serverRequestForm.fields.HasField(key))
            throw new KeyNotFoundException("Server request form not found " + key + " field.");
    }

    private void ValidateHasMonoBehaviour()
    {
        if (monoBehaviour.IsNull())
            throw new NullReferenceException("MonoBehaviour is null.");
    }

    protected IEnumerator RequestData(ServerRequest serverRequest)
    {
        ValidateServerRequestForm(serverRequest.formServerRequest);
        ValidateHasMonoBehaviour();
		if (API_Config.Instance.isDebug) {
			Debug.Log ("Request data :: " + serverRequest.formServerRequest.fields.Print(true));
		}
        yield return monoBehaviour.StartCoroutine(serverRequest.RequestData());
        InvokeRequestCallback(serverRequest);
        serverRequest.Dispose();
    }

    public void InvokeRequestCallback(ServerRequest serverRequest)
    {
        if (serverRequest.isDone)
        {
            if (serverRequest.data.IsNotNull())
            {
                DebugModeLog(serverRequest);
                DataParser(serverRequest.data);
                return;
            }
        }

        int code;
        string message;
        if (serverRequest.error.IsNotNull())
        {
            code = serverRequest.error.code;
            message = serverRequest.error.message;

            if (HasErrorCodeInActionHandleErrorCodeDict(code))
                actionHandleErrorCodeDict[code](code, message);
        }
        else
        {
            code = API_Code.ERROR_CODE;
            message = API_Code.messageDic[API_Code.ERROR_CODE];
        }

        OnRequestDataFailed(code, message);
    }

    private bool HasErrorCodeInActionHandleErrorCodeDict(int code)
    {
        return actionHandleErrorCodeDict.IsNotNull() && actionHandleErrorCodeDict.ContainsKey(code);
    }

#pragma warning disable
    private void DebugModeLog(ServerRequest serverRequest)
    {
        if (API_Config.Instance.isDebug)
        {
            Debug.Log("Method name :: " + methodName);
            Debug.Log("Response data :: " + serverRequest.data.Print(true));
        }
    }
#pragma warning restore 

    protected virtual void DataParser(JSONObject jsonObject)
    {
      
        int code = API_Helper.GetCode(jsonObject);
        string message = API_Helper.GetMessage(jsonObject);
        JSONObject result = null;

        bool through = true;
        if (HasErrorCodeInActionHandleErrorCodeDict(code))
        {
            through = false;
            actionHandleErrorCodeDict[code](code, message);
        }

        if (through)
        {
            if (jsonObject.HasField("result"))
                result = API_Helper.GetResult(jsonObject);

            OnRequestDataSuccess(code, message, result);
        }
    }
}
