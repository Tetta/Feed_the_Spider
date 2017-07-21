using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gSluggishClass : MonoBehaviour {

	public AudioSource audioClick;
	public AudioSource audioEat;
	public AudioSource audioShot;

	private string sluggishState = "";
	private GameObject berry;
	private float timer;
    private float angle;
	private bool helperFlag = false;

    private Transform helper;
    private Transform sluggishPos;


    // Use this for initialization
    void Start() {
        berry = GameObject.Find("berry");

        //Unity 2017
        /*
        if ((SceneManager.GetActiveScene().name == "level4" || SceneManager.GetActiveScene().name == "level6") && gHintClass.hintState == "")
        {
            helperFlag = true;
            helper = transform.GetChild(1);
            sluggishPos = transform.parent;
        }
        */
    }
    // Update is called once per frame
    void Update () {
        if (sluggishState == "collision") {
			//berry.transform.rotation = Quaternion.Euler(0, 0, 0);
			//berry.transform.position = new Vector3(transform.position.x, transform.position.y - 0.32F * transform.localScale.y, 10);
			//berry.transform.localPosition = new Vector3(0, - 165, 10);
		}
		if (sluggishState == "fly") {
			if ((transform.position - berry.transform.position ).sqrMagnitude > 0.09F) OnTriggerExit2D3();

			if (Time.time - timer > 0.5F) 
				collisionEnter2D(); 
		}

		if (sluggishState == "") {
			if ((transform.position - berry.transform.position ).sqrMagnitude < 0.09F) collisionEnter2D();

		}
        
	}
    void FixedUpdate () {
        if (sluggishState == "active") {
			dragSluggish ();

        }


    }

    void OnDrag()
    {
        if (sluggishState == "active")
        {
            dragHelper();

        }
    }

    void dragHelper()
    {        
        if (helperFlag)
        {

            Vector3 diff = transform.parent.position - transform.position;
            float pointBDiffC = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
            float maxDiffC = 180;

            float diffX = maxDiffC / pointBDiffC * diff.x;
            float diffY = maxDiffC / pointBDiffC * diff.y;
            //Unity 2017
            //Physics2D.autoSimulation = false;
            helper.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //helper.GetComponent<Rigidbody2D>().simulated = true;
            helper.GetComponent<Rigidbody2D>().AddForce(new Vector2(diffX, diffY));



            GetComponent<Rigidbody2D>().isKinematic = true;
            /*
            Vector3 mousePosition = gHintClass.checkHint(transform.parent.gameObject, true);
            Vector3 relative = transform.parent.InverseTransformPoint(mousePosition);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            */

            Vector3 mousePosition = gHintClass.checkHint(transform.parent.gameObject, true);
            Vector3 relative = transform.parent.InverseTransformPoint(mousePosition);
            float angle1 = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;

            //if (angle1 < 0) angle1 += 180;
            transform.localPosition = new Vector3(0, -156, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //transform.RotateAround(sluggishPos.position, Vector3.forward, -180 - angle1);
            transform.RotateAround(sluggishPos.position, Vector3.forward, -180 - angle1);
            helper.GetComponent<LineRenderer>().positionCount = 50;
            for (int i = 0; i < 50; i++)
            {
                //Debug.Log(helper.position);
                helper.GetComponent<LineRenderer>().SetPosition(i, helper.position + Vector3.forward);
                //Unity 2017
                //Physics2D.Simulate(0.02F);
            }
            //Physics2D.autoSimulation = false;
            //helper.GetComponent<Rigidbody2D>().simulated = false;
            helper.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            helper.localPosition = new Vector3(0, 0, 0);
        }
    }

    void dragSluggish()
    {
        Vector3 mousePosition = gHintClass.checkHint(transform.parent.gameObject, true);
        Vector3 relative = transform.parent.InverseTransformPoint(mousePosition);
        angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;

        if (!helperFlag)
        {
            if (gHintClass.hintState == "") transform.GetComponent<Rigidbody2D>().MoveRotation(180 - angle);
            else transform.RotateAround(transform.parent.position, Vector3.forward, -180 - angle);
        }
    }

    void collisionEnter2D() {
		berry.GetComponent<Rigidbody2D>().isKinematic = true;
        //berry.GetComponent<Rigidbody2D>().simulated = false;
        berry.GetComponent<Rigidbody2D>().inertia = 0;
        berry.GetComponent<Rigidbody2D>().angularVelocity = 0;
        berry.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        berry.transform.parent = transform;
		berry.transform.localRotation = Quaternion.identity;
		//berry.transform.localPosition = new Vector3(0, - 165, 10);
		berry.transform.localPosition = new Vector3(0, 0, 10);
		transform.localScale = new Vector3(0.98F, 1.02F, 1);
		//berry.GetComponent<Collider2D>().isTrigger = true;
		transform.parent.GetComponent<Animation>()["sluggish eye"].speed = 1;
		transform.parent.GetComponent<Animation>().Blend("sluggish eye");
		sluggishState = "collision";
		audioEat.Play ();

        //copy berry
        if (helperFlag)
        {
            /*
            GameObject berryCopy = Instantiate(berry, transform);
            //berryCopy.transform.DestroyChildren();
            berryCopy.GetComponent<gBerryClass>().enabled = false;
            berryCopy.GetComponent<gBerryClass>().enabled = false;
            */

        }

    }

	//void OnCollisionEnter2D(Collision2D collisionObject) {

	void OnTriggerEnter2D3(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "berry" && sluggishState == "") {
			collisionEnter2D ();
		}
	}

	//void OnCollisionExit2D(Collision2D collisionObject) {
	//void OnTriggerExit2D3(Collider2D collisionObject) {
	void OnTriggerExit2D3() {

		//if (collisionObject.gameObject.name == "berry" && sluggishState == "fly"){// && counterTriggerExit == 2) {
		berry.transform.parent = transform.parent.parent;
			berry.transform.localScale = new Vector3(1, 1, 1);
			berry.GetComponent<Collider2D>().isTrigger = false;
			sluggishState = "";
			//counterTriggerExit = 0;
		//}
	}

	void OnPress(bool flag) {

		//если используется подсказка и объект не подходит, то не нажимается
		bool flagHintUse = true;
		if (gHintClass.hintState == "pause")
		if (gHintClass.actions [gHintClass.counter].id != transform.parent.position)
				flagHintUse = false;
		if (gHintClass.hintState == "start") flagHintUse = false;
		//

		if (sluggishState == "collision" && flag && flagHintUse) {
			//tutorial
			if (ctrProgressClass.progress["currentLevel"] == 4 && gHandClass.handState == "text1") 
				GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0);
            //tutorial
            if (SceneManager.GetActiveScene().name == "level7" && gHandClass.handState == "text2")
                GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand(-1, 0F);

            GetComponent<Rigidbody2D> ().drag = 20;
			transform.localScale = new Vector3 (0.96F, 1.04F, 1);

			sluggishState = "active";
			transform.parent.GetComponent<Animation> ().Stop ();

			gHintClass.checkHint(transform.parent.gameObject);
			audioClick.Play ();

		}

		if (sluggishState == "active" && !flag && flagHintUse) {

			gRecHintClass.recHint(transform.parent);
			//Vector3 mousePosition = gHintClass.checkHint(transform.parent.gameObject, true);
			gHintClass.checkHint(transform.parent.gameObject, true);


				
			staticClass.useSluggish = true;
			//if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED)
			//	GooglePlayManager.instance.UnlockAchievement ("achievement_use_sluggish");
			GetComponent<Rigidbody2D> ().drag = 2;

			berry.GetComponent<Rigidbody2D> ().isKinematic = false;
            //berry.GetComponent<Rigidbody2D>().simulated = true;
            
            //было: вектор между mousePos и parentPos
            //Vector3 diff = transform.parent.position - mousePosition;

            //стало: вектор между pos и parentPos
            Vector3 diff = transform.parent.position - transform.position;
			float pointBDiffC = Mathf.Sqrt (diff.x * diff.x + diff.y * diff.y);
			float maxDiffC = 180;

			GameObject[] webs = GameObject.FindGameObjectsWithTag ("web");
			foreach (var web in webs) {
				if (web.GetComponent<gWebClass> ().webStateCollisionBerry)
					maxDiffC = 320;
			}

			float diffX = maxDiffC / pointBDiffC * diff.x;
			float diffY = maxDiffC / pointBDiffC * diff.y;


			berry.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (diffX, diffY));


			transform.parent.GetComponent<Animation> ().Play ("sluggish press false");
			transform.parent.GetComponent<Animation> () ["sluggish eye"].speed = -1;
			transform.parent.GetComponent<Animation> ().Blend ("sluggish eye");
			transform.parent.GetComponent<Animation> ().CrossFadeQueued ("sluggish idle");

			timer = Time.time;
			sluggishState = "fly";
			audioShot.Play ();

		    if (helperFlag)
		    {
                helper.GetComponent<LineRenderer>().positionCount = 0;
                //Unity 2017
                //Physics2D.autoSimulation = true;
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
		}
	}

}
