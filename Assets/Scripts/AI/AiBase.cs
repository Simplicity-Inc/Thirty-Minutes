using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Controller2D))]
public class AiBase : Agent {

    public int maxAccesibleFloor;
    public int minAccesibleFloor;

    protected int maxAlert;
    protected int minAlert;
    public int currentAlert;

    public List<GameObject> Targets = new List<GameObject>();

    protected Transform target;
    protected Vector3 wayPoint;
    public Transform[] InteractibleLocs;
    private int currWayPoint;

    bool interact;
    bool isTarget;

    public enum AIType { MEDIC, PEDESTRIAN };
    public AIType Type;
    public enum Behavior { WANDER, INTERACTABLES };
    public Behavior AI;
    protected Controller2D controller;
    public int wanderRadius;
    Material material;

    bool isAlive;

    Vector3 currentPos;
    Vector3 size;
	// Use this for initialization
	public override void Start () {
        controller = GetComponent<Controller2D>();
        base.Start();
        isAlive = true;
        wayPoint = Random.insideUnitCircle * wanderRadius;
        size = GetComponent<Collider2D>().bounds.size;
        material = GetComponent<Renderer>().material;
        currWayPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity, Vector2.zero);
        if (controller.collisions.above || controller.collisions.below) { velocity.y = 0; }
        currentPos = transform.position;
        switch (AI)
        {
            case Behavior.WANDER:
                {
                    MoveTo(wayPoint);
                    //MoveTo(FindClosestElevator().transform.position);
                    //head to elevator
                    if (CheckXDis(target,transform) < .4)
                    {
                        AI = Behavior.WANDER;
                    }
                    break;
                }
            case Behavior.INTERACTABLES:
                {
                    if (currWayPoint < InteractibleLocs.Length)
                    {
                        target = InteractibleLocs[currWayPoint];
                        MoveTo(target.position);
                        if (CheckXDis(target,transform) < .4)
                        {
                            currWayPoint++;
                            
                        }
                    }
                    break;
                }
        }
    }
    //Needs more work
    void InteractTest(int waitTime)
    {
                                
    }
    IEnumerator Interact()
        {
        //INCOMPLETE NEEDS TO WORK WITH THE ANIMATION
        //play the animation and check if its trapped
       // animation.Play("clip");
        //yield return new WaitForSeconds(animation["clip"].length * animation["clip"].speed);
        //if (target.GetComponent<Item>())
        //{
        //    //kill the target

        //}
        //else
        //{
        //    currWayPoint++;
        //}
        yield return new WaitForSeconds(2);
    }
    void MoveTo(Vector3 wayPoint)
    {
        if ((currentPos.x - wayPoint.x + (size.x/2)) > .5)
        {
            currentPos.x -= moveSpeed * Time.deltaTime;
            transform.position = currentPos;
           
        }
        if ((currentPos.x - wayPoint.x + (size.x/2)) < .5)
        {
            currentPos.x += moveSpeed * Time.deltaTime;
            transform.position = currentPos;
        }
    }
    float CheckXDis(Transform a, Transform b)
    {
        return Vector3.Distance(new Vector3(a.position.x, 0, 0), new Vector3(b.position.x, 0, 0));
    }
    void Stop()
    {
        currentPos.x += 0;
    }
    public void Die()
    {
        //death animation
        material.color = Color.red;
    }
    GameObject FindClosestElevator()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Elevator");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;

    }
    int DiceRoll(int min, int max)
    {
        int rand;
        rand = Random.Range(min, max);
        return rand;
    }  
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }
    void SetDesiredFloor()
    {
        desiredFloor = DiceRoll(minAccesibleFloor, maxAccesibleFloor);
    }
    public int GetDesiredFloor()
    {
        int floor;
        floor = desiredFloor;
        return floor;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(wayPoint, .5f);
    }
}
