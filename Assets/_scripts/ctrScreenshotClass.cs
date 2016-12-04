using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ctrScreenshotClass : MonoBehaviour
{
	public Transform labelShare;
	public Transform buttonPlay0;
	public Transform buttonPlay1;

	private Texture2D[] screenshots = new Texture2D[5];
	private int counterScreenshots = 0;
	private int currentIndex = 0;
	private int h = 0;
	private int w = 0;
	private int frameCounter = 0;
	float scaleScreen = 1;

	private IEnumerator changeSprites()
    {
		while (true) {
			transform.GetChild (0).GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (screenshots[currentIndex], new Rect(0, 0, screenshots[currentIndex].width, screenshots[currentIndex].height), new Vector2(0.5F, 0.5F), 1);
			currentIndex ++;
			if (currentIndex >= counterScreenshots)
				currentIndex = 0;
			yield return StartCoroutine(staticClass.waitForRealTime(1F));
		}
    }

	public void takeScreenshot() {
		if (counterScreenshots < 5) {
			//первое определение
			if (counterScreenshots == 0) {
				h = Mathf.RoundToInt (Screen.height * 0.125F);
				w = Mathf.RoundToInt (h * Screen.width / Screen.height);
				scaleScreen = 2 * 2048.0F / Screen.height;
			}
			RenderTexture rt = new RenderTexture (w, h, 0);
			Camera.main.targetTexture = rt;
			screenshots [counterScreenshots] = new Texture2D (w, h, TextureFormat.RGB24, false);
			Camera.main.Render ();
			RenderTexture.active = rt;
			screenshots [counterScreenshots].ReadPixels (new Rect (0, 0, w, h), 0, 0);
			Camera.main.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			//Destroy (rt);
			screenshots [counterScreenshots].Apply ();
			//transform.GetChild (0).GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (screenshots [counterScreenshots], new Rect (0, 0, screenshots [counterScreenshots].width, screenshots [counterScreenshots].height), new Vector2 (0.5F, 0.5F), 1);
			transform.GetChild (0).GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (screenshots [counterScreenshots], new Rect (0, 0, screenshots [counterScreenshots].width, screenshots [counterScreenshots].height), new Vector2 (0.5F, 0.5F), 1);
			transform.GetChild (0).GetChild (0).localScale = new Vector3(scaleScreen, scaleScreen, 1);
			counterScreenshots++;
		}
	}

	public void enableScreenshots() {
		//включаем скрины, collider, label share
		transform.GetChild (0).gameObject.SetActive (true);
		labelShare.gameObject.SetActive(true);
		GetComponent<BoxCollider>().enabled = true;

		//frame
		transform.GetChild (0).GetChild (1).localScale = new Vector2 (0.5F * scaleScreen*w + 26, 315);
		//143x256 самый узкий
		transform.localPosition =  transform.localPosition - new Vector3 ((0.5F * scaleScreen*w - 143) , 0, 0);
		//labelShare
		labelShare.localPosition =  labelShare.localPosition - new Vector3 ((0.5F * scaleScreen*w - 143) , 0, 0);
		//двигаем complete menu child
		transform.parent.GetChild(0).GetChild(0).localPosition = new Vector2(-40, 0);
		buttonPlay0.localPosition = new Vector2(401, -542);
		buttonPlay1.localPosition = new Vector2(401, -590);

		if (screenshots.Length > 0) StartCoroutine("changeSprites");
	}

	public void OnClick() {
		Everyplay.PlayLastRecording();
	}

	void Update () {
		if (frameCounter == 10 && name == "button everyplay") {
			if (Everyplay.IsRecordingSupported () && ctrProgressClass.progress["everyplay"] == 1 && !Everyplay.IsRecording()) {
				Everyplay.StartRecording ();
				takeScreenshot ();

			}
		}

		frameCounter++;

	}

	void OnEnable () {

		if (name == "button everyplay market") {
			if ((SceneManager.GetActiveScene ().name == "menu" || SceneManager.GetActiveScene ().name == "level menu") && Everyplay.IsRecording ()) {
				Everyplay.SetMetadata ("Title", "I just opened boosters!");
				Everyplay.StopRecording ();
				transform.GetChild (0).gameObject.SetActive (true);
				GetComponent<BoxCollider>().enabled = true;

			}
		}
	}

	void OnDisable () {

		if (name == "button everyplay market") {
				transform.GetChild (0).gameObject.SetActive (false);
				GetComponent<BoxCollider>().enabled = false;
		}
	}
}
