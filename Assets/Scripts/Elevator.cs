using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
    public int floorLevel;
    
	// Use this for initialization
	void Start () {
	if (tag == "One")
        {
            floorLevel = 1;
        }
    if (tag == "Two")
        {
            floorLevel = 2;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Running2D");
       // other.GetComponent<NonTarget>().SetCurrentFloor(floorLevel);
    }
  
}
