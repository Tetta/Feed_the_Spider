using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class lsEditor : MonoBehaviour {

    public Transform root;
    public Transform scroll2;
    public Transform scroll3;
    public Transform camera1;
    public Transform camera2;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame

    void OnRenderObject() {
        //void Update () {
        if (Camera.current.transform.position.x != 0) {
             //root.position = new Vector3(-Camera.current.transform.position.x * 1F, 0, 0);
            scroll2.position = new Vector3(Camera.current.transform.position.x * 0.33F, 0, 0);
            scroll3.position = new Vector3(Camera.current.transform.position.x * 0.5F, 0, 0);
            camera1.position = new Vector3(Camera.current.transform.position.x, 0, 0);
            camera2.position = new Vector3(Camera.current.transform.position.x, 0, 0);
            // scroll2.position = new Vector3(root.position.x * 0.67F, 0, 0);
            //scroll3.position = new Vector3(root.position.x * 0.5F, 0, 0);
            //Camera..current.transform.position = new Vector3(0, Camera.current.transform.position.y, 0);
        }
    }
}
