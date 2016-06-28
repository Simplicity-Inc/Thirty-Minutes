using UnityEngine;
using System.Collections;

public class Controller2D : RayCastController {
    /// <summary>
    /// The max that you can climb up
    /// </summary>
    private float maxClimbAngle = 80;
    /// <summary>
    /// The max that you can climb down
    /// </summary>
    private float maxDescendAngle = 80;
    /// <summary>
    /// The maximum height the host can go up
    /// </summary>
    public float maxStepHeight = 1;
    public float ladderClimbSpeed = .5f;
    /// <summary>
    /// The ref to the collisions information
    /// </summary>
    public CollisionInfo collisions;
    /// <summary>
    /// A vector that holds the input to handle movement
    /// </summary>
    [HideInInspector] public Vector2 input;
    /// <summary>
    /// A bool to determine if a the controller is the player
    /// </summary>
    public bool isPlayer;
    /// <summary>
    /// A bool to determine if we should apply gravity to the host
    /// </summary>
    [HideInInspector] public bool applyGravity = true;

    public override void Start() {
        base.Start();
        collisions.faceDir = 1;
    }

    /// <summary>
    /// A function that moves the host of controller
    /// </summary>
    /// <param name="moveAmount">How fast we should be moving</param>
    /// <param name="standingOnPlatform">Are we standing on a platform?</param>
    public void Move(Vector2 moveAmount, bool standingOnPlatform) {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    /// <summary>
    /// A function that moves the host of controller
    /// </summary>
    /// <param name="moveAmount">How fast we should be moving</param>
    /// <param name="input">Where are we moving?</param>
    /// <param name="standingOnPlatform">Are we standing on a platform</param>
    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false) {
        // Updates the rays
        UpdateRaycastOrigins();
        // Reset the collisions info
        collisions.Reset();
        // Update the old moveAmount with last frames info
        collisions.moveAmountOld = moveAmount;
        // Set the passed in input to host's controller input
        this.input = input;
        // If we are moving then set our face direction to that way
        if(moveAmount.x != 0) { collisions.faceDir = (int)Mathf.Sign(moveAmount.x); }
        // We are moving down then call DescendSlope
        if(moveAmount.y < 0) { DescendSlope(ref moveAmount); }
        // Check horizontal collisions
        HorizontalCollisions(ref moveAmount);
        // If we are moving on the y axis, check for vertical collisions
        if(moveAmount.y != 0) { VerticalCollisions(ref moveAmount); }
        // Actually move the host of the controller
        transform.Translate(moveAmount);
        // If we are standing on something, set on collisions.below to be true
        if(standingOnPlatform) { collisions.below = true; }
    }
    
