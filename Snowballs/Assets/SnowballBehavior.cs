using UnityEngine;
using System.Collections;

public class SnowballBehavior : MonoBehaviour {

    public float timeInSecondsToLive =5f;
    public GameObject rightPalm;
    public GameObject leftPalm;
    private float startTime = 0.0f;
    private bool started = false;
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(!started)
        {
            startTime = Time.time;
            started = true;
            return;
        }
        



        if(Time.time - startTime >= (timeInSecondsToLive))
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("BOOM!");
    }
}
