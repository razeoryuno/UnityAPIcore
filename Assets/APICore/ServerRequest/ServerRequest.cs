using System;
using System.Collections;
using UnityEngine;

public abstract class ServerRequest : IDisposable
{
    private static float _defaultTimeOut = 30f;
    public static float defaultTimeOut
    {
        get { return _defaultTimeOut; }
        set { _defaultTimeOut = value; }
    }

    public virtual string url { get; set; }
    public ServerRequestForm formServerRequest { get; set; }
    public float timeOut { get; set; }

    public virtual bool isDone { get; protected set; }
    public ErrorServerRequest error { get; protected set; }
    public JSONObject data { get; protected set; }

    protected TimeOutRunner timeOutRunner { get; private set; }

    public ServerRequest(MonoBehaviour monoBehaviour, string url, ServerRequestForm formServerRequest = null)
    {
        if (monoBehaviour.IsNotNull())
            timeOutRunner = monoBehaviour.gameObject.AddComponent<TimeOutRunner>();

        if (formServerRequest.IsNotNull())
            this.formServerRequest = formServerRequest;

        timeOut = defaultTimeOut;
    }

    protected void SetUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
            this.url = url;
    }

    private void ValidateTimeOutRunner()
    {
        if (timeOutRunner.IsNull())
            throw new NullReferenceException("TimeOutRunner is null.");
    }

    public virtual IEnumerator RequestData()
    {
        ValidateTimeOutRunner();
        timeOutRunner.StartCountTimeOut(timeOut);

        var isNotConnectTheInternet = !IsNetworkConnected();
        if (isNotConnectTheInternet)
        {
            timeOutRunner.StopCountTimeOut();
            error = new ErrorServerRequest(API_Code.NO_INTERNET_CONNECTION_CODE);
            isDone = true;
            yield break;
        }
    }

	public static bool IsNetworkConnected ()
	{
		bool isConnected = (Application.internetReachability != NetworkReachability.NotReachable) ? true : false;

		return isConnected;
	}

    protected IEnumerator WaitCheckTimeOut(Func<bool> isDone)
    {
        timeOutRunner.ResumeCountTimeOut();

        while (!timeOutRunner.isTimeOut && !isDone())
            yield return null;

        if (timeOutRunner.isTimeOut)
            error = new ErrorServerRequest(API_Code.TIME_OUT_CODE);

        timeOutRunner.StopCountTimeOut();
    }

    protected void CheckCode(JSONObject data)
    {
        int code = API_Helper.GetCode(data);

        if (!API_Code.CodeIsNormalSuccess(code))
        {
            string errorMessage = API_Helper.GetMessage(data);
            error = new ErrorServerRequest(code, errorMessage);
        }
    }

    protected void SetUnknownError()
    {
        error = new ErrorServerRequest(API_Code.ERROR_CODE);
        isDone = true;
    }

    public virtual void Dispose()
    {
        timeOutRunner.Dispose();
        UnityEngine.Object.Destroy(timeOutRunner);
    }
}