    /// <summary>
    /// Checks for horizontal collisions
    /// </summary>
    /// <param name="moveAmount">A ref to the controller's moveAmount</param>
    void HorizontalCollisions(ref Vector2 moveAmount) {
        // Set a varible to see which way we are going
        float directionX = collisions.faceDir;
        // Set a varible to see how far we should cast our rays
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
        // If we are moving faster than skinWidth, increase rayLength
        if(Mathf.Abs(moveAmount.x) < skinWidth) { rayLength = 2 * skinWidth; }
        // Check each ray
        for(int i = 0; i < horizontalRayCount; i++) {
            // Set which side we should send the ray from
            Vector2 rayOrigin = ( directionX == -1 ) ? RayCastOrigins.BottomLeft : RayCastOrigins.BottomRight;
            // Cast a ray along the controller's host side
            rayOrigin += Vector2.up * ( horizontalRaySpacing * i );
            // Check for ray cast collision
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            // Draw the ray in the editor
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            // If we do hit...
            if(hit) {
                // And we can climb it, then set our collision info to true - else false
                if(hit.collider.tag == "Climbable" && isPlayer) { collisions.climbable = true; } else collisions.climbable = false;
                // If we are right agaisnt just move on
                if(hit.distance == 0) { continue; }
                // Get the slope angle
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                // If its the lowest ray and we can climb that angle
                if(i == 0 && slopeAngle <= maxClimbAngle) {
                    // If we are descending
                    if(collisions.descendingSlope) {
                        // Exit this for next time
                        collisions.descendingSlope = false;
                        // Set the moveAmount to the last frame moveAmount
                        moveAmount = collisions.moveAmountOld;
                    }
                    // 
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                    moveAmount.x = ( hit.distance - skinWidth ) * directionX;
                    rayLength = hit.distance;

                    if(collisions.climbingSlope) { moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x); }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }

                if(hit.collider.tag == "Step" && isPlayer) { collisions.steppingUp = true; } else collisions.steppingUp = false;

                float stepHeight = hit.transform.gameObject.GetComponent<BoxCollider2D>().bounds.max.y - rayOrigin.y;
                if(i == 0 && stepHeight <= maxStepHeight && collisions.steppingUp) {
                    moveAmount.y += stepHeight + 0.05f;
                    moveAmount.x += ( directionX == -1 ) ? -.05f : .05f;
                    collisions.steppingUp = false;
                }
            } 
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for(int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = ( directionY == -1 ) ? RayCastOrigins.BottomLeft : RayCastOrigins.TopLeft;
            rayOrigin += Vector2.right * ( verticalRaySpacing * i + moveAmount.x );
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.yellow);

            if(hit) {
                if(hit.collider.tag == "Through") {
                    if(directionY == 1 || hit.distance == 0) { continue; }
                    if(collisions.fallingThroughPlatform) { continue; }
                    if(input.y == -1) {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }

                moveAmount.y = ( hit.distance - skinWidth ) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope) { moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x); }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if(collisions.climbingSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ( ( directionX == -1 ) ? RayCastOrigins.BottomLeft : RayCastOrigins.BottomRight ) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisions.slopeAngle) {
                    moveAmount.x = ( hit.distance - skinWidth ) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle) 
        {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(moveAmount.y <= climbmoveAmountY) {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmount) {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = ( directionX == -1 ) ? RayCastOrigins.BottomRight : RayCastOrigins.BottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if(hit) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
                if(Mathf.Sign(hit.normal.x) == directionX) {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set falling through platform bool to false
    /// </summary>
    void ResetFallingThroughPlatform() {
        collisions.fallingThroughPlatform = false;
    }

    /// <summary>
    /// A struct that holds all information about collisions
    /// </summary>
    public struct CollisionInfo {
        /// <summary>
        /// A value to see if we are colliding
        /// </summary>
        public bool above, below;
        /// <summary>
        /// A value to see if we are colliding
        /// </summary>
        public bool left, right;
        /// <summary>
        /// A value to determine if we are climbing up a slope
        /// </summary>
        public bool climbingSlope;
        /// <summary>
        /// A value to determine if we are climbing down a slope
        /// </summary>
        public bool descendingSlope;
        /// <summary>
        /// A value that holds information about slopes
        /// </summary>
        public float slopeAngle, slopeAngleOld;
        /// <summary>
        /// The bool to determine if we are stepping up
        /// </summary>        
        public bool steppingUp;
        /// <summary>
        /// A bool to determine if we are climbing a ladder
        /// </summary>
        public bool climbLadder;
        /// <summary>
        /// The value of our moveAmount the previous frame
        /// </summary
        public Vector2 moveAmountOld;
        /// <summary>
        /// The direction we are facing
        /// </summary>
        public int faceDir;
        /// <summary>
        /// A bool to determine if we are falling through a platform
        /// </summary>
        public bool fallingThroughPlatform;
        /// <summary>
        /// A bool to see if we are climbing something
        /// </summary>
        public bool climbable;

        /// <summary>
        /// A function to reset - 
        /// above, below, left, right, climbingSlope, descendingSlope - to false
        /// and set slopeAngle to slopeAngleOld and then slopeAngle to zero
        /// </summary>
        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            steppingUp = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}