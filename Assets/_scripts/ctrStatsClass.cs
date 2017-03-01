using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
//using Analytics;

public class ctrStatsClass : MonoBehaviour {

	[Header("Flurry Settings")]
	[SerializeField] private string _iosApiKey = string.Empty;
	[SerializeField] private string _androidApiKey = string.Empty;
	



	public static void logEvent(string category, string action, string label, int counter) {
		//if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent (category, action, label, counter);
        /*
        Flurry.Instance.LogEvent(category, new Dictionary<string, string>
			{
				{ action, "1" },
				{ label, "1" },
				{ "counter", counter.ToString() }
			});
            */
	}







    /// <summary>
    /// Create Flurry singleton instance and log single event.
    /// </summary>
/*
    private void Awake()
	{
		IAnalytics serviceFlurry = Flurry.Instance;

		AssertNotNull(serviceFlurry, "Unable to create Flurry instance!", this);
		Assert(!string.IsNullOrEmpty(_iosApiKey), "_iosApiKey is empty!", this);
		Assert(!string.IsNullOrEmpty(_androidApiKey), "_androidApiKey is empty!", this);

		//serviceFlurry.SetLogLevel(LogLevel.All);
		serviceFlurry.StartSession(_iosApiKey, _androidApiKey);
		serviceFlurry.LogEvent("start", new Dictionary<string, string>
			{
				#if UNITY_5
				{ "AppVersion", Application.version },
				#endif
				{ "UnityVersion", Application.unityVersion }
			});
	}
*/

	#region [Assert Methods]
    [Conditional("UNITY_EDITOR")]
	private void Assert(bool condition, string message, Object context)
	{
		if (condition)
		{
			return;
		}

		//Debug.LogError(message, context);
	}

    [Conditional("UNITY_EDITOR")]
	private void AssertNotNull(object target, string message, Object context)
	{
		Assert(target != null, message, context);
	}
	#endregion
}
