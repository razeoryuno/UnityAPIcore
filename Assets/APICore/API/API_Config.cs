using System;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public enum Host
{
	Local,
	Develop,
	Test,
	Live,
}

public class API_Config : MonoSingleton<API_Config>
{
	public static Dictionary<int, Action<int, string>> defaultActionHandleErrorCodeDict { get; set; }

	public static Dictionary<Host, string> hostDict = new Dictionary<Host, string> () {
		{ Host.Local,"" },
		{ Host.Develop,"" },
		{ Host.Test,"" },
		{ Host.Live,"" }
	};

	public bool isDebug;

	public bool isUseDummyAuthToken;

	public string dummyAuthToken;

	public Host host;

	public string GetHost ()
	{
		return hostDict [host];
	}
}