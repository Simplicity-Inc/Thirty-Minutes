using UnityEngine;
using System.Collections;

public class ElevatorMovement : PlatformController {

    int floorLevel = 0;
    public float pauseTime;
    private float Timer;
    bool pauseForPassengers;
    bool passengersWaiting;

	// Use this for initialization
	void Start () {
        base.Start();
        
	}

    // Update is called once per frame
    void Update()
    {
        base.Update();
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            floorLevel++;
        }
        if (percentBetweenWaypoints >= 1)
        {
            if (passengersWaiting)
            {
                Timer += Time.deltaTime;
                if (Timer >= pauseTime)
                {

                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Running2D");
        
        //other.GetComponent<NonTarget>().SetCurrentFloor(CheckCurrentFloor());
    }
    Vector3 CheckCurrentFloor()
    {
        Vector3 closest = Vector3.zero;
        float distance = Mathf.Infinity;
        foreach (Vector3 localWaypoint in localWaypoints)
                   {
           Vector3 diff = localWaypoint - transform.position;
           float curDistance = diff.sqrMagnitude;
                       if (curDistance < distance)
                           {
               closest = localWaypoint;
               distance = curDistance;
                           }
                   }
               return closest;
    }
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    int desiredFloor;
    //    desiredFloor = other.GetComponent<NonTarget>().GetDesiredFloor();
    //}
}
