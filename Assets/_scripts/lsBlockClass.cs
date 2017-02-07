using UnityEngine;
using System.Collections;

public class lsBlockClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//temp
		staticClass.initLevels();
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
		//temp
		int	block = int.Parse(name.Substring(6));
        /*
        if (staticClass.levelBlocks[block] > ctrProgressClass.progress["medals"]) {
			GetComponent<UISprite>().color = new Color32(200, 100, 100, 255);
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(0).gameObject.GetComponent<UILabel>().text = staticClass.levelBlocks[block].ToString();
			transform.GetChild(1).gameObject.SetActive(true);

		}
        */
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
