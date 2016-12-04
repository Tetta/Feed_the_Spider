using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gGrootClass : MonoBehaviour {

	public AudioSource audioClick;
	public AudioSource audioOver;
	public AudioSource audioShot;

	public GameObject chainPrefab;
	public GameObject chainFirst;

	private GameObject berry;
	private GameObject spider;
	private int globalCounter = 0;
	private string grootState = "";

	private float chainLength = 80; // -40 +40
    private int maxChainCount = 10;
	private int chainCount = 0;
	private GameObject[] chain;
	private float angle;
	private HingeJoint2D jointGroot;
	private float diffX;
	private float diffY;
	private GameObject[] terrains;
	public struct terrainGrootChain {
		public GameObject terrain;
		public GameObject chain;
	}
	//private float timeStartCreating = 0;
	public static  List<terrainGrootChain>  terrainGrootChains = new List<terrainGrootChain>();

	// Use this for initialization
	void Start () {
		chain = new GameObject[11];
		jointGroot = GetComponent<HingeJoint2D> ();
		terrains = GameObject.FindGameObjectsWithTag("terrain");
		spider = GameObject.Find("/root/spider");
		berry = GameObject.Find("/root/berry");
		GetComponent<Animator> ().Play ("groot idle", 0, Random.value * 5);

	}



	void OnPress(bool isPressed) {
		//если используется подсказка и объект не подходит, то не нажимается
		bool flagHintUse = true;
		if (gHintClass.hintState == "pause")
		if (gHintClass.actions [gHintClass.counter].id != transform.position)
			flagHintUse = false;
		if (gHintClass.hintState == "start") flagHintUse = false;
		//
		if (flagHintUse) {

			if (isPressed) {
				audioClick.Play ();
				if (grootState == "") {
					//tutorial
					if (ctrProgressClass.progress["currentLevel"] == 76 && gHandClass.handState == "text1") 
						GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (3, 0);

					staticClass.useGroot = true;
					grootState = "drag";
					gHintClass.checkHint (gameObject);
				}		
				if (grootState == "enable") {
					//tutorial
					if (ctrProgressClass.progress["currentLevel"] == 76 && gHandClass.handState == "text2") 
						GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0);

					staticClass.useGroot = true;
					grootState = "destroying";
					gHintClass.checkHint (gameObject);
					gRecHintClass.recHint (transform);
					globalCounter = 1;
					chainFirst.SetActive (false);
					destroyingRope ();

				}
			} else {
				if (grootState == "drag") {


					gRecHintClass.recHint (transform);

					Vector3 mousePosition = gHintClass.checkHint (gameObject, true);
					Vector3 diff = mousePosition - transform.position;
					float pointBDiffC = Mathf.Sqrt (diff.x * diff.x + diff.y * diff.y);
					diffX = chainLength / pointBDiffC * diff.x;
					diffY = chainLength / pointBDiffC * diff.y;

					grootState = "creating";
					GetComponent<Animator> ().Play ("groot");
					StartCoroutine (creatingRope ());

				}
			}
		}
	}
	
	void OnDrag() {
		if (grootState == "drag") {
			Vector3 mousePosition = gHintClass.checkHint(gameObject, true);
			Vector3 relative = transform.InverseTransformPoint(mousePosition);
			angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			transform.Rotate(0, 0, 270 - angle);
			//Debug.
			//transform.rotation = Quaternion.Euler(0, 0, 270 - angle);
		}
	}

	void createRope () {
	}

	private IEnumerator creatingRope(){
		yield return StartCoroutine(staticClass.waitForRealTime(0.3F));
		for (int j = 0; j < maxChainCount; j++) {
			
			chainCount ++; 
			int i = chainCount;
			chain[i] = Instantiate(chainPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			chain[i].SetActive(true);
			chain[i].transform.Rotate(0, 0, transform.rotation.eulerAngles.z - 180);

			chain[i].name = "chain " + i;
			chain[i].transform.parent = transform;
			chain[i].transform.localScale = new Vector3(1, 1, 1);
			chain[i].transform.localPosition = new Vector3(-chainLength - chainLength * 0.58F, 0, 0) + (Vector3)GetComponent<HingeJoint2D>().anchor;
			//wait
			yield return StartCoroutine(staticClass.waitForRealTime(0.001F));




			for (int y = 1; y <= i; y++) {
				chain[y].transform.localPosition = new Vector3(-chainLength * (i - y) - chainLength * 0.58F, 0, 0) + (Vector3)GetComponent<HingeJoint2D>().anchor;

			}
			if (i == 1) {
				jointGroot.connectedBody = chain[i].GetComponent<Rigidbody2D>();
				jointGroot.enabled = true;
			} else {
				HingeJoint2D joint = chain[i].GetComponent<HingeJoint2D> ();
				joint.connectedBody = chain[i - 1].GetComponent<Rigidbody2D>();
				joint.enabled = true;
				jointGroot.connectedBody = chain[i].GetComponent<Rigidbody2D>();

			}
			foreach (GameObject terrain in terrains) {
				//if (terrain.GetComponent<Collider2D>().OverlapPoint(transform.position)) continue;

				//first point
				if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX * 0.75F, diffY * 0.75F, 0)/512)) {
					chainFirst.SetActive(true);
					chainFirst.transform.localPosition = chain[1].transform.localPosition + new Vector3( - chainLength * 0.58F, 0, 0);
					chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
					grootState = "enable";
					terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chainFirst});
					audioShot.Play ();
					continue;
				} 
				//second point
				if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX * 0.5F, diffY * 0.5F, 0) / 512)) {
					chainFirst.SetActive(true);
					chainFirst.transform.localPosition = chain[1].transform.localPosition + new Vector3(-chainLength * 0.58F, 0, 0);
					chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
					grootState = "enable";
					terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chainFirst});
					audioShot.Play ();
					continue;
				} 

				//3 point
				if (terrain.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position - new Vector3(diffX * 0.25F, diffY * 0.25F, 0) / 512)) {
					chainFirst.SetActive(true);
					chainFirst.transform.localPosition = chain[1].transform.localPosition + new Vector3(-chainLength * 0.58F, 0, 0);
					chainFirst.GetComponent<HingeJoint2D> ().connectedBody = chain[1].GetComponent<Rigidbody2D>();
					grootState = "enable";
					terrainGrootChains.Add(new terrainGrootChain() {terrain = terrain, chain = chainFirst});
					audioShot.Play ();
					continue;
				} 
			}
			if (chainCount == maxChainCount || spider.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position) || berry.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position)) {
				grootState = "noCollisions";
				globalCounter = 1;
				destroyingRope ();
				audioOver.Play ();
				//tutorial
				if (ctrProgressClass.progress["currentLevel"] == 76 && gHandClass.handState == "text1") 
					GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (1, 0);
			}
			//tutorial
			if (ctrProgressClass.progress["currentLevel"] == 76 && gHandClass.handState == "text1" && grootState == "enable") 
				GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (2, 1.5F);
			




			if (grootState != "creating") break;

		
		}

	}

	void destroyingRope(){
		for (int j = 0; j < maxChainCount + 1; j++) {

			if (chainCount > 0) {
				Destroy(chain[globalCounter], 0);
				chainCount--;
				globalCounter ++;

			} else {
				transform.localRotation = Quaternion.Euler(0, 0, 0);
				GetComponent<Animator>().Play("groot idle");

				grootState = "";
				break;
			}
		}


		chainFirst.SetActive(false);

	}



}
