using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Leap;
public class HandsBehavior : MonoBehaviour {

    public GameObject snowballPrefab;
    public float cooldownTime = 2.0f;
    public UnityEngine.UI.Image cooldownTimer;
    public GameObject startPanel;
    private LeapProvider provider;
    private bool spawningBall = false;
    private bool holdingBall = false;
    private GameObject spawn;
    private bool cooldown = false;
    private float cooldownStart = 0.0f;
    private bool started = false;

	// Use this for initialization
	void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        cooldownTimer.fillAmount = 0.0f;
        
	}

    void Update () {
        


        if(cooldown)
        {
            if(Time.time - cooldownStart > cooldownTime)
            {
                //cooldown over
                cooldown = false;
                cooldownTimer.fillAmount = 0.0f;
            } else
            {
                //total time - elapsed time since start of timer, all over the total time
                cooldownTimer.fillAmount = (cooldownTime - (Time.time - cooldownStart)) / cooldownTime;
                return;
            }
        }

        Frame frame = provider.CurrentFrame;
        if(frame.Hands.Count == 1)
        {
            if(isThumbsUp(frame.Hands[0])) {
                if (!started)
                {
                    startPanel.SetActive(false);
                }
            }
        }
        

        if(frame.Hands.Count == 2)
        {
            Debug.Log("Woot! 2 hands!");
           Hand  first = frame.Hands[0];
            Hand second = frame.Hands[1];
            isThumbsUp(first);
            Vector3 diff = (first.PalmPosition.ToVector3() - second.PalmPosition.ToVector3());
            float difference = diff.magnitude;
            Debug.Log("Difference in positions: " + difference);
            
            //begin drawing ball
            if(difference <= 0.08f && !spawningBall)
            {
                Debug.Log("SPAWN SOMETHING!");
                spawningBall = true;
                spawn = Instantiate(snowballPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                spawn.name = "Snowball";
                
            } //continue drawing ball
            else if(spawningBall)
            {
                spawn.transform.position = first.PalmPosition.ToVector3() - diff/2f;
                spawn.transform.localScale = Vector3.one * (difference) * 0.8f;

                //if ball is large enough, stop changing scale, parent to first palm
                if(spawn.transform.localScale.x >= 0.05f)
                {
                    spawningBall = false;
                    cooldown = true;
                    cooldownStart = Time.time;
                }
            }

            Debug.Log("First Hand Grab Strength: " + first.GrabStrength);
        }
        else
        {
            //if hand disappears while we're spawning a ball, finish the spawn and add physics.
            if(spawningBall)
            {
                Rigidbody rb = spawn.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                spawningBall = false;
            }
        }


	}

    private bool isThumbsUp(Hand h_)
    {
        if(h_.GrabAngle.NearlyEquals(Mathf.PI))
        {
            //we have a fist
            foreach(Finger f in h_.Fingers)
            {
                if(f.Type == Finger.FingerType.TYPE_THUMB)
                {
                    Vector3 thumbDirection = f.Direction.ToVector3();
                    float angle = Vector3.AngleBetween(Vector3.up, thumbDirection);
                    Debug.Log("Thumb Angle: " + angle);

                    if (angle >= 0.7f && angle <= 1.0f) 
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
