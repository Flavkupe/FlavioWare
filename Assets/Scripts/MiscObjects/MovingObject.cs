using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovingObject : MonoBehaviour {

    public MovementType Movement;
    public float MoveSpeed = 3.0f;

    private Rigidbody2D body;

    private RectBounds camBounds;

    public enum MovementType
    {
        BounceAllOver,
    }    	

    protected virtual void BaseStart()
    {
        body = this.GetComponent<Rigidbody2D>();
        Camera cam = Camera.main;
        this.camBounds = cam.GetCamWorldBounds();
        body.velocity = new Vector2(1.0f, 1.0f) * MoveSpeed;
    }

    // Update is called once per frame
    protected virtual void BaseUpdate () {
		switch (Movement)
        {
            case MovementType.BounceAllOver:
            default:
                BounceAllOverUpdate();
                break;
        }
	}

    private void BounceAllOverUpdate()
    {
        if (this.transform.position.x > camBounds.RightBound && body.velocity.x > 0.0f)
        {
            body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
        }
        else if (this.transform.position.x < camBounds.LeftBound && body.velocity.x < 0.0f)
        {
            body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
        }
        else if (this.transform.position.y > camBounds.UpperBound && body.velocity.y > 0.0f)
        {
            body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
        }
        else if (this.transform.position.y < camBounds.LowerBound && body.velocity.y < 0.0f)
        {
            body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
        }
    }
}
