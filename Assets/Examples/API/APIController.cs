using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class APIController : MonoInstance<APIController>
{
	private const float WAIT_TIME_BETWEEN_API = 0.1f;

	Queue<IAPI_DataCallback> apiMethodPromiseQueue;

	IAPI_DataCallback currentApiMethodPromise;

	public bool isUserReadInformation = false;

	protected override void Awake ()
	{
		base.Awake ();

		apiMethodPromiseQueue = new Queue<IAPI_DataCallback> (
			new List<IAPI_DataCallback> () {
				ConstantsAPI.Instance
			}
		);

		DequeueApiMethodPromiseQueue ();
	}

	void Start ()
	{
		StartCoroutine (PlayLoadingAnimation (OnLoadingStart));
	}

	private IEnumerator PlayLoadingAnimation (Action callback)
	{
		yield return new WaitForSeconds (WAIT_TIME_BETWEEN_API);
		callback ();
	}

	private void OnLoadingStart ()
	{
		LoadAPI_DataSeqeunce ();
	}

	void DequeueApiMethodPromiseQueue ()
	{
		if (apiMethodPromiseQueue.Count > 0)
			currentApiMethodPromise = apiMethodPromiseQueue.Dequeue ();
	}

	private void LoadAPI_DataSeqeunce ()
	{
		Action onLoadApiSuccess = OnLoadApiSuccess;
		Action onLoadingComplete = OnLoadingComplete;

		var hasQueueInApiMethodPromiseQueue = apiMethodPromiseQueue.Count > 0;
		var onSuccess = hasQueueInApiMethodPromiseQueue ? onLoadApiSuccess : onLoadingComplete;

		CurrentApiMethodGetData (onSuccess);
	}

	private void CurrentApiMethodGetData (Action onSuccess)
	{
		currentApiMethodPromise.DataCallback ()
            .Done (onSuccess, OnLoadingApiFail);
	}

	private void OnLoadApiSuccess ()
	{
		DequeueApiMethodPromiseQueue ();
		StartWaitTimeBetweenApi ();
	}

	private void OnLoadingApiFail (Exception exception)
	{
		if (exception.IsNotNull ()) {
			if (exception is DataMethodException) {
				var dataMethodException = exception as DataMethodException;
				Debug.LogError ("dataMethodException.Code: " + dataMethodException.Code);
				Debug.Log ("Mesage " + dataMethodException.Message);
			} else if (exception is MemberMethodException) {
				var memberDataMethodException = exception as MemberMethodException;
				Debug.LogError ("memberDataMethodException.Code: " + memberDataMethodException.Code);
			}
		} else {
			int errorCode = API_Code.ERROR_CODE;
			Debug.LogError ("errorCode: " + errorCode);
		}
	}

	private void StartWaitTimeBetweenApi ()
	{
		Action callback = LoadAPI_DataSeqeunce;
		StartCoroutine ("WaitTimeBetweenApi", callback);
	}

	private IEnumerator WaitTimeBetweenApi (Action callback)
	{
		yield return new WaitForSeconds (WAIT_TIME_BETWEEN_API);
		callback ();
	}

	public void OnLoadingComplete ()
	{
		try {
			Debug.Log ("Loading Complete");

		} catch (Exception e) {
			
			Debug.LogError (e.Message);
		}
	}
}
