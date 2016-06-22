using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Controller2D))]
public class NonTarget : Agent {
    private Controller2D controller;
	// Use this for initialization
	public override void Start () {
        controller = GetComponent<Controller2D>();
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity, Vector2.zero);
        if (controller.collisions.above || controller.collisions.below) { velocity.y = 0; }
	}
}
