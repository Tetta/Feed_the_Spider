using UnityEngine;
using System.Collections;

public class lsTerrainPathClass : MonoBehaviour {
	public Transform cameraTransform;
	public Transform cameraColorLeft;
	public Transform cameraColorRight;
	public GameObject backForest;
	public GameObject backRock1;
	public GameObject backRock2;
	public GameObject backIce1;
	public GameObject backIce2;
	public GameObject backDesert;

	private Vector3 screenPoint;
	private Vector3 mouseDownPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnPress() {
		screenPoint = cameraTransform.position;
		mouseDownPosition = 
			Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
	}
	void OnDrag() {
		Vector3 offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) - mouseDownPosition;
		Vector3 newPosition = screenPoint - offset;
		if (newPosition.x > 0) {
			cameraTransform.position = new Vector3 (newPosition.x, 0, 0);
			cameraColorLeft.position = new Vector3 (newPosition.x, 0, 0);
			//cameraColorRight.position = new Vector3 (newPosition.x, 0, 0);
		} else {
			cameraTransform.position = new Vector3 (0, 0, 0);
			cameraColorLeft.position = new Vector3 (0, 0, 0);
			//cameraColorRight.position = new Vector3 (0, 0, 0);
		}
		if (newPosition.x + 600 >= backForest.transform.position.x && newPosition.x <= backRock1.transform.position.x + 600) {
			// процент прозрачности
			//backForest.GetComponent<UIPanel>().alpha = 1.1F - ((cameraTransform.localPosition.x - backForest.transform.localPosition.x) / 1536);
			//backRock1.GetComponent<UIPanel>().alpha = 0.1F + (cameraTransform.localPosition.x - backForest.transform.localPosition.x) / 1536;
			// (граница + полэкрана - камера) / экран
			cameraColorRight.GetComponent<Camera>().rect = new Rect (
				(backForest.transform.localPosition.x + 768 + 2048 * Screen.width / Screen.height / 2 - cameraTransform.localPosition.x) / 
				(2048 * Screen.width / Screen.height), 0, 1, 1);
			backForest.GetComponent<UIPanel>().alpha = cameraColorRight.GetComponent<Camera>().rect.x * 1.34F;
			backRock1.GetComponent<UIPanel>().alpha = (1 - cameraColorRight.GetComponent<Camera>().rect.x) * 1.34F;
			cameraColorRight.localPosition = new Vector3 (cameraTransform.localPosition.x + 2048 * Screen.width / Screen.height * cameraColorRight.GetComponent<Camera>().rect.x / 2, 0, 0);
		}
	}

}
