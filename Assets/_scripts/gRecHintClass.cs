using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gRecHintClass : MonoBehaviour {

	static public float recHintState = 0;
	static public string rec = "";
	static public int counter = 0;
	// Use this for initialization
	void Start ()
	{
        Debug.Log("start gRecHintClass");
        recHintState = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Update is called once per frame
	void OnPress (bool flag) {

	}

	public static void recHint(Transform tr) {
		if (recHintState != -1) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (tr.name == "web" || tr.name == "cloud" || tr.name == "yeti body") mousePos = new Vector3(0, 0, 0); 

			rec = rec + 		
				"\nactions[" + counter + "].id = new Vector3("+tr.position.x+"F, "+tr.position.y+"F, "+tr.position.z+"F); //" + tr.name +
				//"\nactions[" + counter + "].time = "+(Time.unscaledTime - recHintState)+"F;" +
				"\nactions[" + counter + "].frame = "+(gBerryClass.fixedCounter - recHintState)+";" +
				"\nactions[" + counter + "].mouse = new Vector3("+mousePos.x+"F, "+mousePos.y+"F, "+mousePos.z+"F);";
				//"\nactions[" + counter + "].mouse = new Vector3("+Input.mousePosition.x+", "+Input.mousePosition.y+", "+Input.mousePosition.z+");";
			//recHintState += Time.unscaledTime - recHintState;
			recHintState += gBerryClass.fixedCounter - recHintState;
			counter++;
			Debug.Log (rec);
		}
	}
}
