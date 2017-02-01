using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class lsSpiderClass : MonoBehaviour {

	public GameObject levelMenu;
	public UILabel titleNumberLevel;
	public GameObject stars;
	public GameObject time;
	public GameObject web;
	public GameObject sluggish;
	public GameObject destroyer;
	public GameObject yeti;
	public GameObject groot;
	public GameObject cameraUI;
	public GameObject gem1Inactive;
	public GameObject gem2Inactive;

	private string spiderState = "";

	private Vector3 velCamera;
	private Vector3 startPosSpider;
	private Vector3 posIsland;
	private float timerJump;
	private float timeJumpConst = 0.5F;
	private Vector3 tan1;
	private Vector3 tan2;


	// Use this for initialization
	void Start () {
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
		transform.position = GameObject.Find("level " + ctrProgressClass.progress["currentLevel"] + "/gems").transform.position + new Vector3(-110, 10, 0) / 512;
        //if (transform.position.x > 0) cameraUI.transform.position = new Vector3(transform.position.x, cameraUI.transform.position.y, cameraUI.transform.position.z);
        if (transform.position.x > 0) {
            //transform.parent.position = new Vector3(-transform.position.x, 0, 0);
            transform.parent.GetComponent<UIScrollView>().MoveAbsolute(new Vector3(-transform.position.x, 0, 0));
        }
        staticClass.changeSkin ();
		staticClass.changeHat ();

	}
	
	// Update is called once per frame
	void Update () {


		if (spiderState == "jump up") {
			if (Time.time - timerJump > 0.33F) {
				spiderState = "jump";
				staticClass.currentSkinAnimator.speed = 0;
			}

		}
		if (spiderState == "jump") {

			//двигаем камеру
			//cameraUI.transform.position += velCamera * Time.deltaTime;
			//t
			float t = (Time.time - timerJump - 0.33F) / timeJumpConst;
			//двигаем паука

			//transform.position += velSpider * Time.deltaTime;
			transform.localPosition = сalculateBezierPoint(t, startPosSpider, tan1, tan2, posIsland);
			//заканчиваем прыжок
			if (t >= 1) {
				if (velCamera != Vector3.zero) cameraUI.transform.position = new Vector3 (-posIsland.x / 512, 0, 0);
                transform.localPosition = posIsland;
                spiderState = "";
				staticClass.currentSkinAnimator.speed = 1;
				StartCoroutine(selectLevelMenuCorourine());
			}
		}

	}

	public void clickLevel (Vector3 posIslandParam) {
		posIsland = posIslandParam + new Vector3(-110, 10, 0);
		startPosSpider = transform.localPosition;
		//velSpider = (posIsland - startPosSpider) / timeJumpConst;
		if (Mathf.Abs(cameraUI.transform.position.x - startPosSpider.x) > 1) {
			//velCamera = -(new Vector3(posIsland.x - startPosSpider.x, 0, 0) / timeJumpConst) / 512;
			//cameraUI.transform.position = new Vector3 (-transform.position.x, 0, 0);
		} else 
			velCamera = Vector3.zero;

		//касательные
		tan1.x = startPosSpider.x + (posIsland.x - startPosSpider.x) / 4;
		tan2.x = posIsland.x - (posIsland.x - startPosSpider.x) / 4;
		tan1.y = startPosSpider.y + Mathf.Clamp(Mathf.Abs(posIsland.y - startPosSpider.y), 0.5F * 512, 1.8F * 512);
		tan2.y = posIsland.y + Mathf.Clamp(Mathf.Abs(posIsland.y - startPosSpider.y), 0.5F * 512, 1.8F * 512);

		spiderState = "jump up";

		timerJump = Time.time;
		staticClass.currentSkinAnimator.Play("spider jump");
		staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (4).GetComponent<AudioSource> ().Play ();

	}

	public IEnumerator selectLevelMenuCorourine () {
		yield return new WaitForSeconds(0.3F);
		levelMenu.SetActive(true);
		levelMenu.GetComponent<AudioSource> ().Play ();
		levelMenu.SendMessage ("levelMenuEnable");

		//FB
		ctrFbKiiClass.setBestGamers(levelMenu.transform, ctrProgressClass.progress["currentLevel"]);
	}

	Vector3 сalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {

		float u = 1 - t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu * u;
		float ttt = tt * t;
		
		Vector3 p = uuu * p0;    //first term
		p += 3 * uu * t * p1;    //second term
		p += 3 * u * tt * p2;    //third term
		p += ttt * p3;           //fourth term
		
		return p;
	}


}
