using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    /// <summary>
    /// The container for the follow targets controller
    /// </summary>
    public Controller2D target;
    /// <summary>
    /// The amount we offset the camera when following something
    /// </summary>
    public float lookAheadDstX;
    /// <summary>
    /// The amount of time it takes to get to the offset
    /// </summary>
    public float lookSmoothTimeX;
    /// <summary>
    /// The amount of time it takes to follow the target vertical
    /// </summary>
    public float verticalSmoothTime;
    /// <summary>
    /// The size of the tracking box
    /// </summary>
    public Vector2 focusAreaSize;
    /// <summary>
    /// The amount of vertical offset the camera has from the target position
    /// </summary>
    public float verticalOffset;

    /// <summary>
    /// The controller to handle the focus area size when moving
    /// </summary>
    private FocusArea _focusArea;
    /// <summary>
    /// The value of the current look ahead
    /// </summary>
    private float _currentLookAheadX;
    /// <summary>
    /// The value where we want to look ahead
    /// </summary>
    private float _targetLookAheadX;
    /// <summary>
    /// The value for determining the direction the camera forward looks
    /// </summary>
    private float _lookAheadDirX;
    /// <summary>
    /// The value of slerping the camera's X position
    /// </summary>
    private float _smoothLookVelocityX;
    /// <summary>
    /// The value of slerping the camera's Y position
    /// </summary>
    private float _smoothVelocityY;
    /// <summary>
    /// The value to see if we should stop moving the camera
    /// </summary>
    private bool _lookAheadStopped;

    void Start() {
        // Set the focus area based on the size of the target
        _focusArea = new FocusArea(target.Collider.bounds, focusAreaSize);
    }
    void LateUpdate() {
        // Update the focus based on the targets position
        _focusArea.Update(target.Collider.bounds);
        // Set up a container for the position we are going to using the verticalOffset;
        Vector2 focusPosition = _focusArea.center + Vector2.up * verticalOffset;
        // If the area is moving...
        if(_focusArea.velocity.x != 0) {
            // Determine which way we are moving
            _lookAheadDirX = Mathf.Sign(_focusArea.velocity.x);
            // If the target and focus area are moving the same way / And is moving
            if(Mathf.Sign(target.input.x) == Mathf.Sign(_focusArea.velocity.x) && target.input.x != 0) {
                // Start slerping camera
                _lookAheadStopped = false;
                // Set target position to look ahead direction squared
                _targetLookAheadX = _lookAheadDirX * lookAheadDstX;
            } else {
                // If we are stopping the movement
                if(!_lookAheadStopped) {
                    // Set it to true to exit this
                    _lookAheadStopped = true;
                    // Set are target
                    _targetLookAheadX = _currentLookAheadX + ( _lookAheadDirX * lookAheadDstX - _currentLookAheadX ) / 4.0f;
                }
            }
        }
        // Set the current to target
        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _targetLookAheadX, ref _smoothLookVelocityX, lookSmoothTimeX);
        // Change the focus area's position
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref _smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_focusArea.center, focusAreaSize);
    }

    public struct FocusArea {
        /// <summary>
        /// The position of the middle point
        /// </summary>
        public Vector2 center;
        /// <summary>
        /// The value for a corner
        /// </summary>
        float left, right;
        /// <summary>
        /// The value for a corner
        /// </summary>
        float top, bottom;
        /// <summary>
        /// How fast we are moving the focus area
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// The constructor for the FocusArea class
        /// </summary>
        /// <param name="targetBounds">The target's bounding box</param>
        /// <param name="size">How large the tracking box is</param>
        public FocusArea(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2(( left + right ) / 2, ( top + bottom ) / 2);
        }

        /// <summary>
        /// Custom update function
        /// </summary>
        /// <param name="targetBounds">The target's bounding box</param>
        public void Update(Bounds targetBounds) {
            // Init a value for the amount we will move the focus area
            float shiftX = 0;
            // If we are pushing on the left wall
            if(targetBounds.min.x < left) {
                // Set the shift value to move it that way
                shiftX = targetBounds.min.x - left;
            // Else if we are pushing on the right wall
            } else if(targetBounds.max.x > right) { 
                // Set the shift value to move it that way
                shiftX = targetBounds.max.x - right;
            }
            // Push the bounds using the shift value
            left += shiftX;
            right += shiftX;

            // Init a value for the amount we will move the focus area
            float shiftY = 0;
            // If we are pushing on the bottom wall
            if(targetBounds.min.y < bottom) {
                // Set the shift value to move that way
                shiftY = targetBounds.min.y - bottom;
            // Else if we are pushing on the top wall
            } else if(targetBounds.max.y > top) {
                // Set the shift value to move that way
                shiftY = targetBounds.max.y - top;
            }
            // Push the bounds using the shift value
            top += shiftY;
            bottom += shiftY;

            // Change the center value
            center = new Vector2(( left + right ) / 2, ( top + bottom ) / 2);
            // Change the velocity
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}