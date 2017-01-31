using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	private static object _lock = new object();

	public static T Instance
	{
		get
		{
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = (T) FindObjectOfType(typeof(T));
					
					if ( FindObjectsOfType(typeof(T)).Length > 1 )
					{
						Debug.LogError("[MonoSingleton] Something went really wrong " +
						               " - there should never be more than 1 singleton!" +
						               " Reopenning the scene might fix it.");
						return _instance;
					}
					
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
                        singleton.name = "(MonoSingleton) "+ typeof(T).ToString();
						
						DontDestroyOnLoad(singleton);
						
//						Debug.Log("[MonoSingleton] An instance of " + typeof(T) + 
//						          " is needed in the scene, so '" + singleton +
//						          "' was created with DontDestroyOnLoad.");
					} else {
//						Debug.Log("[MonoSingleton] Using instance already created: " +
//						          _instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}
	}

	protected virtual void Awake()
	{
		/* this is just so that DontDestroyOnLoad gets called on the gameObject that's gonna this script upon waking up 
		 * - Calling Ping means accessing the singleton instance, doing so calls DontDestroyOnLoad at the end.
		 */
		Ping();
	}

	public void Ping()
	{
//		Debug.Log("[MonoSingleton]: `" + this + "` is alive!");
	}
}