using UnityEngine;
using System.Collections;

public class ctrParticleSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale < 0.01f) {
			GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
		}
        //else if ( name == "teleport" || name == "stars") GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);

    }
}
