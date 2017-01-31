using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class WWWRequest : ServerRequest
{
    private WWW www;

    public override string url
    {
        get { return base.url; }
        set
        {
            this.TryCatch(() =>
            {
                ValidateUrlProtocolIsHttp(value);
                base.url = value;
            });
        }
    }

    public override bool isDone
    {
        get
        {
            if (www.IsNotNull())
                return www.isDone;

            throw new NullReferenceException("WWW is null.");
        }
    }

    public WWWRequest(MonoBehaviour monoBehaviour, string url, WWWRequestForm wwwFormRequest) :
        base(monoBehaviour, url, wwwFormRequest)
    {
        SetUrl(url);
    }

    public override IEnumerator RequestData()
    {
        yield return timeOutRunner.StartCoroutine(base.RequestData());
        CreateWWW();

        yield return timeOutRunner.StartCoroutine(WaitCheckTimeOut(() => isDone));
        SetData();
    }

    private void SetData()
    {
        if (isDone)
        {
            if (string.IsNullOrEmpty(www.error) && error.IsNull())
            {
                data = new JSONObject(www.text);
                CheckCode(data);
            }
            else
            {
                if (error.IsNull())
                    error = new ErrorServerRequest(API_Code.ERROR_CODE, www.error);
            }
        }
        else
        {
            if (error.IsNull())
                SetUnknownError();
        }
    }

    private void CreateWWW()
    {
        if (formServerRequest.IsNotNull())
        {
            var wwwFormRequest = formServerRequest as WWWRequestForm;
            var wwwForm = wwwFormRequest.wwwForm;
            www = new WWW(url, wwwForm);
        }
        else
        {
            www = new WWW(url);
        }
    }

    private void ValidateUrlProtocolIsHttp(string url)
    {
        string protocol = url.Split(':').First();
        bool protocolIsHttp = protocol.Substring(0, 4) == "http";

        if (!protocolIsHttp)
            throw new Exception("URL protocol is not http.");
    }

    public override void Dispose()
    {
        if (www.IsNotNull())
            www.Dispose();

        base.Dispose();
    }
}
