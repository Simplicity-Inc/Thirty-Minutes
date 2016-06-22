﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour {
    /// <summary>
    /// The layer we detect for when finding collisions
    /// </summary>
    public LayerMask collisionMask;
    /// <summary>
    /// The value used for the inset of the ray -
    /// We need this so when we are standing on something we still collide with it
    /// </summary>
    public const float skinWidth = .015f;
    /// <summary>
    /// The amount of rays that we shoot out to the side depending on which way we are moving -
    /// They are spaced evenly acrossed object using CalculateRaySpacing()
    /// </summary>
    public int horizontalRayCount = 4;
    /// <summary>
    /// The amount of rays that we shoot out up and down depend on which way we are moving -
    /// They are spaced evenly acrossed object using CalculateRaySpacing()
    /// </summary>
    public int verticalRayCount = 4;

    /// <summary>
    /// The spacing CalculateRaySpacing() determines for the horizontal rays
    /// </summary>
    [HideInInspector]
    public float horizontalRaySpacing;
    /// <summary>
    /// The spacing CalculateRaySpacing() determines for the vertical rays
    /// </summary>
    [HideInInspector]
    public float verticalRaySpacing;

    /// <summary>
    /// The private member for the object's collider
    /// </summary>
    [HideInInspector]
    private BoxCollider2D _collider;
    /// <summary>
    /// The Getter/Setter for the object's collider
    /// </summary>
    public BoxCollider2D Collider {
        get { return _collider; }
        set { _collider = value; }
    }
    /// <summary>
    /// The private member that holds the object's corners
    /// </summary>
    private RaycastOrigins _raycastOrigins;
    /// <summary>
    /// The Getter/Setter for the object's corners
    /// </summary>
    public RaycastOrigins RayCastOrigins {
        get { return _raycastOrigins; }
        set { _raycastOrigins = value; }
    }

    public virtual void Awake() { Collider = GetComponent<BoxCollider2D>(); }
    public virtual void Start() { CalculateRaySpacing(); }

    public void UpdateRaycastOrigins() {
        // Get the bounds of the collider
        Bounds bounds = Collider.bounds;
        // Shrink the bounds by twice the skinWidth
        bounds.Expand(skinWidth * -2);
        // Set the ray cast points
        RayCastOrigins = new RaycastOrigins(new Vector2(bounds.min.x, bounds.max.y)
                                           , new Vector2(bounds.max.x, bounds.max.y)
                                           , new Vector2(bounds.min.x, bounds.min.y)
                                           , new Vector2(bounds.max.x, bounds.min.y));
    }

    public void CalculateRaySpacing() {
        // Get the bounds of the collider
        Bounds bounds = Collider.bounds;
        // Shrink the bounds by twice the skinWidth
        bounds.Expand(skinWidth * -2);
        // Make sure we always have at least to rays for the corners
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        // Calculate the spacing based the bounds size by each ray count
        horizontalRaySpacing = bounds.size.y / ( horizontalRayCount - 1 );
        verticalRaySpacing = bounds.size.x / ( verticalRayCount - 1 );
    }

    public struct RaycastOrigins {
        // The constructor to the set the points
        public RaycastOrigins(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight) {
            this._topLeft = topLeft;
            this._topRight = topRight;
            this._bottomLeft = bottomLeft;
            this._bottomRight = bottomRight;
        }

        /// <summary>
        /// Private member for the top left point 
        /// </summary>
        private Vector2 _topLeft;
        /// <summary>
        /// The Getter/Setter for the top left point
        /// </summary>
        public Vector2 TopLeft {
            get { return _topLeft; }
            set { _topLeft = value; }
        }

        /// <summary>
        /// Private member for the top right point
        /// </summary>
        private Vector2 _topRight;
        /// <summary>
        /// The Getter/Setter for the top right point
        /// </summary>
        public Vector2 TopRight {
            get { return _topRight; }
            set { _topRight = value; }
        }

        /// <summary>
        /// Private member for the bottom left point
        /// </summary>
        private Vector2 _bottomLeft;
        /// <summary>
        /// The Getter/Setter for the bottom left point
        /// </summary>
        public Vector2 BottomLeft {
            get { return _bottomLeft; }
            set { _bottomLeft = value; }
        }

        /// <summary>
        /// Private member for the bottom right point
        /// </summary>
        private Vector2 _bottomRight;
        /// <summary>
        /// The Getter/Setter for the bottom right point
        /// </summary>
        public Vector2 BottomRight {
            get { return _bottomRight; }
            set { _bottomRight = value; }
        }
    }
}