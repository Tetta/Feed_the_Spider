using UnityEngine;
using System.Collections;

public class gChainClass : MonoBehaviour {
	public Transform q;

	// Use this for initialization w
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collisionObject){
		//if (collisionObject.gameObject.name == "orby") {
			//staticClass.instanceStaticClass.webState = "collisionOrby";

		//}
		
		
		
	}
	void OnMouseDown () {


	}
	void FixedUpdate () {
		if (q != null)GetComponent<TargetJoint2D> ().target = new Vector2 (q.position.x, q.position.y);
	}
}
