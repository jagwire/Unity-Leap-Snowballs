using UnityEngine;
using System.Collections;
using Leap;

public class FirstPersonPlayer : MonoBehaviour {
    public float speed = 1f;

    private LeapProvider provider;
    private Vector3 lastPalmNormal = Vector3.down;

	// Use this for initialization
	void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = provider.CurrentFrame;
        if(frame.Hands.Count == 2)
        {
            //don't move while we have both hands tracked
            return;
        }

        //Don't assume a hand is being tracked
        if (frame.Hands.Count == 1)
        {
            Hand hand = frame.Hands[0];
            
            //transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg*-10f * hand.Direction.Pitch*Time.deltaTime, new Vector3(1, 0, 0));
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical)*speed * Time.deltaTime);
    }
}
