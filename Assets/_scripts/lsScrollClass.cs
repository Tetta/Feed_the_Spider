using UnityEngine;
using System.Collections;

public class lsTest : MonoBehaviour {

    public Transform root;
    public Transform scroll2;
    public Transform scroll3;
    public Transform cameraObjects;
    public Transform cameraBack;

    private Vector3 rockStartPos;

    // Use this for initialization
    void Start() {
        cameraObjects.position = new Vector3(0, 0, 0);
        cameraBack.position = new Vector3(0, 0, 0);

    }

        // Update is called once per frame

	//void Update () {
	void LateUpdate () {
        scroll2.position = root.position * 0.67F;
        scroll3.position = root.position * 0.5F;

    }





}
