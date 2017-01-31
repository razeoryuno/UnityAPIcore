using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RSG;
using UnityEngine;

public class ConstantsAPI : MonoSingleton<ConstantsAPI>, IAPI_DataCallback
{
	public IPromise DataCallback ()
	{
		var promise = new Promise ((resolve, reject) => {
			string methodName = new ConstantsMethodData ().methodName;
			try {
				Data ((code, message, ConstantsDict) => {
					if (API_Code.CodeIsSuccess (code)) {
						try {
							AssignServerConstantToClient (ConstantsDict);
							resolve ();
						} catch (Exception e) {
							//TODO temporary disable throw exception
							reject (new DataMethodException (methodName, API_Code.ERROR_CODE, e.Message));
						}
					} else {
						reject (new DataMethodException (methodName, code, message));
					}
				});
			} catch (Exception e) {
				reject (new DataMethodException (methodName, API_Code.ERROR_CODE, e.Message));
			}
		});
		
		return promise;
	}

	public void Data (ConstantsMethodData.DataCallback callback)
	{
		var constantsMethodData = new ConstantsMethodData ();
		constantsMethodData.monoBehaviour = this;
		constantsMethodData.GetData (callback);
	}

	private void AssignServerConstantToClient (Dictionary<string, string> ConstantsDict)
	{
		foreach (string key in ConstantsDict.Keys) {
			string variableName = key.ToUpper ();
			string value = ConstantsDict [key];
			FieldInfo field = typeof(GameConstants).GetField (variableName);
			if (field.IsNotNull ()) {
				switch (field.FieldType.Name) {
				case "Int32":
					field.SetValue (typeof(GameConstants), int.Parse (value));

					break;
				case "String":
					field.SetValue (typeof(GameConstants), value);
					break;
				default:
					throw new ArgumentOutOfRangeException ("Failed to cast to Type: " + field.FieldType.Name);
				}
			} else {
				//throw new ArgumentNullException ("FieldInfo failed to get field name: " + variableName);
				Debug.LogWarning ("FieldInfo failed to set field name :" + variableName);
			}
		}
	}
}