using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderController : RayCastController {
    public LayerMask passengerMask;

    public float climbSpeed = 1;

    List<PassengerMovement> passengerMovement;

    public override void Start() {
        horizontalRayCount = verticalRayCount = (int)Mathf.Round(transform.localScale.x) * 10;
        horizontalRayCount = verticalRayCount = Mathf.Clamp(horizontalRayCount, 4, 9999);
        base.Start();
    }

    void Update() {
        UpdateRaycastOrigins();

        
    }

    struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool onLadder;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _onLadder) {
            this.transform = _transform;
            this.velocity = _velocity;
            this.onLadder = _onLadder;
        }
    }
}
