using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gWebClass : MonoBehaviour {
	// Use this for initialization
	public GameObject holder;
	public GameObject web;
	public GameObject web2;
	public GameObject chainPrefab;
	public bool webStateCollisionBerry = false;
	public GameObject pointB;
	public Vector3 pointBAchor;
	public int drag = 2;
	public AudioSource audioShot;
	public AudioSource audioOver;
	public static int fixedCounter = 0;

	private int globalCounter = 0;
	private int maxChainCount2 = 40;
	private int maxChainCount = 20;
	private int chainCount = 0;
	private float  chainLength = 0.08F;
	private GameObject[] chain;
	private float maxDiffC;
	private Vector2 diff;
	private string webState = "";
	private float diffX;
	private float diffY;
	private float chainPositionZ = 1F;
	private HingeJoint2D jointWeb;
	private int normalChainCount = 10;
	private GameObject berry;

	void Start () {
		berry = GameObject.Find("berry");

		chain = new GameObject[100];
		maxDiffC = chainLength * maxChainCount;
		jointWeb = web.GetComponent<HingeJoint2D> ();
		if (pointB != null) {
			if (pointB.name == "berry") webStateCollisionBerry = true;
			if (pointB.name == "spider") gSpiderClass.websSpider.Add(gameObject.GetInstanceID());
			diff = pointB.transform.position + pointBAchor - web.transform.position;
			float pointBDiffC = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
			diffX = maxDiffC / pointBDiffC * diff.x / maxChainCount;
			diffY = maxDiffC / pointBDiffC * diff.y / maxChainCount;
			webState = "creatingWeb";
			while (webState == "creatingWeb") {
				createWeb(pointB.transform.position, pointBAchor, true);

				int i = chainCount;
				//chain[i].rigidbody2D.fixedAngle = true;
				if (pointB.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX, diffY, 0)) ||
					pointB.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position + new Vector3(diffX/2, diffY/2, 0))) {

					chainCount = i;
					HingeJoint2D jointChain = chain[1].GetComponent<HingeJoint2D> ();
					jointChain.connectedBody = pointB.GetComponent<Rigidbody2D>();
					jointChain.connectedAnchor = pointBAchor * 1024;
					jointChain.enabled = true;	
					jointChain.useLimits = false;
					for (int y = 1; y <= chainCount; y ++){
						chain[y].GetComponent<Rigidbody2D>().drag = drag;
						chain[y].GetComponent<BoxCollider2D>().enabled = true;
						//chain[y].rigidbody2D.angularDrag = 2;
						//chain[y].rigidbody2D.centerOfMass = new Vector2(0, -0.3F);
					}

					//i = maxChainCount / 2;
					webState = "enableWeb";

				}
			}
		}
		fixedCounter = 0;

	}

	// Update is called once per frame
	void FixedUpdate () {
		if (webState == "creatingWeb" || webState == "collisionOrby") {
			for (int j = 0; j < 8; j++) {
				if (chainCount < maxChainCount && (webState == "creatingWeb" || webState == "collisionOrby")) {
					createWeb(berry.transform.position, new Vector3(0, 0, 0), false);

				}
			}
		}
		if (webState == "createWeb") {
			HingeJoint2D jointWeb = web.GetComponent<HingeJoint2D> ();
			for (int j = 0; j < 2; j++) {
				if (normalChainCount < chainCount) {
					Destroy(chain[chainCount], 0);

					for (int y = 1; y <= chainCount; y++) {
						chain[y].transform.position =  new Vector3(web.transform.position.x + diffX * (chainCount - y), web.transform.position.y + diffY * (chainCount - y), chainPositionZ);
					}
					//orby.transform.position =  new Vector3(web.transform.position.x + diffX * chainCount, web.transform.position.y + diffY * chainCount, chainPositionZ);
					jointWeb.connectedBody = chain[chainCount - 1].GetComponent<Rigidbody2D>();
					chainCount--;
				} else {
					for (int y = 1; y <= normalChainCount; y++) {
						chain[y].GetComponent<BoxCollider2D>().enabled = true;
					}
					webState = "enableWeb";
					webStateCollisionBerry = true;


					j = 2;
				}
			}

		}

		if (webState == "noCollisions" || webState == "destroyingWeb" || webState == "collisionBlock") {
			if (gBerryClass.berryState != "start finish" && webState == "destroyingWeb") gBerryClass.berryState = "";
			for (int j = 0; j < 2; j++) {
				if (chainCount > 0) {
					Destroy(chain[globalCounter], 0);
					chainCount--;
					globalCounter ++;

				} else {
					webState = "";
				}
			}
			//holder.GetComponent<LineRenderer>().SetVertexCount (chainCount);
			//for(int i = 0; i < chainCount; i++)
			//	holder.GetComponent<LineRenderer>().SetPosition (i, chain[i + globalCounter].transform.localPosition);


		}

		//if (webState == "enableWeb" || webState == "creatingWeb") {
		//holder.GetComponent<LineRenderer>().SetVertexCount (chainCount);
		//for(int i = 1; i <= chainCount; i++) {
		//	holder.GetComponent<LineRenderer>().SetPosition (i - 1, chain[i].transform.localPosition);
		//}
		//}

		/*
		if (webState == "collisionOrby1") {
			HingeJoint2D jointChain = chain[1].GetComponent<HingeJoint2D> ();

			jointChain.useLimits = false;
			//jointChain.limits = new JointAngleLimits2D { min = 180, max = 0};
			jointChain.connectedBody = orby.rigidbody2D;
			jointChain.connectedAnchor = orby.transform.position - chain[1].transform.position;
			//jointChain.connectedAnchor = new Vector2(0, 0);
			jointChain.enabled = true;	
			webState = "afterCollisionOrby";
		}
		*/

		fixedCounter++;
	}

	void createWeb (Vector3 pointBPosition, Vector3 pointBAchor, bool start) {
		chainCount ++; 
		int i = chainCount;
		Vector3 pos = new Vector3(transform.position.x, transform.position.y, 1);
		chain[i] = Instantiate(chainPrefab, pos, Quaternion.identity) as GameObject;
		//chain[i].GetComponent<SpriteRenderer> ().sortingOrder = i;
		Vector3 relative = transform.InverseTransformPoint(pointBPosition + pointBAchor);
		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
		chain[i].transform.Rotate(0, 0, 180 - angle);
		chain[i].name = "chain " + i;
		chain[i].transform.parent = holder.transform;
		chain[i].transform.localScale = new Vector3(1, 1, 1);
		//chain[i].rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		if (i == 1) {
			jointWeb.connectedBody = chain[i].GetComponent<Rigidbody2D>();
			jointWeb.enabled = true;

			//chainClass script = chain[i].GetComponent <chainClass>();
		} else {
			if (berry.GetComponent<Collider2D>().OverlapPoint(chain[1].transform.position) && webState == "creatingWeb") {
				HingeJoint2D jointChain = chain[1].GetComponent<HingeJoint2D> ();
				jointChain.connectedAnchor = new Vector2(0, 0);
				jointChain.connectedBody = berry.GetComponent<Rigidbody2D>();
				jointChain.enabled = true;	
				webState = "collisionOrby";			
			}
			if (webState == "creatingWeb") {
				for (int y = 1; y <= i; y++) {
					chain[y].transform.position = new Vector3(web.transform.position.x + diffX * (i - y), web.transform.position.y + diffY * (i - y), chainPositionZ);
				}
			}
			HingeJoint2D joint = chain[i].GetComponent<HingeJoint2D> ();
			joint.connectedBody = chain[i - 1].GetComponent<Rigidbody2D>();
			joint.enabled = true;
			jointWeb.connectedBody = chain[i].GetComponent<Rigidbody2D>();
			if (webState == "collisionOrby" && i >= normalChainCount) {
				webState = "createWeb";
				audioShot.Play ();
			} 
		}
		if ((chainCount == maxChainCount2 && start) || (chainCount == maxChainCount && !start)) {
			webState = "noCollisions";
			globalCounter = 1;
			audioOver.Play ();
		}

	}

	void OnClick () {
		//если используется подсказка и объект не подходит, то не нажимается
		bool flagHintUse = true;
		if (gHintClass.hintState == "pause")
			if (gHintClass.actions [gHintClass.counter].id != transform.position)
				flagHintUse = false;
		if (gHintClass.hintState == "start") flagHintUse = false;
		if (gBerryClass.berryState == "start finish")  flagHintUse = true; //в конце отрезать паутинки
		//

		if (flagHintUse) {
			gRecHintClass.recHint (transform);
			gHintClass.checkHint (gameObject);

			GetComponent<Animator> ().Play ("web");
			if (webState == "") {

				//tutorial
				if (SceneManager.GetActiveScene ().name == "level1" && gHandClass.handState == "text1") 
					GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (2, 0.5F);

				staticClass.useWeb++;

				StartCoroutine (Coroutine ());

				diff = berry.transform.position - web.transform.position;
				float orbyDiffC = Mathf.Sqrt (diff.x * diff.x + diff.y * diff.y);
				diffX = maxDiffC / orbyDiffC * diff.x / maxChainCount;
				diffY = maxDiffC / orbyDiffC * diff.y / maxChainCount;
				webState = "creatingWeb";
				//creating web instant
				//for (int i = 0; i < maxChainCount2; )


			}
			if (webState == "enableWeb") {
				//tutorial
				if (SceneManager.GetActiveScene ().name == "level1") 
					GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0);

				webStateCollisionBerry = false;
				if (gSpiderClass.websSpider.Count != 0)
				if (gSpiderClass.websSpider.Contains (gameObject.GetInstanceID ()))
					gSpiderClass.websSpider.Remove (gameObject.GetInstanceID ());

				GetComponent<AudioSource> ().Play ();
				staticClass.useWeb++;
				webState = "destroyingWeb";
				globalCounter = 1;
			}
		}

	}


	public IEnumerator Coroutine(){
		//if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) GooglePlayManager.instance.IncrementAchievement("achievement_use_web_50_times", 1);
		yield return new WaitForSeconds(0.1F);
	}
}
