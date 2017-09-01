using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SliceInfo
{
    public Vector3 Start;
    public Vector3 End;
}

public class SliceDetector : MonoBehaviour {

    private CooldownTimer timer = new CooldownTimer(1.0f, true);

    private Vector3? startPos = null;
    private Vector3? endPos = null;
    private bool swiping = false;

    private TrailRenderer trail;

    public float MaxSwipeDist = 5.0f;
    public float SwipeSpeed = 1.0f;
    public float SwipeTimer = 0.5f;
    public float MinDistance = 1.0f;

    public SliceInfo? GetSlice()
    {
        if (!this.swiping || !this.startPos.HasValue || !this.endPos.HasValue)
        {
            return null;
        }

        return new SliceInfo() { Start = this.startPos.Value, End = this.endPos.Value };
    }

	// Use this for initialization
	void Start () {
        trail = this.GetComponent<TrailRenderer>();
        this.timer.SetBaseline(SwipeTimer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (this.swiping)
        {
            return;
        }

	    if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).SetZ(0.0f);
            timer.Reset();            
            this.transform.position = startPos.Value;
        }

        timer.Tick(Time.deltaTime);

        if (startPos != null && (Input.GetMouseButtonUp(0) || timer.IsExpired))
        {
            Vector3 dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = dest - startPos.Value;

            if (direction.magnitude > MinDistance)
            {
                float dist = Mathf.Min(direction.magnitude, MaxSwipeDist);
                direction = direction.normalized * dist;
                this.endPos = this.startPos + direction;
                this.endPos = this.endPos.Value.SetZ(0.0f);
                this.swiping = true;
                StartCoroutine(Swipe());
            }
            else
            {
                timer.SetInactive();
                this.startPos = null;
            }
        }
	}

    IEnumerator Swipe()
    {        
        this.timer.SetInactive();
        while ((this.endPos.Value - this.transform.position).magnitude > 0.2f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.endPos.Value, SwipeSpeed);
            yield return null;
        }

        this.startPos = null;
        this.swiping = false;
    }
}
