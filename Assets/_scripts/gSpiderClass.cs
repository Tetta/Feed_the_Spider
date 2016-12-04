using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gSpiderClass : MonoBehaviour {

	private GameObject[] guiStars = new GameObject[3];
	//private GameObject restart;
	private int fixedUpdateCount;
	private GameObject berry;
	//public Animator currentSkinAnimator;
	public static List<int> websSpider = new List<int>();
	private int fixedCounter;

	//private GameObject completeMenu;
	//private GameObject berry;
	// Use this for initialization
	void Start () {
		
		guiStars[0] = GameObject.Find("gui stars").transform.GetChild(0).gameObject;
		guiStars[1] = GameObject.Find("gui stars").transform.GetChild(1).gameObject;
		guiStars[2] = GameObject.Find("gui stars").transform.GetChild(2).gameObject;
		berry = GameObject.Find("berry");
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();

        /*
		//включаем текущий скин и выключаем все остальные
		for (int i = 0; i < 5; i++) {
			if (transform.GetChild(i).name == staticClass.currentSkin) {
				transform.GetChild(i).gameObject.SetActive(true);
				currentSkinAnimator = transform.GetChild(i).GetComponent<Animator>();
				//включаем текущую шапку и выключаем все остальные
				for (int j = 0; j < 4; j++) {
					if (transform.GetChild (i).GetChild (0).GetChild (j).name == staticClass.currentHat) {
						transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (true);
					} else 
						transform.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (false);
				}
			} else 
				transform.GetChild(i).gameObject.SetActive(false);
		}
		*/
        //staticClass.changeSkin (out currentSkinAnimator);
        staticClass.changeSkin ();
		staticClass.changeHat ();



	}
	void Awake () {
		websSpider.Clear ();
	}


	void FixedUpdate () {
		if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -6 || transform.position.y > 6) GameObject.Find("restart").SendMessage("OnPress", false);

		if (staticClass.scenePrev == "level menu")
		if (fixedUpdateCount == 2) {
			staticClass.currentSkinAnimator.Play ("spider hi", 0);
			staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (0).GetComponent<AudioSource> ().Play ();
		}

		if (fixedUpdateCount % 20 == 1 && gBerryClass.berryState != "start finish") {
			
			if ((berry.transform.position - transform.position).magnitude >= 0.5F) {
				if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month") ||
				    staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month legs")) {
					staticClass.currentSkinAnimator.Play ("spider idle", 1);

				}

			} else {
				if (gBerryClass.berryState != "finish")
				if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (0).IsName ("spider breath")) staticClass.currentSkinAnimator.Play ("spider idle", 0);
					
			}

			//check jump
			if (GetComponent<Rigidbody2D>().velocity.magnitude <= 0.113F && websSpider.Count == 0) {
				if (gBerryClass.berryState == "finish")
					if (transform.rotation.eulerAngles.z > 50 && transform.rotation.eulerAngles.z < 310) {
						staticClass.currentSkinAnimator.Play ("spider jump");
						StartCoroutine (coroutineJump ());
					}
				if (transform.rotation.z > 2.1F || transform.rotation.z < -2.1F) {
					staticClass.currentSkinAnimator.Play("spider jump");
					StartCoroutine(coroutineJump());
				} else {

					if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath") && 
					    !staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month")) {
						if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (0).IsName ("spider fly") ||
						    staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (0).IsName ("spider fly 2")) {

							staticClass.currentSkinAnimator.Play ("spider jump");
							StartCoroutine (coroutineJump ());	
						} else 
					
							if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad start") &&
						     !staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad end") && 
							!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month") && 
							!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month legs") 	) 
							staticClass.currentSkinAnimator.Play ("spider breath");

					}

				}
			} else {


				if (websSpider.Count != 0) staticClass.currentSkinAnimator.Play("spider fly 2");
				 else {
					staticClass.currentSkinAnimator.Play("spider fly");
				}
			}

            //если ягода висит на паутине, то скорей всего не работает из-за gBerryClass.berryState == ""
        }
        else if (fixedUpdateCount % 10 == 0 && gBerryClass.berryState == "") {
			//check mouth
			if ((berry.transform.position - transform.position).magnitude < 0.5F)
			if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (0).IsName ("spider breath")) {
				if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month legs") &&
					!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month") ) 
					staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (1).GetComponent<AudioSource> ().Play ();
				staticClass.currentSkinAnimator.Play ("spider open month legs");

			}
			else if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider fly") ||
			         staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider fly 2")) {
				staticClass.currentSkinAnimator.Play("spider open month"); 
				if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month legs") &&
					!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider open month") )
					staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (1).GetComponent<AudioSource> ().Play ();

			} 
				
		}
		fixedUpdateCount ++;

	}
		
	void OnTriggerEnter2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "star") {
			StartCoroutine(collisionObject.gameObject.GetComponent<gStarClass>().destroyStar());
			guiStars[gBerryClass.starsCounter].SetActive(true);
			gBerryClass.starsCounter ++;
		}
		
	}
	/*
	void OnClick () {
		staticClass.currentSkinAnimator.Play("spider blink", -1);
	}
	*/
	public IEnumerator coroutineJump(){
		if (gYetiClass.yetiState == "") {
			staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (4).GetComponent<AudioSource> ().Play ();
			yield return StartCoroutine (staticClass.waitForRealTime (0.1F));
			transform.rotation = new Quaternion (0, 0, 0, 1);
		}
		yield return StartCoroutine (staticClass.waitForRealTime (0));

	}

	public static IEnumerator coroutineCry(Animator spiderAnimator){
		spiderAnimator.Play("spider sad start", 1);
		if (spiderAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath")) spiderAnimator.Play("spider idle", 0);
		yield return new WaitForSeconds(2F);
		GameObject.Find("restart").SendMessage("OnPress", false);
	}

	
}
