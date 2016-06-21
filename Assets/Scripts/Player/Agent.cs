using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    protected float gravity;
    public float moveSpeed = 6;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    protected float accelerationTimeAirborne = .2f;
    protected float accelerationTimeGrounded = .1f;

    protected float maxJumpVelocity;
    protected float minJumpVelocity;
    protected Vector3 velocity;

    // Use this for initialization
    public virtual void Start () {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
