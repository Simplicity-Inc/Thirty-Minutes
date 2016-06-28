using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerInput))]
public class Player : Agent {
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;

    float timeToWallUnstick;

    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    public override void Start() {
        controller = GetComponent<Controller2D>();
        base.Start();
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update() {

        CalcVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput); // HERE

        if(controller.collisions.above || controller.collisions.below) { velocity.y = 0; }
    }

    /// <summary>
    /// This method gets called by the PlayerInput component
    /// </summary>
    /// <param name="input"></param>
    public void SetDirectionalInput(Vector2 input) { directionalInput = input; }

    /// <summary>
    /// This method gets called by the PlayerInput component
    /// </summary>
    public void OnJumpInputDown() {
        if(wallSliding) {
            if(wallDirX == directionalInput.x) {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            } else if(directionalInput.x == 0) {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            } else {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if(controller.collisions.below) { velocity.y = maxJumpVelocity; }
    }

    /// <summary>
    /// This methods gets called by the PlayerInput component
    /// </summary>
    public void OnJumpInputUp() {
        if(velocity.y > minJumpVelocity) { velocity.y = minJumpVelocity; }
    }

    /// <summary>
    /// A function that handles all things to do with wall sliding
    /// </summary>
    private void HandleWallSliding() {
        wallDirX = ( controller.collisions.left ) ? -1 : 1;
        wallSliding = false;
        if(( controller.collisions.left || controller.collisions.right ) && !controller.collisions.below && velocity.y < 0 && controller.collisions.climbable) {
            wallSliding = true;

            if(velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if(timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if(directionalInput.x != wallDirX && directionalInput.x != 0) { timeToWallUnstick -= Time.deltaTime; } else { timeToWallUnstick = wallStickTime; }
            } else { timeToWallUnstick = wallStickTime; }
        }
    }

    /// <summary>
    /// Calculate the current velocity of the player
    /// </summary>
    private void CalcVelocity() {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, ( controller.collisions.below ) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}