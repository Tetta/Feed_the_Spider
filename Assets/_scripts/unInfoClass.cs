using UnityEngine;
using System.Collections;

public class unInfoClass : MonoBehaviour {
	public static string info;
	public GameObject scrollWindow;
	public UILabel label;

	// Use this for initialization
	void Start () {
		//label.text = info;
		//Application.logMessageReceived += handleLog;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnClick () {
		if (scrollWindow.activeSelf) scrollWindow.SetActive(false);
		else  scrollWindow.SetActive(true);
	}

	void handleLog(string logString, string stackTrace, LogType type) {
		//info = info + "\n" + type + ": " + logString + "\n" + stackTrace;
		info = info + "\n" + type + ": " + logString ;
		label.text = info;
	}

}
