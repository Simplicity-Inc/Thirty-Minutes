using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//change the array of targets to a list to change the size
[RequireComponent(typeof(Controller2D))]
public class AiBase : Agent {
    protected Transform target;

    public List<Transform> wayPointList = new List<Transform>();
    private int currWayPoint;

    public enum AIType { MEDIC, PEDESTRIAN };
    public AIType Type;
    public enum Behavior { WANDER, INTERACTABLES };
    public Behavior AI;
    protected Controller2D controller;
    public int wanderRadius;

    bool isAlive;

    Vector3 currentPos;
    Vector3 size;
	// Use this for initialization
	public override void Start () {
        controller = GetComponent<Controller2D>();
        base.Start();
        isAlive = true;
        size = GetComponent<Collider2D>().bounds.size;
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
                    break;
                }
            case Behavior.INTERACTABLES:
                {
                    WayPoints();
                    break;
                }
        }
    }
    void WayPoints()
    {
        if (currWayPoint < wayPointList.Count)
        {
            target = wayPointList[currWayPoint];
            string name = target.name;
            Debug.Log(name);
            if (wayPointList[currWayPoint].gameObject.activeSelf)
            {
                MoveTo(target.position);
            }
            else { currWayPoint++; }
            if (CheckXDis(target, transform) < .5)
            {
                if (target.GetComponent<Traps>().lethal)
                {
                    Die();
                }
                else { currWayPoint++; }
            }
            if (currWayPoint >= wayPointList.Count)
            {
                currWayPoint = 0;
            }
        }
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
        this.gameObject.SetActive(false);
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .5f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(wayPoint, .5f);
    }
}
