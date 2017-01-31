using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ConstantsMethodData : DataAPI_Method
{
	public class Code : API_Code
	{
		public const int EMPTY_LIST_CODE = 400;
	}
	
	public delegate void DataCallback(int code,string message,Dictionary<string,string> constantData = null);
	
	public DataCallback callback { get; set; }
	
	public override string methodName
	{
		get { return "constants.data"; }
	}
	
	protected override string fileName
	{
		get { return "ConstantsData"; }
	}

	public void GetData (DataCallback callback)
	{
		this.callback = callback;
		
		var serverRequestForm = new WWWRequestForm ();
		AddDefaultToServerRequestForm (serverRequestForm);

		var serverRequest = new WWWRequest (monoBehaviour, methodServerUrl, serverRequestForm);
		monoBehaviour.StartCoroutine (RequestData (serverRequest));
	}

	public override void OnRequestDataSuccess (int code, string message, JSONObject result)
	{
		if (API_Helper.CodeIsSuccess (code))
		{
			Dictionary<string,string> constantsData = null;

			try
			{
				ValidateHasConstantsArray (result);
				constantsData = GetConstantsData (result);
			}
			catch (Exception e)
			{
				code = API_Code.ERROR_CODE;
				message = e.Message;
			}
			
			callback (code, message, constantsData);
		}
		else
		{
			callback (code, message);
		}
	}

	public override void OnRequestDataFailed (int code, string message)
	{
		callback (code, message);
	}

	public static Dictionary<string,string> GetConstantsData (JSONObject resultJson)
	{
		try
		{
			var constantsData = new Dictionary<string,string> ();

			List<JSONObject> constantsJsonList = resultJson ["constants"].GetJsonObjectList ();
			constantsJsonList.ForEach (constantJson => {
				constantsData.Add (
					constantJson ["key"].GetAsString (),
					constantJson ["value"].GetAsString ()
				);
			});

			return constantsData;
		}
		catch (Exception e)
		{	
			Debug.Log ("GetConstantData e= " + e.Message);
			throw e;
		}
	}

	public static void ValidateHasConstantsArray (JSONObject json)
	{
		string key = "constants";
		if (!json.HasField (key))
			throw new KeyNotFoundException ("Not found " + key + " key.");
		if (json [key].Count < 1)
			throw new EmptyListException (key);
	}
}
