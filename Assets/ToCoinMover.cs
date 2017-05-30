using UnityEngine;

public class ToCoinMover : MonoBehaviour
{
    public GameObject Target;
    private Vector3 startPosition;
    void Awake()
    {
        startPosition = transform.position;
    }
	// Use this for initialization
	void OnEnable ()
	{
	    transform.position = startPosition;
        TweenPosition.Begin(gameObject, 0.6f, Target.transform.position, true);
    }
}
